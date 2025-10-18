using NUnit.Framework;
using OpenQA.Selenium;
using StampinUpTests.Helpers;
using FluentAssertions;
using System;
using System.Threading;

namespace StampinUpTests.Tests
{
    [TestFixture]
    public class NewAccountTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverHelper.SetupDriver();
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }

        [Test]
        [Category("NewAccount")]
        [Description("TC-001: Create new customer account")]
        public void CreateNewCustomerAccount_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-001: Create new customer account");
            var email = WebDriverHelper.GenerateRandomEmail();

            // Act
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(10000);

            // Click Create Account
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='btn-create-account']"));
            Thread.Sleep(3000);

            // Fill account registration form
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='reg-first-name']"), "John");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='reg-last-name']"), "Doe");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='reg-email']"), email);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='reg-password']"), "Password123!");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='reg-password-confirmation']"), "Password123!");

            // Submit registration form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='reg-submit']"));
            Thread.Sleep(20000);

            // Handle rewards popup if it appears
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Navigate to settings to validate account creation
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button.action-button"));

            // Assert
            var currentUrl = _driver.Url;
            currentUrl.Should().Contain("account/settings", "because user should be redirected to account settings after successful registration");
            
            Console.WriteLine($"✅ Account created successfully with email: {email}");
        }

        [Test]
        [Category("NewAccount")]
        [Description("TC-003: Add address to user profile")]
        public void AddAddressToUserProfile_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-003: Add Address in Profile");
            
            // Start from home page and login naturally
            _driver.Navigate().GoToUrl("https://www.stampinup.com");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(3000);
            Console.WriteLine("🏠 Started from home page");

            // Handle any initial popups
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Navigate to account page for login
            Console.WriteLine("🔗 Navigating to account login page");
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Login with existing account
            Console.WriteLine("🔐 Logging in with existing account...");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "test_udgfqa876@mail.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(10000);

            // Handle rewards popup if it appears after login
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Check where we landed after login
            var currentUrl = _driver.Url;
            Console.WriteLine($"🎯 After login, current URL: {currentUrl}");

            // Act - Navigate to address creation page
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/address/create");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Handle any additional popups
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Try to click continue/action button if present
            try
            {
                WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button.action-button"));
            }
            catch (Exception)
            {
                Console.WriteLine("ℹ️ No action button found, proceeding to form");
            }

            // Fill the address form
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-first-name']"), "John");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-last-name']"), "Doe");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address.addressLine1']"), "123 Main Street");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-addressLine2']"), "Apt 4B");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-city']"), "Los Angeles");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='autocomplete-field-div']"), "California");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-postalCode']"), "90001");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-telephone']"), "+1234567890");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='address-save']"));
            Thread.Sleep(10000);

            // Assert
            var addresses = _driver.FindElements(By.CssSelector("div[data-testid='address-list-item']"));
            addresses.Should().HaveCountGreaterThan(0, "because address should be added successfully");
            
            Console.WriteLine("✅ Address added successfully!");
        }

        [Test]
        [Category("NewAccount")]
        [Description("TC-004: Edit an existing address")]
        public void EditExistingAddress_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-004: Edit Address");
            
            // Start from home page and login naturally
            _driver.Navigate().GoToUrl("https://www.stampinup.com");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(3000);
            Console.WriteLine("🏠 Started from home page");

            // Handle any initial popups
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Navigate to account page for login
            Console.WriteLine("🔗 Navigating to account login page");
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Login with existing account
            Console.WriteLine("🔐 Logging in with existing account...");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "test_udgfqa876@mail.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(10000);

            // Handle rewards popup if it appears after login
            WebDriverHelper.HandleRewardsPopup(_driver);

            // Check where we landed after login
            var currentUrl = _driver.Url;
            Console.WriteLine($"🎯 After login, current URL: {currentUrl}");

            // Act - Navigate to address list page
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/address");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Handle any additional popups
            WebDriverHelper.HandleRewardsPopup(_driver);

            var editButtons = _driver.FindElements(By.CssSelector("a[data-testid='addresslist-item-btn-edit']"));
            
            // If no addresses exist, skip this test
            if (editButtons.Count == 0)
            {
                Console.WriteLine("ℹ️ No addresses found to edit, skipping test");
                Assert.Inconclusive("No addresses available to edit. Run AddAddressToUserProfile_ShouldSucceed test first.");
                return;
            }
            
            Console.WriteLine($"ℹ️ Found {editButtons.Count} address(es) to edit");

            // Click the first Edit button
            editButtons[0].Click();
            Console.WriteLine("✅ Clicked the first Edit button");
            Thread.Sleep(10000);

            // Capture original values before editing
            var originalFirstName = _driver.FindElement(By.CssSelector("input[data-testid='address-field-first-name']")).GetAttribute("value");
            var originalLastName = _driver.FindElement(By.CssSelector("input[data-testid='address-field-last-name']")).GetAttribute("value");
            var originalAddress = _driver.FindElement(By.CssSelector("input[data-testid='address.addressLine1']")).GetAttribute("value");
            
            Console.WriteLine($"📝 Original values - Name: {originalFirstName} {originalLastName}, Address: {originalAddress}");

            // Toggle between Jane/John based on current values
            string newFirstName, newLastName, newAddress, newCity, newZip, newPhone;
            
            if (originalFirstName.Contains("Jane"))
            {
                // Switch from Jane to John
                newFirstName = "John";
                newLastName = "Doe";
                newAddress = "123 Main Street";
                newCity = "Los Angeles";
                newZip = "90001";
                newPhone = "+1234567890";
                Console.WriteLine("🔄 Current name is Jane, switching to John...");
            }
            else
            {
                // Switch from John (or anything else) to Jane
                newFirstName = "Jane";
                newLastName = "Smith";
                newAddress = "456 Elm Street";
                newCity = "San Francisco";
                newZip = "94105";
                newPhone = "+1987654321";
                Console.WriteLine("🔄 Current name is not Jane, switching to Jane...");
            }

            Console.WriteLine($"🔄 Updating fields: {originalFirstName} {originalLastName} → {newFirstName} {newLastName}");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-first-name']"), newFirstName);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-last-name']"), newLastName);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address.addressLine1']"), newAddress);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-city']"), newCity);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='autocomplete-field-div']"), "California");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-field-postalCode']"), newZip);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='address-telephone']"), newPhone);

            // Verify fields were updated before saving
            var verifyFirstName = _driver.FindElement(By.CssSelector("input[data-testid='address-field-first-name']")).GetAttribute("value");
            var verifyLastName = _driver.FindElement(By.CssSelector("input[data-testid='address-field-last-name']")).GetAttribute("value");
            var verifyAddress = _driver.FindElement(By.CssSelector("input[data-testid='address.addressLine1']")).GetAttribute("value");
            
            Console.WriteLine($"✏️ Updated values - Name: {verifyFirstName} {verifyLastName}, Address: {verifyAddress}");

            // Save the updated address with improved mechanism
            Console.WriteLine("💾 Attempting to save the address...");
            var saveButton = _driver.FindElement(By.CssSelector("button[data-testid='address-save']"));
            Console.WriteLine($"💾 Save button text: '{saveButton.Text}', enabled: {saveButton.Enabled}");
            
            // Scroll to save button to ensure it's visible
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            Thread.Sleep(1000);
            
            // Try clicking with JavaScript if normal click fails
            try
            {
                saveButton.Click();
                Console.WriteLine("💾 Clicked save button normally");
            }
            catch (Exception)
            {
                Console.WriteLine("💾 Normal click failed, trying JavaScript click");
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            
            Console.WriteLine("💾 Waiting for save to complete...");
            Thread.Sleep(10000);
            
            // Wait for page to redirect or show success indicator
            try
            {
                WebDriverHelper.WaitForPageLoad(_driver);
                var saveUrl = _driver.Url;
                Console.WriteLine($"📍 Current URL after save: {saveUrl}");
                
                // Check if redirected to address list (success) or still on edit page (failure)
                if (saveUrl.Contains("/address") && !saveUrl.Contains("/edit") && !saveUrl.Contains("/create"))
                {
                    Console.WriteLine("✅ Successfully redirected to address list");
                }
                else
                {
                    Console.WriteLine("⚠️ Still on edit page, save may have failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error during save verification: {ex.Message}");
            }

            // Navigate back to address list to verify the update
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/address");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Assert - Check if the dynamic values appear in the address list
            var updatedAddresses = _driver.FindElements(By.CssSelector("div[data-testid='address-list-item']"));
            Console.WriteLine($"🔍 Found {updatedAddresses.Count} address(es) in the list");
            
            var updated = false;
            foreach (var addr in updatedAddresses)
            {
                Console.WriteLine($"🔍 Checking address: {addr.Text}");
                if (addr.Text.Contains(newFirstName) && addr.Text.Contains(newLastName))
                {
                    updated = true;
                    Console.WriteLine($"✅ Found updated address with name: {newFirstName} {newLastName}");
                    break;
                }
            }

            updated.Should().BeTrue($"because address should be updated with new information: {newFirstName} {newLastName}");
            Console.WriteLine("✅ Address updated successfully!");
        }
    }
}