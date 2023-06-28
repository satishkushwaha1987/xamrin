using CHBackOffice.ApiServices.ChsProxy;

namespace CHSBackOffice.Support.Classes
{
    public class ParameterInfoClass : ListViewItemBase<Parameter>
    {
        public string Group => BaseObject.GroupId;
        public string Name => BaseObject.Id;
        public string Value => BaseObject.Value;
        public string Description => BaseObject.Description;
    }
}
