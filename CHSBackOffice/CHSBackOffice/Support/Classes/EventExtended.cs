using CHBackOffice.ApiServices.ChsProxy;

namespace CHSBackOffice.Support.Classes
{
    public class EventExtended : ListViewItemBase<ArrayOfEventEvent>
    {
        public string DeviceName => BaseObject.DeviceName;
        public string Summary => BaseObject.Summary;
        public string EventDate => BaseObject.EventDate;
    }
}
