using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ReaderInterfaces;

namespace Timy3Reader {
    public class Timy3MockReader : ITimy3Reader {
        public void Init() {
            return;
        }

        public List<TimingValue> WaitForBulk() {
            return new List<TimingValue>();
        }
    }
}
