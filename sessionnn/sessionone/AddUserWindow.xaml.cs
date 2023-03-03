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
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        DBSettings _context;
        public AddUserWindow()
        {
            _context = DBSettings.getInstance();
            InitializeComponent();
            officeComboBox.ItemsSource = _context.Offices.ToList();
            officeComboBox.SelectedIndex = 0;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorWindow exceptionWindow = null;
            if (lastNameTextBox.Text.Trim().Length == 0)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "Last name must be not empty", false);
            }
            if (firstNameTextBox.Text.Trim().Length == 0)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "First name must be not empty", false);

            }
            if (passwordBox.Password.Trim().Length == 0)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "Password must be not empty", false);

            }
            if (birthDayPicker.SelectedDate == null || getAge(birthDayPicker.SelectedDate.Value) < 18)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "Birthady date incorrect for adult person", false);

            }
            if (emailTextBox.Text.Trim().Length == 0)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "Email must be not empty", false);


            }
            if (_context.Users.Where((element) => element.Email == emailTextBox.Text).Count() != 0)
            {
                exceptionWindow = new ErrorWindow("Error occured while adding", "Email addres already taken by another account", false);

            }
            if (exceptionWindow != null)
            {
                exceptionWindow.Owner = Window.GetWindow(this);
                exceptionWindow.ShowDialog();
                return;
            }
            

            var currentUser = new Users();
            currentUser.Email = emailTextBox.Text;
            currentUser.Lastname = lastNameTextBox.Text;
            currentUser.Firstname = firstNameTextBox.Text;
            currentUser.Birthdate = DateTime.Parse(birthDayPicker.Text);
            currentUser.Password = Hasher.hashToMD5(passwordBox.Password);
            currentUser.RoleID = 2;
            currentUser.Active = true;
            currentUser.Offices = officeComboBox.SelectedItem as Offices;
            _context.Users.Add(currentUser);
            _context.SaveChanges();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public int getAge(DateTime Birthdate)
        {
            TimeSpan span = DateTime.Now - Birthdate;
            int years = (new DateTime(1, 1, 1) + span).Year - 1;
            return years;
        }
    }
}
