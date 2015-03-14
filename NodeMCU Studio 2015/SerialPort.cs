using NodeMCU_Studio;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SP = System.IO.Ports.SerialPort;

namespace NodeMCU_Studio_2015
{
    class SerialPort : IDisposable
    {
        private static SerialPort instance;
        private SP currentSP;

        private const int MAX_RETRIES = 100;

        private SerialPort()
        {
            currentSP = new SP();
        }

        private void TriggerIsOpenChanged()
        {
            if (IsOpenChanged != null)
            {
                IsOpenChanged(currentSP.IsOpen);
            }
        }

        public string[] GetPortNames()
        {
            return SP.GetPortNames();
        }

        public void Close()
        {
            if (currentSP.IsOpen)
            {
                currentSP.Close();
                if (IsOpenChanged != null)
                {
                    IsOpenChanged(currentSP.IsOpen);
                }
            }
        }

        public bool Open(string port)
        {
            Close();
            currentSP.BaudRate = 9600;
            currentSP.ReadTimeout = 0;
            currentSP.PortName = port;
            currentSP.Open();
            if (IsOpenChanged != null)
            {
                IsOpenChanged(currentSP.IsOpen);
            }
            return currentSP.IsOpen;
        }

        public bool ExecuteAndWait(string command)
        {
            currentSP.WriteLine(command);
            for (var i = 0;i < MAX_RETRIES;i++)
            {
                string s = currentSP.ReadExisting();
                if (s.Contains(">"))
                {
                    return true;
                }
                NativeMethods.Sleep(100);
            }
            return false;
        }

        public static SerialPort GetInstance()
        {
            if (instance == null)
            {
                instance = new SerialPort();
            }
            return instance;
        }

        public void Dispose()
        {
            currentSP.Dispose();
        }

        public event Action<bool> IsOpenChanged;
    }
}
