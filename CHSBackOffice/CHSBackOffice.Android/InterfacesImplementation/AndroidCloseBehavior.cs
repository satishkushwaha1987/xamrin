
using System;
using CHSBackOffice.Support.Device;

namespace CHSBackOffice.Droid.InterfacesImplementation
{
    public class AndroidCloseBehavior: ICloseBehavior
    {
        public event EventHandler Closing;

        public void Close()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}