using System.Text.Json;
using Json.Schema;

namespace RestfulBooker.Tests.Support;

public static class JsonSchemaValidator
{
    public static void Validate(
        string json,
        string schemaFileName)
    {
        var schemaPath = Path.Combine(
            AppContext.BaseDirectory,
            "Schemas",
            schemaFileName);

        if (!File.Exists(schemaPath))
        {
            throw new FileNotFoundException(
                $"JSON schema file was not found: {schemaPath}");
        }

        var schema =
            JsonSchema.FromFile(schemaPath);

        using var jsonDocument =
            JsonDocument.Parse(json);

        var results =
            schema.Evaluate(
                jsonDocument.RootElement);

        Assert.That(
            results.IsValid,
            Is.True,
            $"The API response does not match the JSON schema '{schemaFileName}'.");
    }
}