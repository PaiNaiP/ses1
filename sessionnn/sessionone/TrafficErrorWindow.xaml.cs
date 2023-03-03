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

namespace sessionone
{
    /// <summary>
    /// Логика взаимодействия для TrafficErrorWindow.xaml
    /// </summary>
    public partial class TrafficErrorWindow : Window
    {
        TrafficViewer trafficException;
        DBSettings _context;
        public TrafficErrorWindow(TrafficViewer trafficException, DBSettings _context)
        {
            this._context = _context;
            this.trafficException = trafficException;
            InitializeComponent();
            
            titleTextBlock.Text = "No logout detected for your last login on " + trafficException.dateFormated + " at " + trafficException.enterFormatedTime;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           var contextTraffic = _context.TrafficViewer.Find(trafficException.ID);
           contextTraffic.ExitCause = new TextRange(errorDescriptionTextBox.Document.ContentStart, errorDescriptionTextBox.Document.ContentEnd).Text;
           
            contextTraffic.CauseID = (bool)systemCrashRadio.IsChecked ? 1 : 2;
            _context.SaveChanges();
            this.Close();
        }
    }
}
