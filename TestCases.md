# Stampin' Up Website Test Cases

## Test Scope
This document outlines comprehensive test cases for www.stampinup.com focusing on user account creation, profile management, and authentication functionalities.

## Test Environment
- **Website**: www.stampinup.com
- **Testing Framework**: NUnit with C# and Selenium WebDriver
- **Browser**: Chrome (latest version)
- **Test Data**: Generated test emails and predefined user information

---

## New Customer Account Creation Test Cases

### TC-001: Create New Customer Account
**Objective**: Verify that a new user can successfully create an account on Stampin' Up website

**Preconditions**: 
- User is not logged in
- Valid email address is available for registration

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Click on "Create Account" button
3. Fill in the registration form:
   - First Name: "John"
   - Last Name: "Doe" 
   - Email: [Generated unique email]
   - Password: "Password123!"
   - Confirm Password: "Password123!"
4. Click "Submit" button
5. Wait for account creation confirmation
6. Navigate to account settings page

**Expected Result**: 
- Account is created successfully
- User is redirected to account settings page
- URL contains "account/settings"

**Test Data**: 
- Email: Dynamically generated (format: test_[random6chars][random3digits]@mail.com)
- Password: Password123!

---

### TC-003: Add Address to New User Profile
**Objective**: Verify that a newly created user can add address information to their profile

**Preconditions**: 
- User has successfully created an account
- User is logged in

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/address/create
2. Click action button to proceed
3. Fill in address form:
   - First Name: "John"
   - Last Name: "Doe"
   - Address Line 1: "123 Main Street"
   - Address Line 2: "Apt 4B"
   - City: "Los Angeles"
   - State: "California"
   - Postal Code: "90001"
   - Phone: "+1234567890"
4. Click "Save Address" button
5. Verify address appears in address list

**Expected Result**: 
- Address is saved successfully
- Address appears in the user's address list
- Address list contains at least one address entry

---

### TC-004: Edit Existing Address
**Objective**: Verify that a user can edit an existing address in their profile

**Preconditions**: 
- User is logged in
- User has at least one address saved in their profile

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/address
2. Locate the first address entry
3. Click "Edit" button for the address
4. Update address fields:
   - First Name: "Jane"
   - Last Name: "Smith"
   - Address Line 1: "456 Elm Street"
   - City: "San Francisco"
   - State: "California"
   - Postal Code: "94105"
   - Phone: "+1987654321"
5. Click "Save Address" button
6. Verify updated information appears in address list

**Expected Result**: 
- Address is updated successfully
- Updated information is displayed in the address list
- Changes are persisted after page refresh

---

## Existing User Account Test Cases

### TC-002: Login with Existing Account
**Objective**: Verify that an existing user can successfully log into their account

**Preconditions**: 
- Valid user account exists in the system
- User has correct login credentials

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Enter email address in email field
3. Enter password in password field
4. Click "Login" button
5. Wait for authentication process
6. Click action button if prompted
7. Navigate to account settings

**Expected Result**: 
- User is authenticated successfully
- User is redirected to account settings page
- URL contains "account/settings"

**Test Data**: 
- Email: test_udgfqa876@mail.com
- Password: Password123!

---

### TC-007: Edit First Name in Account Settings
**Objective**: Verify that an existing user can edit their first name in account settings

**Preconditions**: 
- User is logged in to their account
- User has access to account settings

**Test Steps**:
1. Navigate to account settings page
2. Click "Edit Contact Settings" button
3. Locate first name field
4. Clear current first name value
5. Enter updated first name (append random number to existing name)
6. Click "Save Changes" button
7. Verify updated first name appears in account information

**Expected Result**: 
- First name is updated successfully
- Updated first name is displayed in account information
- Changes are saved and persisted

---

### TC-008: Edit Last Name in Account Settings
**Objective**: Verify that an existing user can edit their last name in account settings

**Preconditions**: 
- User is logged in to their account
- User has access to account settings

**Test Steps**:
1. Navigate to account settings page
2. Click "Edit Contact Settings" button
3. Locate last name field
4. Clear current last name value
5. Enter updated last name (append random number to existing name)
6. Click "Save Changes" button
7. Verify updated last name appears in account information

**Expected Result**: 
- Last name is updated successfully
- Updated last name is displayed in account information
- Changes are saved and persisted

---

## Negative Test Cases

### TC-005: Login with Invalid Credentials
**Objective**: Verify that login fails when incorrect credentials are provided

**Preconditions**: 
- User is not logged in
- Invalid credentials are used

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Enter invalid email address: "invalid_creds@gmail.com"
3. Enter password: "Password123!"
4. Click "Login" button
5. Wait for authentication attempt
6. Try to navigate to account settings page

**Expected Result**: 
- Login attempt fails
- User is not authenticated
- User cannot access account settings
- URL does not contain "account/settings"

---

### TC-006: Login with Empty Email
**Objective**: Verify that form validation prevents login with empty email field

**Preconditions**: 
- User is on login page

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Leave email field empty
3. Enter password: "Password123!"
4. Attempt to click "Login" button
5. Check form validation

**Expected Result**: 
- Form validation prevents submission
- Email field shows validation error
- Login button may be disabled or form submission blocked

---

### TC-007: Login with Empty Password
**Objective**: Verify that form validation prevents login with empty password field

**Preconditions**: 
- User is on login page

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Enter email: "valid@email.com"
3. Leave password field empty
4. Attempt to click "Login" button
5. Check form validation

**Expected Result**: 
- Form validation prevents submission
- Password field shows validation error
- Login button may be disabled or form submission blocked

---

### TC-008: Login with Invalid Email Format
**Objective**: Verify that form validation prevents login with improperly formatted email

**Preconditions**: 
- User is on login page

**Test Steps**:
1. Navigate to https://www.stampinup.com/account/settings
2. Enter invalid email format: "invalid-email-format"
3. Enter password: "Password123!"
4. Attempt to click "Login" button
5. Check form validation

**Expected Result**: 
- Form validation prevents submission
- Email field shows format validation error
- Login button may be disabled or form submission blocked

---

## Test Data Management

### User Account Data
- **Test Email Pattern**: test_[6 random lowercase letters][3 random digits]@mail.com
- **Test Password**: Password123!
- **Test Names**: John Doe, Jane Smith
- **Test Addresses**: 
  - 123 Main Street, Apt 4B, Los Angeles, CA 90001
  - 456 Elm Street, San Francisco, CA 94105

### Address Information
- **Phone Numbers**: +1234567890, +1987654321
- **States**: California
- **Cities**: Los Angeles, San Francisco
- **Postal Codes**: 90001, 94105

---

## Test Environment Requirements

### Software Requirements
- **.NET Framework**: 8.0 or higher
- **NUnit**: 3.13.3
- **Selenium WebDriver**: 4.15.0
- **Chrome Browser**: Latest version
- **ChromeDriver**: Compatible with Chrome version

### Test Execution Notes
- Tests include deliberate wait times to handle page loading
- Random email generation prevents account conflicts
- Tests use data-testid attributes for element identification
- JavaScript execution used for reliable form field clearing

---

## Risk Mitigation

### Known Limitations
- Tests depend on specific data-testid attributes in the DOM
- Network latency may affect test execution timing
- Dynamic content loading requires sufficient wait times
- Email uniqueness is handled through random generation

### Test Maintenance
- Regular updates required if UI elements change
- Test data should be refreshed periodically
- Execution environment should match production browser versions
- Monitor for website updates that may affect element selectors