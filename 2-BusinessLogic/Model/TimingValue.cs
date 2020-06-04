using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [Serializable]
    public class TimingValue {
        public int MeasurementNumber { get; set; }
        public string Time { get; set; }
        public int StartNumber { get; set; }
    }
}
