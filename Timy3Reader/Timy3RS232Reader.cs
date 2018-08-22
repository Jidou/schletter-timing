using Model;
using ReaderInterfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Timy3Reader {
    public class Timy3RS232Reader : ITimy3Reader {

        private static SerialPort SerialPort;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static int MessageCount = 0;

        public void Init() {
            SerialPort = new SerialPort("COM3", 9600);
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(Receive);
            ConnectionTest();
        }


        public List<TimingValue> WaitForBulk() {
            throw new NotImplementedException();
        }


        private static void ConnectionTest() {
            SerialPort.Open();
            SerialPort.RtsEnable = true;
            SerialPort.DtrEnable = true;
        }


        public static void Receive(object sender, SerialDataReceivedEventArgs e) {
            var InputData = SerialPort.ReadExisting();
            logger.Info($"{MessageCount}: {InputData}");
            MessageCount++;
        }
    }
}
