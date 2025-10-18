using NUnit.Framework;
using OpenQA.Selenium;
using StampinUpTests.Helpers;
using FluentAssertions;
using System;
using System.Threading;

namespace StampinUpTests.Tests
{
    [TestFixture]
    public class InvalidCredentialsTests
    {
        private IWebDriver _driver;
        private const string InvalidEmail = "invalid_creds@gmail.com";
        private const string InvalidPassword = "Password123!";

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
        [Category("NegativeTest")]
        [Description("TC-005: Login with wrong credentials should fail")]
        public void LoginWithInvalidCredentials_ShouldFail()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-005: Login with Wrong Creds, Should Fail");

            // Act
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Fill login form with invalid credentials
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), InvalidEmail);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), InvalidPassword);

            // Submit login form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(20000);

            // Try to navigate to account settings to verify login failure
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Assert
            var currentUrl = _driver.Url;
            currentUrl.Should().NotContain("account/settings", 
                "because user should not be able to access account settings with invalid credentials");
            
            Console.WriteLine($"✅ Login unsuccessful for: {InvalidEmail}, as Expected");
        }

        [Test]
        [Category("NegativeTest")]
        [Description("TC-006: Login with empty email should fail")]
        public void LoginWithEmptyEmail_ShouldFail()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-006: Login with Empty Email, Should Fail");

            // Act
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Fill login form with empty email
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");

            // Submit login form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            // Assert - Check if validation error appears or submit button is disabled
            var emailField = _driver.FindElement(By.CssSelector("input[data-testid='auth-email']"));
            var isFormValid = (bool)((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].checkValidity();", emailField);
            
            isFormValid.Should().BeFalse("because form should not be valid with empty email");
            Console.WriteLine("✅ Login form validation working correctly for empty email");
        }

        [Test]
        [Category("NegativeTest")]
        [Description("TC-007: Login with empty password should fail")]
        public void LoginWithEmptyPassword_ShouldFail()
        {
            // Arrange
            Console.WriteLine("🔹 Running TC-007: Login with Empty Password, Should Fail");

            // Act
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            // Fill login form with empty password
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "valid@email.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "");

            // Submit login form
            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            // Assert - Check if validation error appears or submit button is disabled
            var passwordField = _driver.FindElement(By.CssSelector("input[data-testid='auth-password']"));
            var isFormValid = (bool)((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].checkValidity();", passwordField);
            
            isFormValid.Should().BeFalse("because form should not be valid with empty password");
            Console.WriteLine("✅ Login form validation working correctly for empty password");
        }

    }
}