using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using UIAutomationProject.Utilities;

namespace UIAutomationProject.PageObject
{
    public class CreateAccountPage : PageBase
    {
        private IWebDriver driver;
        private By byRegisterButton = By.Id("register_btn");
        private By byIAgree = By.CssSelector("input[name='i_agree']");

        public CreateAccountPage(IWebDriver webDriver)
        {
            driver = webDriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//div[@id='formCover']//div//div//sec-view//div[@class='inputContainer ng-scope']")]
        private IList<IWebElement> FormInputContainers;

        [FindsBy(How = How.XPath, Using = "//div[@id='formCover']//div//div//sec-view//div[@class='inputContainer ng-scope']//label[@class='invalid']")]
        private IList<IWebElement> ValidationErrorMessages;

        [FindsBy(How = How.XPath, Using = "//h3[text()='CREATE ACCOUNT']")]
        private IWebElement PageHeading;

        [FindsBy(How = How.CssSelector, Using = "input[name='i_agree']")]
        private IWebElement IAgreeCheckBox;

        [FindsBy(How = How.Id, Using = "register_btn")]
        private IWebElement RegisterButton;

        public bool isMandatoryField(IWebElement element)
        {
            try
            {
                if (element.FindElement(By.TagName("span")).Text == "*")
                    return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
            return false;
        }

        public void FillCreateAccountForm()
        {
            String ErrorMessage = String.Empty, InputTitle = String.Empty, ValidationMessage = String.Empty;

            foreach (IWebElement FormInputContainer in FormInputContainers)
            {
                bool isRequiredField = isMandatoryField(FormInputContainer);
                InputTitle = FormInputContainer.FindElement(By.TagName("label")).Text;

                if (isRequiredField)
                {
                    FormInputContainer.FindElement(By.TagName("input")).Click();
                    PageHeading.Click();
                    ErrorMessage = FormInputContainer.FindElement(By.TagName("label")).Text;
                    ValidationMessage = $"{InputTitle} field is required";
                    Assert.AreEqual(ValidationMessage, ErrorMessage);
                }

                try
                {
                    FormInputContainer.FindElement(By.TagName("input")).SendKeys(FetchFormInputValue(InputTitle));
                }
                catch (NoSuchElementException e)
                {
                    SelectElement countryLists = new SelectElement(FormInputContainer.FindElement(By.TagName("select")));
                    countryLists.SelectByText(FetchFormInputValue(InputTitle));
                }
            }
        }

        public HomePage RegisterAccount(String payloadFile)
        {
            WaitTillElementisClickable(driver, byIAgree);
            ImportCreateUserJsonFile(payloadFile);
            FillCreateAccountForm();
            IAgreeCheckBox.Click();
            if (ValidationErrorMessages.Count == 0)
            {
                WaitTillElementisClickable(driver, byRegisterButton);
                RegisterButton.Click();
            }
            return new HomePage(driver);
        }
    }
}


