using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using UIAutomationProject.Utilities;

namespace UIAutomationProject.PageObject
{
    public class HomePage : PageBase
    {
        private IWebDriver driver;
        private readonly By bySpeaker = By.Id("speakersImg");
        private readonly By byCreateNewAccountButton = By.LinkText("CREATE NEW ACCOUNT");
        private readonly By byUserName = By.XPath("//a[@id='menuUserLink']//span");

        public HomePage(IWebDriver webDriver)
        {
            driver = webDriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "menuUserLink")]
        private readonly IWebElement UserIcon;  
        
        public bool ValidateUserCreation() {
            WaitTillElementisVisible(driver, byUserName);
            if(driver.FindElement(byUserName).Text.Length > 0)
                return true;
            return false;
        }

        public CreateAccountPage CreateNewAccount()
        {
            WaitTillElementisClickable(driver, bySpeaker);
            ExecuteJSClickAction(driver, UserIcon);
            WaitTillElementisClickable(driver, byCreateNewAccountButton);
            driver.FindElement(byCreateNewAccountButton).Click();
            WaitForPageLoad(driver);
            return new CreateAccountPage(driver);
        }
    }
}


