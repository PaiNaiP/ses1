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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using sessionone.assets;

namespace sessionone
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBSettings _dataBase;
        int logCounter = 0;

        public MainWindow()
        {
            _dataBase = DBSettings.getInstance();
            InitializeComponent();
        }

        private void _exitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void _loginButtonClick(object sender, RoutedEventArgs e)
        {
           var hash = Hasher.hashToMD5(passwordField.Password);
            ErrorWindow errorWindow;
            try
            {
               
                var user = _dataBase.Users.First((element) => element.Password.Trim() == hash && element.Email == loginField.Text.Trim());
                if (!user.Active)
                {
                    errorWindow =  (new ErrorWindow("Ошибка авторизации", "Вы заблокированы, обратитесь к администратору.", false));
                    errorWindow.Owner = Window.GetWindow(this);
                    errorWindow.Show();
                }
                else
                {
                    if (user.Roles.Title == "Administrator")
                    {
                        (new AdminMenu()).Show();
                    }
                    else
                    {
                        (new UserMenu(user)).Show();
                    }
                    this.Close();

                }
            }
            catch 
            {
                logCounter++;
                if (logCounter == 3)
                {
                    logCounter = 0;
                   errorWindow = (new ErrorWindow("Ошибка авторизации", "10", true));

                }
                else
                {
                   errorWindow = (new ErrorWindow("Ошибка авторизации", "Введенные данные некорректны, повторите попытку.", false));
                }
                errorWindow.Owner = Window.GetWindow(this);
                errorWindow.ShowDialog();
            }
        }
    }
}
