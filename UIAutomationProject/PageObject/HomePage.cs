using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using UIAutomationProject.Utilities;

namespace UIAutomationProject.PageObject
{
    public class HomePage : PageBase
    {
        private IWebDriver driver;
        private By bySpeaker = By.Id("speakersImg");
        private By byCreateNewAccountButton = By.LinkText("CREATE NEW ACCOUNT");
        private By byUserName = By.XPath("//a[@id='menuUserLink']//span");

        public HomePage(IWebDriver webDriver)
        {
            driver = webDriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "menuUserLink")]
        private IWebElement UserIcon;  

        [FindsBy(How = How.LinkText, Using = "CREATE NEW ACCOUNT")]
        private IWebElement CreateNewAccountButton;

        [FindsBy(How = How.XPath, Using = "//a[@id='menuUserLink']//span")]
        private IWebElement UserName;

        public void ValidateUserCreation() {
            WaitTillElementisVisible(driver, byUserName);
            Assert.True(UserName.Text.Length > 0);     
        }

        public CreateAccountPage CreateNewAccount()
        {
            WaitTillElementisClickable(driver, bySpeaker);
            ExecuteJSClickAction(driver, UserIcon);
            WaitTillElementisClickable(driver, byCreateNewAccountButton);
            CreateNewAccountButton.Click();
            return new CreateAccountPage(driver);
        }
    }
}


