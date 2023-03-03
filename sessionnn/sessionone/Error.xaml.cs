using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace sessionone
{
    /// <summary>
    /// Логика взаимодействия для Error.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        String subtitle;
        bool isCountDown;
        DispatcherTimer _timer;
        TimeSpan _time;
        public ErrorWindow(String title, String subtitle, bool isCountDown)
        {
            this.subtitle = subtitle;
            this.isCountDown = isCountDown;
        
            InitializeComponent();
            this.titleText.Text = title;
            if (!isCountDown)
            {
               this.desriptionText.Text = subtitle;
            }
            if (isCountDown)
            {
            
                this.closeButton.IsEnabled = false;
                initTimer(int.Parse(subtitle));
            }
        }
        
        private void initTimer(int defaultValue)
        {
            _time = TimeSpan.FromSeconds(defaultValue);
            desriptionText.Text = "Превышено колличество попыток входа. \n" +
               "Времени осталось: " + _time.ToString("c");
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                desriptionText.Text = "Превышено колличество попыток входа. \n" +
                "Времени осталось: " + _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    this.closeButton.IsEnabled = true;
                    this.Close();
                }
                
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
