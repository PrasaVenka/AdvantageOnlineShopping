using OpenQA.Selenium.Firefox;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using WebDriverManager.Helpers;
using Newtonsoft.Json;
using UIAutomationProject.DTO;
using SeleniumExtras.WaitHelpers;

namespace UIAutomationProject.Utilities
{
    public class PageBase
    {
        public IWebDriver driver;
        public ExtentReports extentReports;
        public ExtentTest extentTest;
        int pageTimeout = 10;
        CreateUser registerFormInputs;
        
        [OneTimeSetUp]
        public void SetupReport()
        {
            String reportPath = GetFilePath("Report", "index.html");
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            extentReports = new ExtentReports();
            extentReports.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void StartBrowser()
        {
            extentTest = extentReports.CreateTest(TestContext.CurrentContext.Test.MethodName);

            String browserName;
            browserName = TestContext.Parameters["browser"];

            if (browserName == null)
            {
                browserName = ConfigurationManager.AppSettings["browser"];
            }
            InitBrowser(browserName);
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.advantageonlineshopping.com/";
        }
        public void ImportCreateUserJsonFile(String filename)
        {
            String filePath = GetFilePath("TestData", filename);
            registerFormInputs = JsonConvert.DeserializeObject<CreateUser>(File.ReadAllText(filePath));
        }
        public void WaitTillElementisVisible(IWebDriver webdriver, By locator)
        {
            WebDriverWait webDriverWait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(pageTimeout));
            webDriverWait.Until(ExpectedConditions.ElementIsVisible(locator));

        }
        public void WaitTillElementisClickable(IWebDriver webdriver, By locator)
        {
            WebDriverWait webDriverWait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(pageTimeout));
            webDriverWait.Until(ExpectedConditions.ElementToBeClickable(locator));

        }

        public void ExecuteJSClickAction(IWebDriver driver, IWebElement element)
        {

            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].click();", element);

        }

        public void InitBrowser(String browserName)
        {

            switch (browserName)
            {
                case "Firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig(), "Latest");
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    driver = new FirefoxDriver(firefoxOptions);
                    break;
                case "Chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    ChromeOptions chromeOptions = new ChromeOptions();
                    driver = new ChromeDriver(chromeOptions);
                    break;
                case "Edge":
                    new DriverManager().SetUpDriver(new EdgeConfig(), "Latest");
                    EdgeOptions edgeOptions = new EdgeOptions();
                    driver = new EdgeDriver(edgeOptions);
                    break;

            }
        }

        public string GetFilePath(string subFolder, String fileName)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "\\" + subFolder + "\\" + fileName;
            return reportPath;
        }


        public String FetchFormInputValue(String Key)
        {
            String FormatedKey = Key.Replace(" ", "").Replace("/", "");
            List<InputField> inputFields = registerFormInputs.InputFields;
            foreach (InputField field in inputFields)
            {
                if (field.Name == FormatedKey)
                {
                    if (FormatedKey == "Username" && registerFormInputs.DynamicUserCreation)
                        field.Value = field.Value + (DateTime.Now).ToString("hhmmss"); // dynamic username
                    return field.Value;
                }
            }
            return "Default";
        }
        public MediaEntityModelProvider CaptureScreenshot(IWebDriver driver, String imagename)
        {

            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            var screenshot = takesScreenshot.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.
                CreateScreenCaptureFromBase64String(screenshot, imagename).Build();
        }

        [TearDown]
        public void Teardown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            // Extend report implementation
            string failFileName = "FailedScreenshot_" + DateTime.Now.ToString() + ".png";
            string passFileName = "PassedScreenshot_" + DateTime.Now.ToString() + ".png";

            if (status == TestStatus.Failed)
            {
                extentTest.Fail("Test case failed", CaptureScreenshot(driver, failFileName));
            }
            else if (status == TestStatus.Passed)
            {
                extentTest.Pass("Test case passed", CaptureScreenshot(driver, passFileName));
            }
            driver.Close();
            driver.Quit();
        }

        [OneTimeTearDown]
        public void flushReport()
        {
            if (extentReports != null) { extentReports.Flush(); }
        }
    }
}
