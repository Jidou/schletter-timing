using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ReaderInterfaces {
    public interface ITimy3Reader {

        void Init();

        List<TimingValue> WaitForBulk();
    }
}
