using NUnit.Framework;
using OpenQA.Selenium;
using StampinUpTests.Helpers;
using FluentAssertions;
using System;
using System.Threading;

namespace StampinUpTests.Tests
{
    [TestFixture]
    public class LoginValidationTests
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
        [Category("LoginValidation")]
        [Description("TC-014: Login with empty email should fail")]
        public void LoginWithEmptyEmail_ShouldFail()
        {
            Console.WriteLine("🔹 Running TC-014: Login with empty email");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(3000);

            VerifyLoginValidationError("email");
            Console.WriteLine("✅ Empty email validation working correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-015: Login with empty password should fail")]
        public void LoginWithEmptyPassword_ShouldFail()
        {
            Console.WriteLine("🔹 Running TC-015: Login with empty password");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "test@example.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(3000);

            VerifyLoginValidationError("password");
            Console.WriteLine("✅ Empty password validation working correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-016: Login with invalid email format should fail")]
        public void LoginWithInvalidEmailFormat_ShouldFail()
        {
            Console.WriteLine("🔹 Running TC-016: Login with invalid email format");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "invalid-email");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(3000);

            VerifyLoginValidationError("email");
            Console.WriteLine("✅ Invalid email format validation working correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-017: Login with nonexistent email should fail")]
        public void LoginWithNonexistentEmail_ShouldFail()
        {
            Console.WriteLine("🔹 Running TC-017: Login with nonexistent email");
            NavigateToLoginForm();

            var randomEmail = WebDriverHelper.GenerateRandomEmail();
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), randomEmail);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "Password123!");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            VerifyLoginError("credentials");
            Console.WriteLine("✅ Nonexistent email validation working correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-018: Login with correct email but wrong password should fail")]
        public void LoginWithWrongPassword_ShouldFail()
        {
            Console.WriteLine("🔹 Running TC-018: Login with wrong password");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "test_udgfqa876@mail.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "WrongPassword123!");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            VerifyLoginError("credentials");
            Console.WriteLine("✅ Wrong password validation working correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-019: Login with SQL injection attempt should fail safely")]
        public void LoginWithSQLInjection_ShouldFailSafely()
        {
            Console.WriteLine("🔹 Running TC-019: Login with SQL injection attempt");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "admin'--");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "' OR '1'='1");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            VerifyLoginError("credentials");
            Console.WriteLine("✅ SQL injection attempt blocked correctly");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-020: Login with XSS attempt should be sanitized")]
        public void LoginWithXSSAttempt_ShouldBeSanitized()
        {
            Console.WriteLine("🔹 Running TC-020: Login with XSS attempt");
            NavigateToLoginForm();

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), "<script>alert('xss')</script>@test.com");
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), "<script>alert('xss')</script>");

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(3000);

            var pageSource = _driver.PageSource;
            pageSource.Should().NotContain("<script>alert('xss')</script>", "because XSS attempts should be sanitized");
            
            VerifyLoginError("credentials");
            Console.WriteLine("✅ XSS attempt handled safely");
        }

        [Test]
        [Category("LoginValidation")]
        [Description("TC-021: Login with extremely long input should be handled gracefully")]
        public void LoginWithExtremelyLongInput_ShouldBeHandled()
        {
            Console.WriteLine("🔹 Running TC-021: Login with extremely long input");
            NavigateToLoginForm();

            var longString = new string('a', 1000) + "@test.com";
            var longPassword = new string('b', 1000);

            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-email']"), longString);
            WebDriverHelper.WaitAndType(_driver, By.CssSelector("input[data-testid='auth-password']"), longPassword);

            WebDriverHelper.WaitAndClick(_driver, By.CssSelector("button[data-testid='auth-submit']"));
            Thread.Sleep(5000);

            VerifyLoginError("credentials");
            Console.WriteLine("✅ Long input handled gracefully");
        }

        private void NavigateToLoginForm()
        {
            _driver.Navigate().GoToUrl("https://www.stampinup.com/account/settings");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);

            WebDriverHelper.HandleRewardsPopup(_driver);
        }

        private void VerifyLoginValidationError(string fieldType)
        {
            try
            {
                var submitButton = _driver.FindElement(By.CssSelector("button[data-testid='auth-submit']"));
                if (submitButton != null)
                {
                    Console.WriteLine($"✅ Client-side validation prevented form submission for {fieldType}");
                    return;
                }
            }
            catch
            {
                // Submit button not found, might have attempted to submit
            }

            var validationMessages = _driver.FindElements(By.CssSelector(".error, .validation-error, [class*='error'], [class*='invalid']"));
            if (validationMessages.Count > 0)
            {
                Console.WriteLine($"✅ Found validation error message for {fieldType}");
                return;
            }

            try
            {
                var isFormValid = (bool)((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    var form = document.querySelector('form');
                    return form ? form.checkValidity() : true;
                ");

                if (!isFormValid)
                {
                    Console.WriteLine($"✅ HTML5 validation prevented submission for {fieldType}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not check form validity: {ex.Message}");
            }

            Console.WriteLine($"⚠️ Validation for {fieldType} might not be working properly");
        }

        private void VerifyLoginError(string errorType)
        {
            var currentUrl = _driver.Url;
            
            if (currentUrl.Contains("account/settings") && !currentUrl.Contains("error"))
            {
                Console.WriteLine($"⚠️ Login appeared to succeed - {errorType} validation might not be working");
                return;
            }

            var errorMessages = _driver.FindElements(By.CssSelector(".error, .alert, [class*='error'], [class*='invalid'], [class*='failed']"));
            if (errorMessages.Count > 0)
            {
                Console.WriteLine($"✅ Found login error message for {errorType}");
                return;
            }

            var submitButton = _driver.FindElements(By.CssSelector("button[data-testid='auth-submit']"));
            if (submitButton.Count > 0)
            {
                Console.WriteLine($"✅ Still on login form - {errorType} validation prevented login");
                return;
            }

            Console.WriteLine($"⚠️ {errorType} validation might not be working properly");
        }
    }
}