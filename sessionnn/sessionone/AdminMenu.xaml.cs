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

namespace sessionone.assets
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        private DBSettings _dataBase;

        public AdminMenu()
        {
            _dataBase = DBSettings.getInstance();
            InitializeComponent();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
            this.Close();
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addMenu = new AddUserWindow();
            addMenu.Owner = Window.GetWindow(this);
            addMenu.Closing += ChangeMenu_Closing;
            addMenu.ShowDialog();
        }


        private void usersGrid_Loaded(object sender, RoutedEventArgs e)
        {
            initGrid();
        }
        void initGrid()
        {
          
            List<UsermodelToShow> users = new List<UsermodelToShow>();
            foreach (var user in _dataBase.Users.ToList())
            {
                users.Add(new UsermodelToShow(user.ID, user.getAge(), user.Email, user.Firstname, user.Lastname, user.Offices, user.Active, user.Roles));
            }
            usersGrid.ItemsSource = users;
        }

        private void comboBoxOffices_Loaded(object sender, RoutedEventArgs e)
        {
            var currentComboBox = (sender as ComboBox);
            var actualOffices = _dataBase.Offices.ToList();
            var mockOffice = new Offices();
            mockOffice.Title = "All offices";
            actualOffices.Add(mockOffice);
            currentComboBox.ItemsSource = actualOffices;
            currentComboBox.SelectedItem = mockOffice;
        }

        void changeSelection()
        {
            var pickedOffice = comboBoxOffices.SelectedItem as Offices;
            List<UsermodelToShow> users = new List<UsermodelToShow>();

            if (pickedOffice.Title == "All offices")
            {
                foreach (var user in _dataBase.Users.ToList())
                {
                    users.Add(new UsermodelToShow(user.ID, user.getAge(), user.Email, user.Firstname, user.Lastname, user.Offices, user.Active, user.Roles));
                }
            }
            else
            {
                foreach (var user in _dataBase.Users.Where((element) => element.OfficeID == pickedOffice.ID).ToList())
                {
                    users.Add(new UsermodelToShow(user.ID, user.getAge(), user.Email, user.Firstname, user.Lastname, user.Offices, user.Active, user.Roles));
                }
            }
            
            usersGrid.ItemsSource = users;
        }

        private void comboBoxOffices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSelection();
        }

        private void banButton_Click(object sender, RoutedEventArgs e)
        {
            var loadedUp = usersGrid.SelectedItem as UsermodelToShow;
            _dataBase.Users.First((element) => element.ID == loadedUp.id).Active = !loadedUp.isActive;
            _dataBase.SaveChanges();
            changeSelection();
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            var loadedUp = usersGrid.SelectedItem as UsermodelToShow;
            if (loadedUp == null)
                return;
            var selectedUser = _dataBase.Users.First((element) => element.ID == loadedUp.id);
            var changeMenu = new ChangeUser(selectedUser);
            changeMenu.Owner = Window.GetWindow(this);
            changeMenu.Closing += ChangeMenu_Closing;
            changeMenu.ShowDialog();
        }

        private void ChangeMenu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            changeSelection();
        }
    }
}
