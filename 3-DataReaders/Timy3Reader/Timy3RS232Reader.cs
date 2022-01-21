using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using NLog;
using SchletterTiming.Model;
using SchletterTiming.ReaderInterfaces;

namespace SchletterTiming.Timy3Reader {
    public class Timy3RS232Reader : ITimy3Reader {

        private const string SerialPortName = "COM3";
        private const int BaudRate = 9600;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private static SerialPort _serialPort;
        private static readonly int MessageCount = 0;
        private static int _internalIdCounter = 0;

        private List<TimingValue> _memoryDump = new List<TimingValue>();
        private bool _memoryDumpRecieved;
        private bool _waitingForMemoryDump;

        private bool _isInitialized = false;

        public void Init() {
            _serialPort = new SerialPort(SerialPortName, BaudRate);
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(Receive);
            ConnectionTest();
            _isInitialized = true;
        }


        public List<TimingValue> WaitForBulk() {
            if (!_isInitialized) {
                Init();
            }

            _memoryDumpRecieved = false;
            _waitingForMemoryDump = true;
            _memoryDump = new List<TimingValue>();

            while (!_memoryDumpRecieved) {
                Thread.Sleep(500);
            }

            return _memoryDump;
        }


        private static void ConnectionTest() {
            _serialPort.Open();
            _serialPort.RtsEnable = true;
            _serialPort.DtrEnable = true;
        }


        public void Receive(object sender, SerialDataReceivedEventArgs e) {
            var dataReceived = _serialPort.ReadTo("\r");
            logger.Info($"{MessageCount}: {dataReceived}");

            var parsedLine = dataReceived.Split(' ');

            if (!_waitingForMemoryDump) {
                return;
            }

            if (parsedLine.Length == 7) {

                var timingValue = new TimingValue {
                    Time = parsedLine[3],
                    MeasurementNumber = int.Parse(parsedLine[1]),
                    InternalId = _internalIdCounter++,
                };

                _memoryDump.Add(timingValue);
            } else if (dataReceived.Contains("ALGE-TIMING") || dataReceived.Contains("TIMY V 0974")) {
                _memoryDumpRecieved = true;
                _waitingForMemoryDump = false;
            }
        }
    }
}
