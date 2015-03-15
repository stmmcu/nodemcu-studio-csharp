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
        private static SerialPort _instance;
        public readonly SP CurrentSp;
        private readonly object _lock = new object();

        private const int MaxRetries = 100;

        private SerialPort()
        {
            CurrentSp = new SP();
        }

        public string[] GetPortNames()
        {
            return SP.GetPortNames();
        }

        public void Close()
        {
            if (CurrentSp.IsOpen)
            {
                CurrentSp.Close();
                IsOpenChanged?.Invoke(CurrentSp.IsOpen);
            }
        }

        public bool Open(string port)
        {
            Close();
            CurrentSp.BaudRate = 9600;
            CurrentSp.ReadTimeout = 0;
            CurrentSp.PortName = port;
            CurrentSp.Open();
            IsOpenChanged?.Invoke(CurrentSp.IsOpen);
            return CurrentSp.IsOpen;
        }

        public IDisposable Use()
        {
            System.Threading.Monitor.Enter(_lock);
            IsWorkingChanged?.Invoke(true);
            return new UnLock(_lock, IsWorkingChanged);
        }

        private class UnLock : IDisposable
        {
            private readonly Object _lock;
            private readonly Action<bool> _isWorkingChanged;
            public UnLock(Object obj, Action<bool> changed)
            {
                _lock = obj;
                _isWorkingChanged = changed;
            }
            public void Dispose()
            {
                _isWorkingChanged?.Invoke(false);
                System.Threading.Monitor.Exit(_lock);
            }
        }

        public bool ExecuteAndWait(string command)
        {
            CurrentSp.WriteLine(command);
            for (var i = 0;i < MaxRetries;i++)
            {
                var s = CurrentSp.ReadExisting();
                OnDataReceived?.Invoke(s);
                if (s.Contains(">"))
                {
                    return true;
                }
                NativeMethods.Sleep(100);
            }
            return false;
        }

        public string ExecuteWaitAndRead(string command)
        {
            StringBuilder result = new StringBuilder();

            CurrentSp.WriteLine(command);
            for (var i = 0; i < MaxRetries; i++)
            {
                var s = CurrentSp.ReadExisting();
                result.Append(s);
                OnDataReceived?.Invoke(s);
                if (result.ToString().EndsWith("\n> "))
                {
                    break;
                }
                NativeMethods.Sleep(100);
            }
            var str = result.ToString();
            return str.Substring(command.Length+2, str.Length-4-2-command.Length); // Kill the echo, '\r\n' and '\r\n> '
        }

        public static SerialPort GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SerialPort();
            }
            return _instance;
        }

        public void Dispose()
        {
            CurrentSp.Dispose();
        }

        public event Action<bool> IsOpenChanged;
        public event Action<string> OnDataReceived;
        public event Action<bool> IsWorkingChanged;
    }
}
