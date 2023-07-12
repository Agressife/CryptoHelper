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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
namespace Crypto_HelperV4
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Page
    {
        public MainWindow mainWindow;
        public login(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void toEnter_Click(object sender, RoutedEventArgs e)
        {
            if (loginTB.Text.Length > 0)
            {
                if (passTB.Password.Length > 0)
                {
                    if (checker(loginTB.Text, passTB.Password))
                    {
                        currentLogin.curlog = loginTB.Text;
                        mainWindow.OpenPage(MainWindow.pages.workspace);
                    }
                    else MessageBox.Show("Вы ввели неверный логин или пароль!");
                }
                else MessageBox.Show("Вы не ввели пароль,попробуйте ещё раз!");
            }
            else  MessageBox.Show("Вы не ввели логин,попробуйте ещё раз!");
            
        }

        private void toRegin_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.regin);
        }
        private bool checker(string login,string password)
        {
            bool check = false;
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"SELECT * FROM users WHERE login='{login}' AND password='{password}'";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                check = true;
            }
            return check;
        }
    }
}
