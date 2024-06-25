using AventStack.ExtentReports;
using NUnit.Framework;
using UIAutomationProject.Utilities;
using UIAutomationProject.PageObject;
using UIAutomationProject.DataModel;

namespace UIAutomationProject.TestScripts
{
    public class MandatoryFieldValidationTest : PageBase
    {
        [Test]
        [TestCase("ValidateMandatoryFields.json")]
        public void MandatoryFieldValidationTestCase(String payloadFile)
        {
            String ScreenshotFileName = "Screenshot_" + DateTime.Now.ToString() + ".png";
            extentTest.Log(Status.Info, "Validating the mandatory field in Create account page " + payloadFile, CaptureScreenshot(driver, ScreenshotFileName));
            
            HomePage homepage = new HomePage(driver);
            extentTest.Log(Status.Info, "Landing on home page is success" , CaptureScreenshot(driver, ScreenshotFileName));
            
            CreateAccountPage createNewAccount = homepage.CreateNewAccount();
            extentTest.Log(Status.Info, "Landing on Login/create acconut panel is success", CaptureScreenshot(driver, ScreenshotFileName));
            
            var MandatoryFieldList = ReadJsonData<MandatoryFields>(payloadFile);
            foreach (var MandatoryField in MandatoryFieldList.MandatoryElements)
            {
                Tuple<String,bool> FieldValidationResult = createNewAccount.ValidateMandaotryField(MandatoryField.ElementLocator, MandatoryField.ElementData);
                Assert.AreEqual($"{MandatoryField.ElementName} field is required", FieldValidationResult.Item1);
                Assert.True(FieldValidationResult.Item2);
                extentTest.Log(Status.Pass, $"{MandatoryField.ElementName} field validation is verified successfully", CaptureScreenshot(driver, ScreenshotFileName));
            }
            
        }

    }
}
