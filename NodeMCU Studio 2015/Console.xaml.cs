using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NodeMCU_Studio_2015
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : Window
    {

        private readonly SynchronizationContext _context;
        public Console()
        {
            InitializeComponent();

            _context = SynchronizationContext.Current;
            SerialPort.GetInstance().OnDataReceived += delegate(string s)
            {
                _context.Post(_ =>
                {
                    ConsoleTextBox.AppendText(s);
                    ConsoleTextBox.ScrollToEnd();
                }, null);
            };
            SerialPort.GetInstance().IsWorkingChanged += delegate(bool isWorking)
            {
                _context.Post(_ =>
                {
                    Execute.IsEnabled = !isWorking;
                }, null);
            };
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            var command = TextBox.Text;
            TextBox.Text = "";
            new Thread(() =>
            {
                using (SerialPort.GetInstance().Use())
                {
                    try
                    {
                        SerialPort.GetInstance().ExecuteAndWait(command);
                    } catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }
                }
            }).Start();
        }
    }
}
