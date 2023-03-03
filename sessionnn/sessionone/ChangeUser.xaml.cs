using sessionone.assets;
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
    /// Логика взаимодействия для ChangeUser.xaml
    /// </summary>
    public partial class ChangeUser : Window
    {
        DBSettings _context;
        Users currentUser;
        public ChangeUser(Users currentUser)
        {
            _context = DBSettings.getInstance();
            this.currentUser = currentUser;
            InitializeComponent();
            lastNameTextBox.Text = currentUser.Lastname;
            firstNameTextBox.Text = currentUser.Firstname;
            emailTextBox.Text = currentUser.Email;
            officeComboBox.ItemsSource = _context.Offices.ToList();
            officeComboBox.SelectedItem = currentUser.Offices;

            if (currentUser.RoleID == 1)
            {
                radioAdmin.IsChecked = true;
            }
            else
            {
                radioUser.IsChecked = true;
            }
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
            currentUser.Lastname = lastNameTextBox.Text;
            currentUser.Firstname = firstNameTextBox.Text;
            currentUser.Email = emailTextBox.Text;
            currentUser.Offices = officeComboBox.SelectedItem as Offices;
            currentUser.RoleID = (bool)radioAdmin.IsChecked ? 1 : 2;
            _context.SaveChanges();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
