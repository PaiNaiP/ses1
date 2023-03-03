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
using System.ComponentModel;
namespace sessionone
{
    /// <summary>
    /// Логика взаимодействия для UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Window
    {
        private DBSettings _dataBase;

        DispatcherTimer _timer;
        TimeSpan _time;
        TrafficViewer currentTraffic;
        public UserMenu(Users user)
        {
            currentTraffic = new TrafficViewer();
            currentTraffic.EnterTime = DateTime.Now;
            currentTraffic.UserID = user.ID;
            _dataBase = DBSettings.getInstance();
            
            InitializeComponent();
            helloTextBlock.Text = "Hi " + user.Lastname + ", Welcome to AMONIC Airlaince";
            var loadedEvents = user.TrafficViewer.ToList();
            foreach (var errorEvent in loadedEvents.Where((element) => element.ExitTime == null && element.ExitCauses == null).ToList())
            {
                var exceptionWindow = new TrafficErrorWindow(errorEvent, _dataBase);
                exceptionWindow.ShowDialog();
            }
            _dataBase.TrafficViewer.Add(currentTraffic);
            _dataBase.SaveChangesAsync();
            double spendedSeconds = 0;
            foreach(var errorEvent in loadedEvents.Where((element) => DateTime.Now.Subtract(element.EnterTime.Date).Days <= 30))
            {
                spendedSeconds += errorEvent.timeSpendDuration.TotalSeconds;
            }
            initTimer((int)spendedSeconds);
            Closing += OnWindowClosing;
            logGrid.ItemsSource = loadedEvents;
        }
        private void initTimer(int startTime)
        {
            _time = TimeSpan.FromSeconds(0);
            tickTextBlock.Text = "Time spent on system: "  + _time.ToString("c");
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tickTextBlock.Text = "Time spent on system: " + _time.ToString("c");
                _time = _time.Add(TimeSpan.FromSeconds(+1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            currentTraffic.ExitTime = DateTime.Now;
            _dataBase.SaveChangesAsync();
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
            this.Close();
        }

    

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
