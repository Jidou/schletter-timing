using System.Collections.Generic;
using Model;

namespace ReaderInterfaces {
    public interface ITimy3Reader {

        void Init();

        List<TimingValue> WaitForBulk();
    }
}
