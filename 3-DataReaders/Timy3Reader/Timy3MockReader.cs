using System.Collections.Generic;
using SchletterTiming.Model;
using SchletterTiming.ReaderInterfaces;

namespace SchletterTiming.Timy3Reader {
    public class Timy3MockReader : ITimy3Reader {
        public void Init() {
            return;
        }

        public List<TimingValue> WaitForBulk() {
            return new List<TimingValue>();
        }
    }
}
