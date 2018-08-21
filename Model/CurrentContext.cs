using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public static class CurrentContext {
        public static int SaveCounter = 0;
        public static Race Race { get; set; }
    }
}
