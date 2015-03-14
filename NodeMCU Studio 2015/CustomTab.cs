using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NodeMCU_Studio_2015
{
    class CustomTab : TabItem
    {
        private CustomHeader customHeader;

        public CustomTab()
        {
            customHeader = new CustomHeader();
            this.Header = customHeader;

            customHeader.button_Close.MouseEnter += new MouseEventHandler(button_Close_MouseEnter);
            customHeader.button_Close.MouseLeave += new MouseEventHandler(button_Close_MouseLeave);
            customHeader.button_Close.Click += new RoutedEventHandler(button_Close_Click);
            customHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
        }

        public string Title
        {
            set
            {
                customHeader.label_TabTitle.Content = value;
            }
            get
            {
                return customHeader.label_TabTitle.Content as string;
            }
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            customHeader.button_Close.Visibility = Visibility.Visible;
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            customHeader.button_Close.Visibility = Visibility.Hidden;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            customHeader.button_Close.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                customHeader.button_Close.Visibility = Visibility.Hidden;
            }
        }

        private void button_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            customHeader.button_Close.Foreground = Brushes.Red;
        }

        private void button_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            customHeader.button_Close.Foreground = Brushes.Black;
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as TabControl).Items.Remove(this);
        }

        private void label_TabTitle_SizeChanged(object sender, RoutedEventArgs e)
        {
            customHeader.button_Close.Margin = new Thickness(
                customHeader.label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }
    }
}
