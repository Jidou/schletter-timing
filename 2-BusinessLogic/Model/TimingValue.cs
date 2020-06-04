using System;

namespace SchletterTiming.Model {
    [Serializable]
    public class TimingValue {
        public int MeasurementNumber { get; set; }
        public string Time { get; set; }
        public int StartNumber { get; set; }
    }
}
