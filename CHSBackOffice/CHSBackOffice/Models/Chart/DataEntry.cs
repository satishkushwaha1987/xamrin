

using SkiaSharp;

namespace CHSBackOffice.Models.Chart
{
    public class DataEntry
    {
        #region Constructors
        public DataEntry()
        {
        }

        public DataEntry(float value, float capacity, SKColor color)
        {
            Value = value;
            Capacity = capacity;
            Color = color;
        }
        #endregion

        #region Properties

        public float Value { get; set; }

        public float Capacity { get; set; }

        public SKColor Color { get; set; }

        #endregion

    }
}
