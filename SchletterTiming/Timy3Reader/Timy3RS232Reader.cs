using Model;
using ReaderInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timy3Reader {
    public class Timy3RS232Reader : ITimy3Reader {
        public void Init() {
            throw new NotImplementedException();
        }

        public IEnumerable<Dummy> WaitForBulk() {
            throw new NotImplementedException();
        }
    }
}
