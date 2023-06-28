using CHSBackOffice.Support.Classes;
using Prism.Events;

namespace CHSBackOffice.Events
{
    class ChartDataIsReady<T> : PubSubEvent<T>
    {
    }
}
