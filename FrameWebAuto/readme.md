# FrameVR Automation Tests

Playwright automated test suite for [framevr.io](https://framevr.io) built with C# and NUnit.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PowerShell 7](https://github.com/PowerShell/PowerShell/releases) (required for Playwright browser install)
- Git

## Setup

### 1. Clone the repository
```
git clone <your-repo-url>
cd <repo-folder>
```

### 2. Install dependencies
```
dotnet restore
```

### 3. Install Playwright browsers
```
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install
```

### 4. Set environment variables

PowerShell:
```
$env:FRAME_AUTOMATION_EMAIL="your@email.com"
$env:FRAME_AUTOMATION_PASSWORD="yourpassword"
```

Command Prompt:
```
set FRAME_AUTOMATION_EMAIL=your@email.com
set FRAME_AUTOMATION_PASSWORD=yourpassword
```

> Environment variables must be set in the same terminal session you run tests from.

## Running Tests

Run all tests:
```
dotnet test
```

Run with visible browser (headed mode):
```
$env:HEADED=1
dotnet test
```

Run a specific test:
```
dotnet test --filter "TestName"
```

## Test Coverage

| Test | Description |
|------|-------------|
| `LoginAuthSuccess` | Valid credentials log in successfully |
| `LoginBadCredential` | Invalid password shows error message |
| `LogOutSuccess` | Authenticated user can log out |

## Project Structure

```
FrameWebAuto/
├── LoginTests.cs        # Login and authentication tests
└── FrameWebAuto.csproj  # Project dependencies
```

## Notes

- Tests run in Chromium by default
- SlowMo and Headless settings are in GlobalSetUp inside LoginTests.cs
- To run headless, set Headless = true in GlobalSetUp