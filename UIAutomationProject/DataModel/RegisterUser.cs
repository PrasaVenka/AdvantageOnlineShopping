namespace UIAutomationProject.DataModel
{
    public class InputField
    {
        public string Locator { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class CreateUser
    {
        public List<InputField> CreateAccount_InputFields { get; set; }
        public bool AcceptMarketingComs { get; set; }
        public bool AcceptTCs { get; set; }
        public bool DynamicUserCreation { get; set; }
    }
}