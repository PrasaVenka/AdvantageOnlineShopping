namespace UIAutomationProject.DataModel
{
    public class MandatoryElement
    {
        public string ElementName { get; set; }
        public string ElementLocator { get; set; }
        public string ElementData { get; set; }
    }
    public class MandatoryFields
    {
        public List<MandatoryElement> MandatoryElements { get; set; }
    }
}
