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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            InitializeComponent();
            SynchronizationContext context = SynchronizationContext.Current;

            new Task(() => {
                System.Threading.Thread.Sleep(1000);

                context.Post((_) =>
                {
                    Hide();
                    PowerfulLuaEditor editor = new PowerfulLuaEditor(this);
                    editor.Show();
                }, null);
                
            }).Start();
        }
    }
}
