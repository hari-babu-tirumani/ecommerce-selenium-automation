using NUnit.Framework;
using OpenQA.Selenium;
using StampinUpTests.Helpers;
using FluentAssertions;
using System;
using System.Threading;

namespace StampinUpTests.Tests
{
    [TestFixture]
    public class ExistingAccountTests
    {
        private IWebDriver _driver;
        private const string TestEmail = "test_udgfqa876@mail.com";
        private const string TestPassword = "Password123!";

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
        [Category("ExistingAccount")]
        [Description("TC-002: Login with existing account")]
        public void LoginWithExistingAccount_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-002: Login with existing account");

            // Act
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Fill login form
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), TestEmail);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), TestPassword);

            // Submit login form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(20000);

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button.action-button"));

            // Navigate to settings page to validate login
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            Thread.Sleep(5000);

            // Assert
            var currentUrl = _driver.Url;
            currentUrl.Should().Contain("account/settings", "because user should be redirected to account settings after successful login");
            
            Console.WriteLine($"✅ Login successful for: {TestEmail}");
        }

        [Test]
        [Category("ExistingAccount")]
        [Description("TC-007: Edit first name field of existing account")]
        public void EditFirstNameField_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-007: Edit firstName");
            LoginToAccount();

            // Act
            EditAccountField("firstName");

            // Assert is handled within EditAccountField method
            Console.WriteLine("✅ firstName updated successfully!");
        }

        [Test]
        [Category("ExistingAccount")]
        [Description("TC-008: Edit last name field of existing account")]
        public void EditLastNameField_ShouldSucceed()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-008: Edit lastName");
            LoginToAccount();

            // Act
            EditAccountField("lastName");

            // Assert is handled within EditAccountField method
            Console.WriteLine("✅ lastName updated successfully!");
        }

        private void LoginToAccount()
        {
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Fill login form
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), TestEmail);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), TestPassword);

            // Submit login form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(20000);

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button.action-button"));

            // Navigate to settings page
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
        }

        private void EditAccountField(string fieldName)
        {
            // Navigate to account settings
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);

            // Click edit contact setting button
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='edit-contact-setting']"));
            Console.WriteLine("✅ Clicked the Edit button");
            Thread.Sleep(10000);

            // Get current value and apply toggle logic
            var inputElement = _driver.FindElement(By.CssSelector($"input[data-testid='account-card-{fieldName}']"));
            var currentValue = inputElement.GetAttribute("value");
            
            Console.WriteLine($"📝 Current {fieldName}: '{currentValue}'");

            // Toggle logic based on current value and field type
            string newValue;
            if (fieldName == "firstName")
            {
                if (currentValue.Contains("Jane"))
                {
                    newValue = "John";
                    Console.WriteLine("🔄 Switching firstName from Jane to John");
                }
                else
                {
                    newValue = "Jane";
                    Console.WriteLine("🔄 Switching firstName to Jane");
                }
            }
            else if (fieldName == "lastName")
            {
                if (currentValue.Contains("Smith"))
                {
                    newValue = "Doe";
                    Console.WriteLine("🔄 Switching lastName from Smith to Doe");
                }
                else
                {
                    newValue = "Smith";
                    Console.WriteLine("🔄 Switching lastName to Smith");
                }
            }
            else
            {
                // Fallback for other fields
                newValue = currentValue.Contains("Test") ? "Updated" : "Test";
                Console.WriteLine($"🔄 Toggling {fieldName} to: {newValue}");
            }

            Console.WriteLine($"🔄 Updating {fieldName}: '{currentValue}' → '{newValue}'");

            // Clear and update with toggle value
            WebDriverHelper.WaitAndType(_driver, By.CssSelector($"input[data-testid='account-card-{fieldName}']"), newValue);
            WebDriverHelper.WaitAndClick(_driver, By.XPath("//button[@data-testid='save-changes']"));
            Thread.Sleep(20000);

            // Validation: check if the updated field appears in the account information
            var fields = _driver.FindElements(By.CssSelector("div[data-testid='account-label-content'] span.mt-2"));
            var updated = false;
            foreach (var fieldValue in fields)
            {
                if (fieldValue.Text.Contains(newValue))
                {
                    updated = true;
                    Console.WriteLine($"✅ Found updated {fieldName} with value: {newValue}");
                    break;
                }
            }

            updated.Should().BeTrue($"because {fieldName} should be updated to '{newValue}' successfully");
        }
    }
}