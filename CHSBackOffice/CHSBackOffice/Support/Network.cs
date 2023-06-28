using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace CHSBackOffice.Support
{
    static class Network
    {
        public static IConnectivity Current = CrossConnectivity.Current;
    }
}
