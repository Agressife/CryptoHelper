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
    /// Логика взаимодействия для regin.xaml
    /// </summary>
    public partial class regin : Page
    {
        public MainWindow mainWindow;
        public regin(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void toEnterReg_Click(object sender, RoutedEventArgs e)
        {
            if (regTB.Text.Length > 0)
            {
                if (passRegTB.Password.Length > 0)
                {
                    if (checkerReg(regTB.Text))
                    {
                        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
                        SqlConnection connection = new SqlConnection(connectionString);
                        string sqlExpression = $"INSERT INTO users(login,password) VALUES ('{regTB.Text}','{passRegTB.Password}')";
                        connection.Open();
                        string sql = $"CREATE TABLE {regTB.Text} (id INT, cryptoName TEXT, cryptoCount FLOAT, cryptoPrice DECIMAL)";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        connection.Open();
                        SqlCommand createTable = new SqlCommand(sql, connection);
                        SqlDataReader dataReader = createTable.ExecuteReader();
                        mainWindow.OpenPage(MainWindow.pages.login);
                        MessageBox.Show("Вы успешно зарегестрировались!");
                        connection.Close();
                    }
                    else MessageBox.Show("Этот логин уже зарегестрирован,попробуйте войти в систему или придумайте другой!");
                }
                else MessageBox.Show("Вы не ввели пароль,попробуйте ещё раз!");
            }
            else MessageBox.Show("Вы не ввели логин,попробуйте ещё раз!");
        }
        public bool checkerReg(string reg)
        {
            bool check =true;
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"SELECT * FROM users WHERE login='{reg}'";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                check = false;
            }
            return check;
        }

        private void toEnter_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.login);
        }
    }
}
