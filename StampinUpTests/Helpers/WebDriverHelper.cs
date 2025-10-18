using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace StampinUpTests.Helpers
{
    public static class WebDriverHelper
    {
        public static IWebDriver SetupDriver()
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-blink-features=AutomationControlled");
                options.AddExcludedArgument("enable-automation");
                options.AddAdditionalOption("useAutomationExtension", false);
                
                var driver = new ChromeDriver(options);
                ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");
                
                Console.WriteLine("✓ Successfully created Chrome driver");
                return driver;
            }
            catch (Exception ex)
            {
                throw new Exception("ChromeDriver setup failed.", ex);
            }
        }

        public static IWebElement? WaitAndClick(IWebDriver driver, By locator, int timeoutSeconds = 15)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                
                // Try normal click first
                try
                {
                    element.Click();
                    return element;
                }
                catch (ElementClickInterceptedException)
                {
                    // If normal click fails, try JavaScript click
                    Console.WriteLine($"⚠️ Normal click intercepted, trying JavaScript click for: {locator}");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                    return element;
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"❌ Element not clickable: {locator}");
                return null;
            }
        }

        public static IWebElement? WaitAndType(IWebDriver driver, By locator, string text, int timeoutSeconds = 15)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                
                // Multiple clearing approaches to ensure field is empty
                try
                {
                    // First try standard clear
                    element.Clear();
                }
                catch
                {
                    // If clear fails, continue with JavaScript
                }
                
                // Clear using JavaScript and trigger events
                ((IJavaScriptExecutor)driver).ExecuteScript(@"
                    arguments[0].value = '';
                    arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                    arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
                ", element);
                
                // Add small delay to ensure clearing is processed
                Thread.Sleep(500);
                
                element.SendKeys(text);
                return element;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"❌ Element not found or not clickable: {locator}");
                return null;
            }
        }

        public static void WaitForPageLoad(IWebDriver driver, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            
            // Add random delay to mimic human behavior
            var random = new Random();
            Thread.Sleep(random.Next(2000, 4000));
        }

        public static string GenerateRandomEmail()
        {
            var random = new Random();
            var randomString = GenerateRandomString(6);
            var randomNumber = random.Next(100, 999);
            return $"test_{randomString}{randomNumber}@mail.com";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            
            return new string(result);
        }

        public static bool WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.ElementExists(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static void HandleRewardsPopup(IWebDriver driver)
        {
            try
            {
                // Wait for popup to appear and click "MAYBE LATER"
                var maybeLaterButton = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'MAYBE LATER')]")));
                maybeLaterButton.Click();
                Console.WriteLine("✅ Dismissed rewards popup by clicking 'MAYBE LATER'");
                Thread.Sleep(2000);
            }
            catch (WebDriverTimeoutException)
            {
                // Popup didn't appear, continue normally
                Console.WriteLine("ℹ️ No rewards popup detected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not handle rewards popup: {ex.Message}");
            }
        }
    }
}