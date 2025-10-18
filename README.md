# Stampin' Up Website Test Automation Suite

## Overview
This is a comprehensive C# test automation suite for testing the Stampin' Up website (www.stampinup.com) using NUnit framework and Selenium WebDriver. The test suite covers account creation, user profile management, and authentication scenarios as specified in the SDET assessment requirements.

## Project Structure
```
Stampup/
├── StampinUpTests.sln                    # Visual Studio solution file
├── README.md                             # This documentation file
├── TestCases.md                          # Detailed written test cases
└── StampinUpTests/                       # Main test project
    ├── StampinUpTests.csproj             # Project configuration file
    ├── Helpers/
    │   └── WebDriverHelper.cs            # Selenium WebDriver utility methods
    └── Tests/
        ├── NewAccountTests.cs            # Tests for new account creation and setup
        ├── ExistingAccountTests.cs       # Tests for existing account operations
        └── InvalidCredentialsTests.cs    # Negative tests for authentication
```

## Test Scenarios Covered

### New Account Creation
- **TC-001**: Create new customer account
- **TC-003**: Add address to user profile
- **TC-004**: Edit existing address

### Existing Account Management
- **TC-002**: Login with existing account
- **TC-007**: Edit first name in account settings
- **TC-008**: Edit last name in account settings

### Negative Testing
- **TC-005**: Login with invalid credentials
- **TC-006**: Login with empty email
- **TC-007**: Login with empty password

### Registration Validation
- **TC-009**: Registration with empty first name should fail
- **TC-010**: Registration with invalid email format should fail
- **TC-011**: Registration with mismatched passwords should fail
- **TC-012**: Registration with weak password should fail
- **TC-013**: Registration with existing email should fail

### Login Validation
- **TC-014**: Login with empty email should fail
- **TC-015**: Login with empty password should fail
- **TC-016**: Login with invalid email format should fail
- **TC-017**: Login with nonexistent email should fail
- **TC-018**: Login with wrong password should fail
- **TC-019**: Login with SQL injection attempt should fail safely
- **TC-020**: Login with XSS attempt should be sanitized
- **TC-021**: Login with extremely long input should be handled

### Account Management Validation
- **TC-022**: Edit first name with empty value should fail
- **TC-023**: Edit last name with empty value should fail
- **TC-024**: Edit name with special characters should be handled
- **TC-025**: Edit name with extremely long values should be handled
- **TC-026**: Edit name with numeric values should be handled
- **TC-027**: Save changes without modifications should work
- **TC-028**: Cancel editing should revert changes

### Address Management Validation
- **TC-029**: Add address with empty first name should fail
- **TC-030**: Add address with empty street address should fail
- **TC-031**: Add address with invalid postal code should fail
- **TC-032**: Add address with invalid phone number should fail
- **TC-033**: Add address with special characters in name fields
- **TC-034**: Add address with extremely long values should be handled
- **TC-035**: Edit address with empty required fields should fail
- **TC-036**: Delete address should require confirmation

## Prerequisites

### Software Requirements
- **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **Google Chrome** browser (latest version)

### Verify .NET Installation
```bash
dotnet --version
# Should return 9.0.x or higher (You have: 9.0.8)
```

## Setup and Execution

### 1. Navigate to Project Directory
```bash
cd /Users/laxmanvytla/Downloads/Stampup
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run All Tests with Detailed Output
```bash
dotnet test --logger:console;verbosity=detailed
```

### Run Tests by Category
```bash
# New account creation tests
dotnet test --filter Category=NewAccount

# Existing account management tests
dotnet test --filter Category=ExistingAccount

# Negative/validation tests
dotnet test --filter Category=NegativeTest

# Registration validation tests
dotnet test --filter Category=RegistrationValidation

# Login validation tests
dotnet test --filter Category=LoginValidation

# Account management validation tests
dotnet test --filter Category=AccountManagement

# Address management validation tests
dotnet test --filter Category=AddressValidation

# Debug and investigation tests
dotnet test --filter Category=Debug
```

## Run Individual Tests

### New Account Tests
```bash
# TC-001: Create new customer account
dotnet test --filter Name=CreateNewCustomerAccount_ShouldSucceed

# TC-003: Add address to user profile
dotnet test --filter Name=AddAddressToUserProfile_ShouldSucceed

# TC-004: Edit existing address
dotnet test --filter Name=EditExistingAddress_ShouldSucceed
```

### Existing Account Tests
```bash
# TC-002: Login with existing account
dotnet test --filter Name=LoginWithExistingAccount_ShouldSucceed

# TC-007: Edit first name in account settings
dotnet test --filter Name=EditFirstNameField_ShouldSucceed

# TC-008: Edit last name in account settings
dotnet test --filter Name=EditLastNameField_ShouldSucceed
```

### Negative Tests
```bash
# TC-005: Login with invalid credentials
dotnet test --filter Name=LoginWithInvalidCredentials_ShouldFail

# TC-006: Login with empty email
dotnet test --filter Name=LoginWithEmptyEmail_ShouldFail

# TC-007: Login with empty password
dotnet test --filter Name=LoginWithEmptyPassword_ShouldFail
```

### Registration Validation Tests
```bash
# TC-009: Registration with empty first name
dotnet test --filter Name=RegistrationWithEmptyFirstName_ShouldFail

# TC-010: Registration with invalid email format
dotnet test --filter Name=RegistrationWithInvalidEmail_ShouldFail

# TC-011: Registration with mismatched passwords
dotnet test --filter Name=RegistrationWithMismatchedPasswords_ShouldFail

# TC-012: Registration with weak password
dotnet test --filter Name=RegistrationWithWeakPassword_ShouldFail

# TC-013: Registration with existing email
dotnet test --filter Name=RegistrationWithExistingEmail_ShouldFail
```

### Login Validation Tests
```bash
# TC-014: Login with empty email
dotnet test --filter Name=LoginWithEmptyEmail_ShouldFail

# TC-015: Login with empty password
dotnet test --filter Name=LoginWithEmptyPassword_ShouldFail

# TC-016: Login with invalid email format
dotnet test --filter Name=LoginWithInvalidEmailFormat_ShouldFail

# TC-017: Login with nonexistent email
dotnet test --filter Name=LoginWithNonexistentEmail_ShouldFail

# TC-018: Login with wrong password
dotnet test --filter Name=LoginWithWrongPassword_ShouldFail

# TC-019: Login with SQL injection attempt
dotnet test --filter Name=LoginWithSQLInjection_ShouldFailSafely

# TC-020: Login with XSS attempt
dotnet test --filter Name=LoginWithXSSAttempt_ShouldBeSanitized

# TC-021: Login with extremely long input
dotnet test --filter Name=LoginWithExtremelyLongInput_ShouldBeHandled
```

### Account Management Validation Tests
```bash
# TC-022: Edit first name with empty value
dotnet test --filter Name=EditFirstNameWithEmptyValue_ShouldFail

# TC-023: Edit last name with empty value
dotnet test --filter Name=EditLastNameWithEmptyValue_ShouldFail

# TC-024: Edit first name with special characters
dotnet test --filter Name=EditFirstNameWithSpecialCharacters_ShouldBeHandled

# TC-025: Edit name with extremely long values
dotnet test --filter Name=EditNameWithExtremelyLongValue_ShouldBeHandled

# TC-026: Edit name with numeric values
dotnet test --filter Name=EditNameWithNumericValues_ShouldBeHandled

# TC-027: Save changes without modifications
dotnet test --filter Name=SaveChangesWithoutModifications_ShouldWork

# TC-028: Cancel editing should revert changes
dotnet test --filter Name=CancelEditingShouldRevertChanges
```

### Address Management Validation Tests
```bash
# TC-029: Add address with empty first name
dotnet test --filter Name=AddAddressWithEmptyFirstName_ShouldFail

# TC-030: Add address with empty street address
dotnet test --filter Name=AddAddressWithEmptyStreetAddress_ShouldFail

# TC-031: Add address with invalid postal code
dotnet test --filter Name=AddAddressWithInvalidPostalCode_ShouldFail

# TC-032: Add address with invalid phone number
dotnet test --filter Name=AddAddressWithInvalidPhoneNumber_ShouldFail

# TC-033: Add address with special characters in name
dotnet test --filter Name=AddAddressWithSpecialCharactersInName_ShouldBeHandled

# TC-034: Add address with extremely long values
dotnet test --filter Name=AddAddressWithExtremelyLongValues_ShouldBeHandled

# TC-035: Edit address with empty required fields
dotnet test --filter Name=EditAddressWithEmptyRequiredFields_ShouldFail

# TC-036: Delete address should require confirmation
dotnet test --filter Name=DeleteAddressShouldRequireConfirmation
```

### Debug Tests
```bash
# Debug home page behavior
dotnet test --filter Name=DebugHomePage_ShouldShowWhatBrowserSees
```

## Test Configuration

### Chrome Browser Setup
The tests automatically handle Chrome browser setup:
- Downloads and configures ChromeDriver
- Sets optimal Chrome options for testing
- Implements anti-detection measures

### Test Data
- **Email Generation**: Automatic generation of unique test emails (pattern: test_[6chars][3digits]@mail.com)
- **Existing Account**: test_udgfqa876@mail.com / Password123!
- **Test Addresses**: Predefined address data for consistency

### Wait Strategies
- **Default timeout**: 15 seconds for element interactions
- **Page load timeout**: 30 seconds
- **Custom delays**: 2-4 seconds between actions (randomized)

## Troubleshooting

### Common Issues

#### 1. ChromeDriver Version Mismatch
```bash
dotnet add StampinUpTests package Selenium.WebDriver.ChromeDriver
```

#### 2. Test Timeouts
- Check internet connection
- Verify www.stampinup.com is accessible
- Increase timeout values in `/Users/laxmanvytla/Downloads/Stampup/StampinUpTests/Helpers/WebDriverHelper.cs` if needed

#### 3. Element Not Found Errors
- Website UI may have changed
- Check if data-testid attributes still exist
- Update selectors in test files if necessary

#### 4. Permission Issues (macOS)
```bash
chmod +x /Users/laxmanvytla/.nuget/packages/selenium.webdriver.chromedriver/*/driver/*/chromedriver
```

## Test Results

### Console Output
Tests provide detailed console output including:
- 🔹 Test execution progress
- ✅ Success confirmations
- ❌ Failure details with specific error messages
- Email addresses used for account creation

### List All Available Tests
```bash
dotnet test --list-tests
```

## Quick Test Commands Reference

```bash
# Setup
cd /Users/laxmanvytla/Downloads/Stampup
dotnet restore
dotnet build

# Run all tests
dotnet test

# Run by category
dotnet test --filter Category=NewAccount
dotnet test --filter Category=ExistingAccount
dotnet test --filter Category=NegativeTest
dotnet test --filter Category=RegistrationValidation
dotnet test --filter Category=LoginValidation
dotnet test --filter Category=AccountManagement
dotnet test --filter Category=AddressValidation

# Run individual tests (examples)
dotnet test --filter Name=CreateNewCustomerAccount_ShouldSucceed
dotnet test --filter Name=LoginWithExistingAccount_ShouldSucceed
dotnet test --filter Name=LoginWithInvalidCredentials_ShouldFail
dotnet test --filter Name=RegistrationWithEmptyFirstName_ShouldFail
dotnet test --filter Name=EditFirstNameWithEmptyValue_ShouldFail
```

## Assessment Submission

This test suite fulfills all requirements for the Stampin' Up SDET assessment:

✅ **Written Test Cases**: Detailed scenarios in `TestCases.md`  
✅ **Account Creation**: Comprehensive new user registration testing  
✅ **Profile Setup**: Address management and account settings  
✅ **Existing User Scenarios**: Login and profile editing functionality  
✅ **Modern Framework**: C# with NUnit and Selenium WebDriver  
✅ **Complete Documentation**: Implementation and usage guides  

---
