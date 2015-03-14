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

        public string[] GetPortNames()
        {
            return SP.GetPortNames();
        }

        public void Close()
        {
            if (currentSP.IsOpen)
            {
                currentSP.Close();
            }
        }

        public bool Open(string port)
        {
            Close();
            currentSP.BaudRate = 9600;
            currentSP.ReadTimeout = 0;
            currentSP.PortName = port;
            currentSP.Open();
            return currentSP.IsOpen;
        }

        public void ExecuteAndWait(string command)
        {
            currentSP.WriteLine(command);
            for (var i = 0;i < MAX_RETRIES;i++)
            {
                string s = currentSP.ReadExisting();
                if (s.Contains("\r\n>"))
                {
                    return ;
                }
                NativeMethods.Sleep(0);
            }
            MessageBox.Show("Cannot execute command " + command);
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
    }
}
