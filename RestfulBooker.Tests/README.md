\# RESTful Booker BDD Automation Framework



A C# Behaviour-Driven Development automation framework for testing the RESTful Booker application.



The framework combines:



\- Reqnroll

\- NUnit

\- RestSharp

\- Selenium WebDriver

\- Serilog

\- Allure Reports

\- Extent Reports

\- Native Reqnroll Reports



The project contains both API automation and a small Selenium UI smoke test.



\---



\## Technologies



| Technology | Purpose |

|---|---|

| C# / .NET 8 | Programming language and runtime |

| Reqnroll | BDD framework |

| NUnit | Test execution and assertions |

| RestSharp | REST API automation |

| Selenium WebDriver | UI browser automation |

| Serilog | Console and file logging |

| Allure | Test reporting and failure evidence |

| Extent Reports | HTML test dashboard |

| Reqnroll Native Reports | BDD scenario reporting |



\---



\## Application Under Test



RESTful Booker:



```text

https://restful-booker.herokuapp.com

```



The framework tests both the public landing page and REST API.



\---



\## Project Structure



```text

RESTfulBookerBDD/

│

├── RestfulBooker.Tests/

│   │

│   ├── Api/

│   │   └── BookerApiClient.cs

│   │

│   ├── Config/

│   │   └── TestSettings.cs

│   │

│   ├── Features/

│   │   ├── ApiHealth.feature

│   │   ├── BookingLifecycle.feature

│   │   └── UiSmoke.feature

│   │

│   ├── Hooks/

│   │   ├── ExtentHooks.cs

│   │   └── TestHooks.cs

│   │

│   ├── Models/

│   │   └── BookingModels.cs

│   │

│   ├── Pages/

│   │   └── HomePage.cs

│   │

│   ├── StepDefinitions/

│   │   ├── ApiSteps.cs

│   │   └── UiSteps.cs

│   │

│   ├── Support/

│   │   ├── Dependencies.cs

│   │   ├── DriverFactory.cs

│   │   ├── ExtentReport.cs

│   │   ├── Log.cs

│   │   └── ScenarioState.cs

│   │

│   ├── ApiSmokeTest.cs

│   ├── UiSmokeTest.cs

│   ├── appsettings.json

│   ├── allureConfig.json

│   ├── reqnroll.json

│   └── RestfulBooker.Tests.csproj

│

├── .gitignore

└── README.md

```



\---



\# BDD Scenarios



\## API Health



Tags:



```text

@api @smoke

```



Checks that the RESTful Booker service is available.



```gherkin

When I call the booking service health endpoint

Then the health response should be successful

```



Expected result:



```text

GET /ping → HTTP 201

```



\---



\## Booking Lifecycle



Tags:



```text

@api @regression

```



Tests the complete booking lifecycle:



```text

PING

&#x20;↓

AUTH

&#x20;↓

POST

&#x20;↓

GET

&#x20;↓

PUT

&#x20;↓

GET

&#x20;↓

PATCH

&#x20;↓

GET

&#x20;↓

DELETE

&#x20;↓

GET → 404

```



The scenario:



1\. Checks the booking service is available

2\. Authenticates and receives an admin token

3\. Creates a unique booking

4\. Retrieves the booking

5\. Fully replaces the booking

6\. Confirms the replacement was persisted

7\. Partially updates selected fields

8\. Confirms the partial update was persisted

9\. Deletes the booking

10\. Confirms the deleted booking returns HTTP 404



\---



\## Negative API Test



Tag:



```text

@negative

```



Tests that updating a booking without authentication is rejected.



Expected response:



```text

HTTP 401 or 403

```



\---



\## UI Smoke Test



Tags:



```text

@ui @smoke

```



Uses Selenium WebDriver to:



\- Open the RESTful Booker landing page

\- Confirm the page identifies RESTful Booker

\- Confirm the page contains API learning content



Chrome runs headlessly by default.



\---



\# Configuration



Configuration is stored in:



```text

RestfulBooker.Tests/appsettings.json

```



Current configuration:



```json

{

&#x20; "BaseUrl": "https://restful-booker.herokuapp.com",

&#x20; "Browser": "chrome",

&#x20; "Headless": true

}

```



To see Chrome during the Selenium test, change:



```json

"Headless": true

```



to:



```json

"Headless": false

```



\---



\# Run Commands



Navigate to the test project:



```bash

cd RestfulBooker.Tests

```



\## Restore Packages



```bash

dotnet restore

```



\## Build



```bash

dotnet build

```



\## Run All Tests



```bash

dotnet test

```



Expected:



```text

total: 6

failed: 0

succeeded: 6

```



\---



\# Run Tests by Tag



Reqnroll tags are exposed as NUnit test categories.



\## Smoke Tests



```bash

dotnet test --filter "TestCategory=smoke"

```



Runs:



\- API health smoke scenario

\- Selenium UI smoke scenario



\---



\## API Tests



```bash

dotnet test --filter "TestCategory=api"

```



Runs:



\- API health

\- Booking lifecycle

\- Negative authentication scenario



\---



\## Regression Tests



```bash

dotnet test --filter "TestCategory=api\&TestCategory=regression"

```



\---



\## Negative Test



```bash

dotnet test --filter "TestCategory=negative"

```



\---



\## UI Test



```bash

dotnet test --filter "TestCategory=ui"

```



\---



\# API Endpoints Tested



| Method | Endpoint | Purpose |

|---|---|---|

| GET | `/ping` | Service health |

| POST | `/auth` | Authentication |

| POST | `/booking` | Create booking |

| GET | `/booking/{id}` | Retrieve booking |

| PUT | `/booking/{id}` | Full update |

| PATCH | `/booking/{id}` | Partial update |

| DELETE | `/booking/{id}` | Delete booking |



\---



\# Logging



Serilog records API activity to both the console and log files.



Example:



```text

GET /ping

Status 201; Body Created

```



Generated logs are located under the test output directory, for example:



```text

bin/Debug/net8.0/logs/

```



Generated logs are excluded from Git.



\---



\# Native Reqnroll Report



Running:



```bash

dotnet test

```



generates the native Reqnroll report.



Location:



```text

bin/Debug/net8.0/TestResults/reqnroll-report.html

```



Open from Git Bash:



```bash

start "" "bin/Debug/net8.0/TestResults/reqnroll-report.html"

```



Reqnroll also generates:



```text

bin/Debug/net8.0/TestResults/reqnroll.ndjson

```



\---



\# Extent Report



The Extent dashboard is generated after the Reqnroll test run.



Location:



```text

bin/Debug/net8.0/TestResults/extent-report.html

```



Open it with:



```bash

start "" "bin/Debug/net8.0/TestResults/extent-report.html"

```



The report displays:



\- BDD scenario names

\- Tags

\- Scenario steps

\- Pass/fail status

\- Execution duration



\---



\# Allure Report



Allure result files are generated under:



```text

bin/Debug/net8.0/allure-results

```



After running the tests:



```bash

dotnet test

```



serve the report with:



```bash

allure serve bin/Debug/net8.0/allure-results

```



Allure provides:



\- Scenario results

\- Detailed execution information

\- Failure diagnostics

\- Attachments



\---



\# Failure Screenshots



Failed `@ui` scenarios automatically trigger the Reqnroll UI hook.



The framework:



```text

UI scenario fails

&#x20;       ↓

TestHooks detects failure

&#x20;       ↓

Selenium captures screenshot

&#x20;       ↓

Screenshot attached to Allure

&#x20;       ↓

Browser closes automatically

```



Failure screenshot attachment was verified using a controlled failing UI scenario.



\---



\# Scenario State



`ScenarioState` stores information that needs to be shared between BDD steps within the same scenario.



Examples include:



```text

Admin token

Booking ID

Expected booking

Latest booking

Last API response

```



Each scenario receives its own state to prevent test data leaking between scenarios.



\---



\# API Client



`BookerApiClient` centralises HTTP communication with RESTful Booker.



Step definitions do not contain repeated raw HTTP request code.



Instead:



```text

Feature

&#x20;  ↓

Step Definition

&#x20;  ↓

BookerApiClient

&#x20;  ↓

RestSharp

&#x20;  ↓

RESTful Booker

```



\---



\# Page Object Model



Selenium interactions are separated from the BDD step definitions.



```text

UiSmoke.feature

&#x20;     ↓

UiSteps.cs

&#x20;     ↓

HomePage.cs

&#x20;     ↓

DriverFactory.cs

&#x20;     ↓

Chrome

```



This keeps the framework easier to maintain and reuse.



\---



\# Git Workflow



Development uses feature branches.



Typical workflow:



```bash

git checkout main

git pull origin main

git checkout -b feature/example

```



After completing and testing the change:



```bash

git add .

git commit -m "Describe change"

git push -u origin feature/example

```



Then:



```text

Create Pull Request

→ Review

→ Merge to main

```



Update the local repository:



```bash

git checkout main

git pull origin main

```



\---



\# Current Test Status



Current automated suite:



```text

6 tests

0 failures

```



This includes:



\- 4 Reqnroll BDD scenarios

\- 1 standalone API smoke test

\- 1 standalone Selenium smoke test



The BDD suite covers:



```text

Health check           ✅

Authentication         ✅

Create booking         ✅

Retrieve booking       ✅

Full update            ✅

Partial update         ✅

Delete booking         ✅

Deleted booking → 404  ✅

Unauthenticated update ✅

UI landing page        ✅

```



\---



\# Reports Available



The framework currently provides three reporting layers:



| Report | Purpose |

|---|---|

| Reqnroll HTML | BDD scenario and step readability |

| Extent Report | Stakeholder-friendly execution dashboard |

| Allure | Detailed diagnostics and failure attachments |



\---



\# Notes



RESTful Booker is a shared public testing service.



Its test data may reset periodically and bookings created by other learners may also be present.



For this reason, the framework:



\- Generates unique booking data

\- Does not depend on pre-existing booking IDs

\- Uses future booking dates

\- Deletes created bookings during the lifecycle scenario

