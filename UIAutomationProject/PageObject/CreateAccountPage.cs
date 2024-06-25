using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using UIAutomationProject.DataModel;
using UIAutomationProject.Utilities;

namespace UIAutomationProject.PageObject
{
    public class CreateAccountPage : PageBase
    {
        private IWebDriver driver;

        private readonly By byRegisterButton = By.Id("register_btn");
        private readonly By byIAgree = By.CssSelector("input[name='i_agree']");

        public CreateAccountPage(IWebDriver webDriver)
        {
            driver = webDriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//h3[text()='CREATE ACCOUNT']")]
        private readonly IWebElement PageHeading;

        public HomePage RegisterAccount(String payloadFile)
        {            
            WaitTillElementisClickable(driver, byIAgree);
            var CreateAccountDatas = ReadJsonData<CreateUser>(payloadFile);
            foreach (InputField Input in CreateAccountDatas.CreateAccount_InputFields)
            {
                if (CreateAccountDatas.DynamicUserCreation && Input.Name == "Username")
                    Input.Value+=(DateTime.Now).ToString("hhmmss");

                try
                {
                    driver.FindElement(By.Name(Input.Locator)).SendKeys(Input.Value);
                }
                catch (NoSuchElementException e)
                {
                    SelectElement countryLists = new SelectElement(driver.FindElement(By.Name(Input.Locator)));
                    countryLists.SelectByText(Input.Value);
                }
            }           
            driver.FindElement(byIAgree).Click();
            WaitTillElementisClickable(driver, byRegisterButton);
            driver.FindElement(byRegisterButton).Click();
            return new HomePage(driver);
        }

         public Tuple<String,bool> ValidateMandaotryField(String elementLocatorName,String elementValue) 
        {
            bool fieldValidationSuccess = false;
            By byInputField = By.XPath("//input[@name='" + elementLocatorName + "']");
            By byErrorField = By.XPath("//input[@name='" + elementLocatorName + "']/following-sibling::label");
            IWebElement inputFieldElement, errorLabelElement;
            inputFieldElement = driver.FindElement(byInputField);
            WaitTillElementisClickable(driver, byInputField);
            inputFieldElement.Click();
            PageHeading.Click();
            errorLabelElement = driver.FindElement(byErrorField);
            String errorMessage = errorLabelElement.Text;
            inputFieldElement.SendKeys(elementValue);
            //validate the error message cleared
            if (!(errorLabelElement.Text).Contains("is required"))
                fieldValidationSuccess = true;

            return new Tuple<String, bool>(errorMessage, fieldValidationSuccess);
        }
    }
}


