using NUnit.Framework;
using OpenQA.Selenium;
using StampinUpTests.Helpers;
using System;
using System.Threading;
using System.Linq;

namespace StampinUpTests.Tests
{
    [TestFixture]
    public class DebugHomePageTest
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
        [Category("Debug")]
        [Description("Debug: Investigate home page differences between manual and automated browsing")]
        public void DebugHomePage_ShouldShowWhatBrowserSees()
        {
            Console.WriteLine("🔍 DEBUG: Investigating home page behavior in automated browser");
            
            // Navigate to home page
            _driver.Navigate().GoToUrl("https://www.stampinup.com");
            WebDriverHelper.WaitForPageLoad(_driver);
            Thread.Sleep(5000);
            
            Console.WriteLine($"📍 Current URL: {_driver.Url}");
            Console.WriteLine($"📄 Page Title: {_driver.Title}");
            
            // Handle any popups first
            WebDriverHelper.HandleRewardsPopup(_driver);
            Thread.Sleep(2000);
            
            // Take screenshot of current state
            Console.WriteLine("📸 Taking screenshot of home page...");
            
            // Look for Sign In / Login related elements
            Console.WriteLine("\n🔍 Searching for Sign In / Login elements:");
            
            var signInSelectors = new[]
            {
                "a", "button", // Get all links and buttons first
                "[data-testid*='sign']", "[data-testid*='login']", "[data-testid*='account']",
                ".sign-in", ".login", ".account-link", ".account",
                "#sign-in", "#login", "#account"
            };
            
            // First, let's see ALL links and buttons
            try
            {
                var allLinks = _driver.FindElements(By.TagName("a"));
                var allButtons = _driver.FindElements(By.TagName("button"));
                
                Console.WriteLine($"📊 Found {allLinks.Count} links and {allButtons.Count} buttons on the page");
                
                // Check first few links for sign-in related text
                Console.WriteLine("\n🔗 Sample links found:");
                foreach (var link in allLinks.Take(10))
                {
                    try
                    {
                        var text = link.Text?.Trim() ?? "";
                        var href = link.GetAttribute("href") ?? "";
                        if (text.Length > 0 || href.Contains("sign") || href.Contains("login") || href.Contains("account"))
                        {
                            Console.WriteLine($"   - Text: '{text}' | Href: '{href.Substring(0, Math.Min(href.Length, 50))}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   - Error reading link: {ex.Message}");
                    }
                }
                
                // Check buttons
                Console.WriteLine("\n🔘 Sample buttons found:");
                foreach (var button in allButtons.Take(5))
                {
                    try
                    {
                        var text = button.Text?.Trim() ?? "";
                        if (text.Length > 0)
                        {
                            Console.WriteLine($"   - Button text: '{text}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   - Error reading button: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error checking links/buttons: {ex.Message}");
            }
            
            foreach (var selector in signInSelectors)
            {
                try
                {
                    var elements = _driver.FindElements(By.CssSelector(selector));
                    if (elements.Count > 0)
                    {
                        Console.WriteLine($"✅ Found {elements.Count} element(s) with selector: {selector}");
                        foreach (var element in elements.Take(3)) // Show first 3
                        {
                            try
                            {
                                Console.WriteLine($"   - Text: '{element.Text}', Tag: {element.TagName}, Visible: {element.Displayed}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"   - Error reading element: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ No elements found with selector: {selector}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error with selector '{selector}': {ex.Message}");
                }
            }
            
            // Check for any navigation/header elements
            Console.WriteLine("\n🔍 Searching for navigation/header elements:");
            var navSelectors = new[]
            {
                "nav", "header", ".header", ".navigation", ".nav", ".menu", ".top-bar"
            };
            
            foreach (var selector in navSelectors)
            {
                try
                {
                    var elements = _driver.FindElements(By.CssSelector(selector));
                    if (elements.Count > 0)
                    {
                        Console.WriteLine($"✅ Found {elements.Count} navigation element(s) with selector: {selector}");
                        foreach (var element in elements.Take(2))
                        {
                            try
                            {
                                var text = element.Text?.Substring(0, Math.Min(element.Text.Length, 100)) ?? "";
                                Console.WriteLine($"   - Text preview: '{text}'");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"   - Error reading nav element: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error with nav selector '{selector}': {ex.Message}");
                }
            }
            
            // Check if we're redirected or blocked
            Console.WriteLine($"\n📊 Final page state:");
            Console.WriteLine($"   - URL: {_driver.Url}");
            Console.WriteLine($"   - Title: {_driver.Title}");
            
            // Check for any blocking elements or redirects
            var pageSource = _driver.PageSource;
            Console.WriteLine($"📄 Page source length: {pageSource.Length} characters");
            
            if (pageSource.Contains("robots") || pageSource.Contains("blocked") || pageSource.Contains("captcha"))
            {
                Console.WriteLine("⚠️ Possible bot detection or blocking detected in page source");
            }
            
            if (pageSource.Contains("cloudflare") || pageSource.Contains("please wait") || pageSource.Contains("checking your browser"))
            {
                Console.WriteLine("⚠️ Possible Cloudflare or similar protection detected");
            }
            
            // Check if page is mostly empty
            if (pageSource.Length < 1000)
            {
                Console.WriteLine("⚠️ Page source is very small, might be blocked or redirected");
                Console.WriteLine($"📄 Page source preview: {pageSource.Substring(0, Math.Min(pageSource.Length, 500))}");
            }
            
            if (_driver.Url != "https://www.stampinup.com/" && !_driver.Url.StartsWith("https://www.stampinup.com"))
            {
                Console.WriteLine("⚠️ Unexpected redirect detected");
            }
            
            // Check for common blocking indicators
            if (pageSource.ToLower().Contains("access denied") || 
                pageSource.ToLower().Contains("forbidden") ||
                pageSource.ToLower().Contains("503") ||
                pageSource.ToLower().Contains("bot") ||
                pageSource.ToLower().Contains("automation"))
            {
                Console.WriteLine("⚠️ Possible access blocking detected in page content");
            }
            
            Console.WriteLine("\n🏁 Debug investigation complete");
        }
    }
}