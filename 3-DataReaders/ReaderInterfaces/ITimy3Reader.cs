using System.Collections.Generic;
using Model;

namespace SchletterTiming.ReaderInterfaces {
    public interface ITimy3Reader {

        void Init();

        List<TimingValue> WaitForBulk();
    }
}
