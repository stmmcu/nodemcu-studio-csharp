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
        private readonly SP _currentSp;
        private readonly object _lock = new object();

        private const int MaxRetries = 100;

        private SerialPort()
        {
            _currentSp = new SP();
        }

        public string[] GetPortNames()
        {
            return SP.GetPortNames();
        }

        public void Close()
        {
            if (_currentSp.IsOpen)
            {
                _currentSp.Close();
                IsOpenChanged?.Invoke(_currentSp.IsOpen);
            }
        }

        public bool Open(string port)
        {
            Close();
            _currentSp.BaudRate = 9600;
            _currentSp.ReadTimeout = 0;
            _currentSp.PortName = port;
            _currentSp.Open();
            IsOpenChanged?.Invoke(_currentSp.IsOpen);
            return _currentSp.IsOpen;
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
            _currentSp.WriteLine(command);
            for (var i = 0;i < MaxRetries;i++)
            {
                string s = _currentSp.ReadExisting();
                OnDataReceived?.Invoke(s);
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
            if (_instance == null)
            {
                _instance = new SerialPort();
            }
            return _instance;
        }

        public void Dispose()
        {
            _currentSp.Dispose();
        }

        public event Action<bool> IsOpenChanged;
        public event Action<string> OnDataReceived;
        public event Action<bool> IsWorkingChanged;
    }
}
