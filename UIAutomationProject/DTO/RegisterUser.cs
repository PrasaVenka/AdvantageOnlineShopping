namespace UIAutomationProject.DTO
{
    public class InputField
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class CreateUser
    {
        public List<InputField> InputFields { get; set; }
        public bool AcceptMarketingComs { get; set; }
        public bool AcceptTCs { get; set; }
        public bool DynamicUserCreation { get; set; }
    }
}