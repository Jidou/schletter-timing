using System;
using System.Collections.Generic;
using System.Threading;
using Alge;
using NLog;
using SchletterTiming.Model;
using SchletterTiming.ReaderInterfaces;

namespace SchletterTiming.Timy3Reader {
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
            logger.Trace($"Device {e.Device.Id} Bytes: {e.Data.Length}");
        }


        private void timyUsb_LineReceived(object sender, DataReceivedEventArgs e) {
            logger.Info($"Device {e.Device.Id} Bytes: {e.Data}");

            var parsedLine = e.Data.Split(' ');

            if (waitingForMemoryDump) {
                TimingValue timingValue;

                if (parsedLine[0].StartsWith("c")) {
                    timingValue = new TimingValue {
                        MeasurementNumber = TryParseMeasurementNumber(parsedLine[0]),
                        Time = parsedLine[2],
                    };
                } else {
                    timingValue = new TimingValue {
                        MeasurementNumber = TryParseMeasurementNumber(parsedLine[1]),
                        Time = parsedLine[3],
                    };
                }

                memoryDump.Add(timingValue);
            } else if (e.Data.Contains("ALGE-TIMING") || e.Data.Contains("TIMY V 0974")) {
                    memoryDumpRecieved = true;
                    waitingForMemoryDump = false;
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


        private int TryParseMeasurementNumber(string value) {
            if (!int.TryParse(value, out var measurement)) {
                value = value.Remove(0, 2);
                if (!int.TryParse(value, out measurement)) {
                    measurement = -1;
                }
            }

            return measurement;
        }
    }
}
