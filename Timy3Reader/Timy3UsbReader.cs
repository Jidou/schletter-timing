using ReaderInterfaces;
using System;
using System.Threading;
using Alge;
using Model;
using NLog;
using System.Collections.Generic;

namespace Timy3Reader {
    public class Timy3UsbReader : ITimy3Reader {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private TimyUsb timyUsb;
        private List<TimingValue> memoryDump = new List<TimingValue>();
        private bool memoryDumpRecieved;
        private bool waitingForMemoryDump;


        public void Init() {
            timyUsb = new TimyUsb();
            timyUsb.Start();

            //timyUsb.DeviceConnected += new EventHandler<DeviceChangedEventArgs>(timyUsb_DeviceConnected);
            //timyUsb.DeviceDisconnected += new EventHandler<DeviceChangedEventArgs>(timyUsb_DeviceDisconnected);
            timyUsb.LineReceived += new EventHandler<DataReceivedEventArgs>(timyUsb_LineReceived);
            timyUsb.BytesReceived += timyUsb_BytesReceived;
            //timyUsb.RawReceived += new EventHandler<DataReceivedEventArgs>(timyUsb_RawReceived);
            //timyUsb.PnPDeviceAttached += new EventHandler(timyUsb_PnPDeviceAttached);
            //timyUsb.PnPDeviceDetached += new EventHandler(timyUsb_PnPDeviceDetached);
            timyUsb.HeartbeatReceived += new EventHandler<HeartbeatReceivedEventArgs>(HeartbeatEvent);
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


        private void HeartbeatEvent(object sender, HeartbeatReceivedEventArgs args) {
            return;
        }


        private void timyUsb_BytesReceived(object sender, BytesReceivedEventArgs e) {
            logger.Debug($"Device {e.Device.Id} Bytes: {e.Data.Length}");
        }


        private void timyUsb_LineReceived(object sender, DataReceivedEventArgs e) {
            logger.Info($"Device {e.Device.Id} Bytes: {e.Data}");

            if ( (e.Data.Contains("ALGE-TIMING") || e.Data.Contains("TIMY V 0974")) && waitingForMemoryDump ) {
                memoryDumpRecieved = true;
                waitingForMemoryDump = false;
                return;
            }

            var parsedLine = e.Data.Split(' ');

            if (waitingForMemoryDump) {
                var timingValue = new TimingValue {
                    Time = parsedLine[3],
                    MeasurementNumber = int.Parse(parsedLine[1])
                };

                memoryDump.Add(timingValue);
            }

            if (e.Data.StartsWith("PROG: ")) {
                logger.Debug(e.Data);
            }
        }


        private void timyUsb_RawReceived(object sender, DataReceivedEventArgs e) {
            if (!e.Data.StartsWith("TIMY:")) {
                logger.Info($"Device {e.Device.Id} Raw: {e.Data}");
            }
        }
    }
}
