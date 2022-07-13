using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Notification
{
    // This class using Selenium and NUnit Framework
    public class WhatsApp
    {
        public static IWebDriver webDriver;
        private static string chromeDriver = HttpContext.Current.Server.MapPath("~/Driver");

        public WhatsApp()
        {
            //Close process chromedriver.exe first
            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }

            IWebDriver driver;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--profile-directory=WhatsAppWeb");
            options.AddArgument("--user-data-dir=C:/Temp/ChromeProfile");

            // Setup Chrome In Windows Server (different User Account)
            // Set security folder (--user-data-dir) allow on everyone can access
            // Application path "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" --profile-directory="WhatsAppWeb" --user-data-dir="C:/Temp/ChromeProfile"

            driver = new ChromeDriver(chromeDriver, options);
            driver.Url = "https://web.whatsapp.com/";
            webDriver = driver;
        }

        public static int SendTo(string phone, string message)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(message))
            {
                return -1;
            }

            phone = phone.Replace("+", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

            IWebDriver driver;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--profile-directory=WhatsAppWeb");
            options.AddArgument("--user-data-dir=cc");
            driver = new ChromeDriver(chromeDriver, options);
            driver.Url = "https://api.whatsapp.com/send?phone=" + phone;
            webDriver = driver;

            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(300);
                WebDriverWait waiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(300));

                waiter.IgnoreExceptionTypes(typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(ElementNotInteractableException)
                    );

                IWebElement continueChat = waiter.Until(
                    x => x.FindElement(By.CssSelector("a[title=\"Share on WhatsApp\"]"))
                    );

                continueChat.Click();


                IWebElement useWhatsAppWeb = waiter.Until(
                    x => x.FindElement(By.XPath("//*[text()='use WhatsApp Web']"))
                    );

                useWhatsAppWeb.Click();

                IWebElement messageText = waiter.Until(
                    x => x.FindElement(By.CssSelector("footer div:first-child div:nth-child(2) div div:nth-child(2)"))
                    );
                messageText.Clear();
                messageText.Click();

                string replacement = "<br/>";
                message = message.Replace("<br />", replacement).Replace("<br>", replacement).Replace("\n", replacement);
                var formatted_message = message.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                for (int x = 0; x < formatted_message.Length; x++)
                {
                    messageText.SendKeys(formatted_message[x] + (Keys.Shift + Keys.Enter));
                }

                messageText.SendKeys(Keys.Enter);

                webDriver.Close();
                webDriver.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                webDriver.Close();
                webDriver.Dispose();
                ex.Message.ToString();
                return -1;
            }
        }

        public static void SendTo(List<string> phones, string message)
        {
            foreach (string _phone in phones)
            {
                string phone = _phone;
                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(message))
                {
                    return;
                }

                phone = phone.Replace("+", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

                IWebDriver driver;
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--profile-directory=WhatsAppWeb");
                options.AddArgument("--user-data-dir=C:/Temp/ChromeProfile");
                driver = new ChromeDriver(chromeDriver, options);
                webDriver = driver;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(300);
                WebDriverWait waiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(300));
                driver.Url = "https://api.whatsapp.com/send?phone=" + phone;

                try
                {
                    waiter.IgnoreExceptionTypes(typeof(NoSuchElementException),
                        typeof(ElementNotVisibleException),
                        typeof(ElementNotInteractableException)
                        );

                    IWebElement continueChat = waiter.Until(
                        x => x.FindElement(By.CssSelector("a[title=\"Share on WhatsApp\"]"))
                        );

                    continueChat.Click();


                    IWebElement useWhatsAppWeb = waiter.Until(
                        x => x.FindElement(By.XPath("//*[text()='use WhatsApp Web']"))
                        );

                    useWhatsAppWeb.Click();

                    IWebElement messageText = waiter.Until(
                        x => x.FindElement(By.CssSelector("footer div:first-child div:nth-child(2) div div:nth-child(2)"))
                        );
                    messageText.Clear();
                    messageText.Click();

                    string replacement = "<br/>";
                    message = message.Replace("<br />", replacement).Replace("<br>", replacement).Replace("\n", replacement);
                    var formatted_message = message.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                    for (int x = 0; x < formatted_message.Length; x++)
                    {
                        messageText.SendKeys(formatted_message[x] + (Keys.Shift + Keys.Enter));
                    }

                    messageText.SendKeys(Keys.Enter);
                }
                catch (Exception ex)
                {
                    webDriver.Close();
                    webDriver.Dispose();
                    ex.Message.ToString();
                }

                webDriver.Close();
                webDriver.Dispose();

            }


        }

        //new wa otp
        public static async Task<bool> SendMsg(string Phone, string Message)
        {
            Phone = Phone.Replace("-", string.Empty);
            Phone = Phone.Replace("+", string.Empty);
            Phone = Phone.Replace(" ", string.Empty);

            using (WebClient webClient = new WebClient())
            {
                string host = ConfigurationManager.AppSettings.Get("wa_api").ToString();
                string idApi = ConfigurationManager.AppSettings.Get("wa_id").ToString();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("id", idApi);
                parameters.Add("number", Phone);
                parameters.Add("message", Message);

                byte[] responsebytes = webClient.UploadValues(host, "POST", parameters);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody.Contains("SUCCESS"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //[TearDown]
        public static void Dispose()
        {
            webDriver.Close();
            webDriver.Dispose();
        }

        public static IWebElement FindElement(IWebDriver chromeDriver, IWebElement element, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => element);
            }
            return element;
        }
    }
}