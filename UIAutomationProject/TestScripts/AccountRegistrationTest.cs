using AventStack.ExtentReports;
using NUnit.Framework;
using UIAutomationProject.Utilities;
using UIAutomationProject.PageObject;

namespace UIAutomationProject.TestScripts
{
    public class AccountRegistrationTest : PageBase
    {
        [Test]
        [TestCase("NewUserRegistraion.json")]
        public void AccountRegistrationTestcase(String payloadFile)
        {
            String ScreenshotFileName = "Screenshot_" + DateTime.Now.ToString() + ".png";
            extentTest.Log(Status.Info, "Creating a new user from json file " + payloadFile, CaptureScreenshot(driver, ScreenshotFileName));
            HomePage homepage = new HomePage(driver);
            extentTest.Log(Status.Info, "Landing on home page is success" , CaptureScreenshot(driver, ScreenshotFileName));
            CreateAccountPage createNewAccount = homepage.CreateNewAccount();
            extentTest.Log(Status.Info, "Landing on Login/create acconut panel is success", CaptureScreenshot(driver, ScreenshotFileName));
            HomePage loggedIn = createNewAccount.RegisterAccount(payloadFile);
            extentTest.Log(Status.Info, "Landing on create new account page is success", CaptureScreenshot(driver, ScreenshotFileName));
            Assert.True(loggedIn.ValidateUserCreation());
            extentTest.Log(Status.Pass, "User created successfully", CaptureScreenshot(driver, ScreenshotFileName));            
        }

    }
}
