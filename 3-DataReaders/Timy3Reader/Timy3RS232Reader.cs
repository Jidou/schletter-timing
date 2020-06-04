using Model;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using System.Threading;
using SchletterTiming.ReaderInterfaces;

namespace Timy3Reader {
    public class Timy3RS232Reader : ITimy3Reader {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private static SerialPort SerialPort;
        private static int MessageCount = 0;

        private List<TimingValue> memoryDump = new List<TimingValue>();
        private bool memoryDumpRecieved;
        private bool waitingForMemoryDump;


        public void Init() {
            SerialPort = new SerialPort("COM3", 9600);
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(Receive);
            ConnectionTest();
        }


        public List<TimingValue> WaitForBulk() {
            memoryDumpRecieved = false;
            waitingForMemoryDump = true;
            memoryDump = new List<TimingValue>();

            while (!memoryDumpRecieved) {
                Thread.Sleep(500);
            }

            return memoryDump;
        }


        private static void ConnectionTest() {
            SerialPort.Open();
            SerialPort.RtsEnable = true;
            SerialPort.DtrEnable = true;
        }


        public void Receive(object sender, SerialDataReceivedEventArgs e) {
            var dataReceived = SerialPort.ReadTo("\r");
            logger.Info($"{MessageCount}: {dataReceived}");

            var parsedLine = dataReceived.Split(' ');

            if (waitingForMemoryDump) {

                if (parsedLine.Length == 7) {

                    var timingValue = new TimingValue {
                        Time = parsedLine[3],
                        MeasurementNumber = int.Parse(parsedLine[1])
                    };

                    memoryDump.Add(timingValue);
                } else if (dataReceived.Contains("ALGE-TIMING") || dataReceived.Contains("TIMY V 0974")) {
                    memoryDumpRecieved = true;
                    waitingForMemoryDump = false;
                }
            }
        }
    }
}
