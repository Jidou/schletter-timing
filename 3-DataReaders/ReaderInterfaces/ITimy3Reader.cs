using System.Collections.Generic;
using SchletterTiming.Model;

namespace SchletterTiming.ReaderInterfaces {
    public interface ITimy3Reader {

        void Init();

        List<TimingValue> WaitForBulk();
    }
}
