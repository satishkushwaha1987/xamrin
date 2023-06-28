
using CHSBackOffice.Support.Device;
using System.Threading;

namespace CHSBackOffice.iOS.InterfacesImplementation
{
    public class IosCloseBehavior : ICloseBehavior
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}