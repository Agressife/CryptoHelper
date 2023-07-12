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
using HtmlAgilityPack;
using System.Data.SqlClient;
namespace Crypto_HelperV4
{
    /// <summary>
    /// Логика взаимодействия для workspace.xaml
    /// </summary>
    public partial class workspace : Page
    {
        public ItemCollection Collection { get; set;}
        public MainWindow mainWindow;       
        public const string StartPageLink = @"https://www.google.com/search?q=%D0%BA%D1%83%D1%80%D1%81+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D0%B0&oq=%D0%BA%D1%83%D1%80%D1%81+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D0%B0&aqs=chrome..69i57j0i10i512j0i10i131i433j0i512j0i10i131i433j0i10j69i61l2.1878j0j7&sourceid=chrome&ie=UTF-8";
        List<Crypto> cryptos = new List<Crypto>();
        static Crypto somecrypto = new Crypto();
        public workspace(MainWindow _mainWindow)
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"SELECT cryptoName, cryptoCount, cryptoPrice FROM {currentLogin.curlog}";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var crypto = new Crypto();
                    crypto.setName(reader.GetValue(0).ToString());
                    crypto.allTokens = Convert.ToDouble(reader.GetValue(1));
                    crypto.usdForTokenBought = (decimal)reader.GetValue(2);
                    crypto.setTicket();
                    crypto.setUsdFTN();
                    crypto.setUsdFAT();
                    crypto.setUsdFATN();
                    crypto.setUsdIncome();
                    crypto.setPercentOI();
                    cryptos.Add(crypto);
                }
            mainWindow = _mainWindow;
            var htmlDoc = new HtmlWeb().Load(StartPageLink);
            var rows = htmlDoc.DocumentNode.SelectNodes("//div[@class='dDoNo ikb4Bb gsrt']//span[@class='DFlfde SwHCTb']");
            foreach (var cell in rows)
            {
                currentRateTB.Text = cell.InnerText + " ₽";
                break;
            }
            var htmlBitcoin = new HtmlWeb().Load(@"https://www.google.com/search?q=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D0%B1%D0%B8%D1%82%D0%BA%D0%BE%D0%B8%D0%BD%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&oq=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D0%B1%D0%B8%D1%82%D0%BA%D0%BE%D0%B8%D0%BD%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&aqs=chrome.0.0i512j69i57.4474j1j7&sourceid=chrome&ie=UTF-8");
            var what = htmlBitcoin.DocumentNode.SelectNodes("//span[@class='pclqee']");
            foreach(var cell in what)
            {
                currentBitcoinTB.Text = cell.InnerText + " $";
                break;
            }
            var htmlEth = new HtmlWeb().Load(@"https://www.google.com/search?q=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D1%8D%D1%84%D0%B8%D1%80%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&sxsrf=AOaemvJT_bi26zX4D0o52QZcjphu3Imlsw%3A1637747168395&ei=4AmeYfrVF_mGwPAPsvKfoAg&ved=0ahUKEwj6xdDR27D0AhV5AxAIHTL5B4QQ4dUDCA4&uact=5&oq=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D1%8D%D1%84%D0%B8%D1%80%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&gs_lcp=Cgdnd3Mtd2l6EAMyBQgAEM0CMgUIABDNAjIFCAAQzQIyBQgAEM0COgcIIxCwAxAnOgcIABBHELADSgUIPBIBMUoECEEYAEoECEYYAFD4BFiLCGC0CWgBcAJ4AIABfogB0wSSAQMwLjWYAQCgAQHIAQnAAQE&sclient=gws-wiz");
            var eth = htmlEth.DocumentNode.SelectNodes("//span[@class='pclqee']");
            foreach (var cell in eth)
            {
                currentETHTB.Text = cell.InnerText + " $";
                break;
            }
            tableOutput.ItemsSource = cryptos;
            decimal overallIncome = 0;
            decimal overallSpent = 0;
            foreach(var cr in cryptos)
            {
                overallIncome += cr.usdIncome;
                overallSpent += cr.usdForAllToken;
            }
            if (overallSpent != 0)
            {
                overallPercent.Text = (overallIncome * 100 / overallSpent).ToString();
                overallincome.Text = overallIncome.ToString();
            }
            else overallPercent.Text = "0";
        }
        private void AddCrypto()
        {
            if (inputCryptoName.Visibility != Visibility.Hidden)
            {

                if (inputCryptoName.Text == "" || inputCryptoCount.Text == "" || inputCryptoBought.Text == "")
                {
                    MessageBox.Show("Вы ввели не все значения, попробуйте ещё раз!");
                }
                else
                {
                    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    var crypto = new Crypto();
                    crypto.setName(inputCryptoName.Text);
                    crypto.setTicket();
                    crypto.allTokens = Convert.ToDouble(inputCryptoCount.Text);
                    crypto.usdForTokenBought = Convert.ToDecimal(inputCryptoBought.Text);
                    crypto.setUsdFAT();
                    crypto.setUsdFTN();
                    crypto.setUsdFATN();
                    crypto.setUsdIncome();
                    crypto.setPercentOI();
                    cryptos.Add(crypto);
                    tableOutput.ItemsSource = cryptos;
                    string usdfatb = "";
                    foreach (var ch in inputCryptoBought.Text)
                    {
                        if (ch == '.' || ch == ',')
                        {
                            usdfatb += '.';
                        }
                        else
                        {
                            usdfatb += ch;
                        }
                    }
                    string counter = crypto.allTokens.ToString();
                    string newcounter = "";
                    string boughter = crypto.usdForTokenBought.ToString();
                    string newboughter = "";
                    for(int i=0;i<counter.Length;i++)
                    {
                        if(counter[i]==',')
                        {
                            newcounter+='.';
                        }
                        else
                        {
                            newcounter += counter[i];
                        }
                    }
                    for (int i = 0; i < boughter.Length; i++)
                    {
                        if (boughter[i] == ',')
                        {
                            newboughter += '.';
                        }
                        else
                        {
                            newboughter += boughter[i];
                        }
                    }
                    string sqlExpression = $"INSERT INTO {currentLogin.curlog}(id,cryptoName,cryptoCount,cryptoPrice) VALUES (0,'{crypto.name}',{newcounter},{usdfatb})";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    insertCrypto.Visibility = Visibility.Hidden;
                    inputCryptoBought.Text = null;
                    inputCryptoCount.Text = null;
                    inputCryptoName.Text = null;
                    inputCryptoBought.Visibility = Visibility.Hidden;
                    inputCryptoCount.Visibility = Visibility.Hidden;
                    inputCryptoName.Visibility = Visibility.Hidden;
                    labelName.Visibility = Visibility.Hidden;
                    labelCount.Visibility = Visibility.Hidden;
                    labelBought.Visibility = Visibility.Hidden;
                }
            }
        }
        private void insertCrypto_Click(object sender, RoutedEventArgs e)
        {
            if(inputCryptoName.Visibility==Visibility.Hidden)
            {
                inputCryptoBought.Visibility = Visibility.Visible;
                inputCryptoCount.Visibility = Visibility.Visible;
                inputCryptoName.Visibility = Visibility.Visible;
                labelName.Visibility = Visibility.Visible;
                labelCount.Visibility = Visibility.Visible;
                labelBought.Visibility = Visibility.Visible;
            }
            else
            {
                AddCrypto();
            }
        }
        public void refresher()
        {
            inputCryptoBought.Visibility = Visibility.Hidden;
            inputCryptoCount.Visibility = Visibility.Hidden;
            inputCryptoName.Visibility = Visibility.Hidden;
            labelName.Visibility = Visibility.Hidden;
            labelCount.Visibility = Visibility.Hidden;
            labelBought.Visibility = Visibility.Hidden;
            insertCrypto.Visibility = Visibility.Visible;
            insertCrypto_Copy.Visibility = Visibility.Hidden;
            changeCrypto.Visibility = Visibility.Hidden;
            insertCrypto.Visibility = Visibility.Hidden;
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"SELECT cryptoName, cryptoCount, cryptoPrice FROM {currentLogin.curlog}";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            cryptos.Clear();
            while (reader.Read())
            {
                var crypto = new Crypto();
                crypto.setName(reader.GetValue(0).ToString());
                crypto.allTokens = Convert.ToDouble(reader.GetValue(1));
                crypto.usdForTokenBought = (decimal)reader.GetValue(2);
                crypto.setTicket();
                crypto.setUsdFTN();
                crypto.setUsdFAT();
                crypto.setUsdFATN();
                crypto.setUsdIncome();
                crypto.setPercentOI();
                cryptos.Add(crypto);
            }
            var htmlDoc = new HtmlWeb().Load(StartPageLink);
            var rows = htmlDoc.DocumentNode.SelectNodes("//div[@class='dDoNo ikb4Bb gsrt']//span[@class='DFlfde SwHCTb']");
            foreach (var cell in rows)
            {
                currentRateTB.Text = cell.InnerText + " Рубля";
                break;
            }
            var htmlBitcoin = new HtmlWeb().Load(@"https://www.google.com/search?q=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D0%B1%D0%B8%D1%82%D0%BA%D0%BE%D0%B8%D0%BD%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&oq=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D0%B1%D0%B8%D1%82%D0%BA%D0%BE%D0%B8%D0%BD%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&aqs=chrome.0.0i512j69i57.4474j1j7&sourceid=chrome&ie=UTF-8");
            var what = htmlBitcoin.DocumentNode.SelectNodes("//span[@class='pclqee']");
            foreach (var cell in what)
            {
                currentBitcoinTB.Text = cell.InnerText + " $";
                break;
            }
            var htmlEth = new HtmlWeb().Load(@"https://www.google.com/search?q=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D1%8D%D1%84%D0%B8%D1%80%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&sxsrf=AOaemvJT_bi26zX4D0o52QZcjphu3Imlsw%3A1637747168395&ei=4AmeYfrVF_mGwPAPsvKfoAg&ved=0ahUKEwj6xdDR27D0AhV5AxAIHTL5B4QQ4dUDCA4&uact=5&oq=%D0%BE%D1%82%D0%BD%D0%BE%D1%88%D0%B5%D0%BD%D0%B8%D0%B5+%D1%8D%D1%84%D0%B8%D1%80%D0%B0+%D0%BA+%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80%D1%83&gs_lcp=Cgdnd3Mtd2l6EAMyBQgAEM0CMgUIABDNAjIFCAAQzQIyBQgAEM0COgcIIxCwAxAnOgcIABBHELADSgUIPBIBMUoECEEYAEoECEYYAFD4BFiLCGC0CWgBcAJ4AIABfogB0wSSAQMwLjWYAQCgAQHIAQnAAQE&sclient=gws-wiz");
            var eth = htmlEth.DocumentNode.SelectNodes("//span[@class='pclqee']");
            foreach (var cell in eth)
            {
                currentETHTB.Text = cell.InnerText + " $";
                break;
            }
            tableOutput.ItemsSource = cryptos;
            decimal overallIncome = 0;
            decimal overallSpent = 0;
            foreach (var cr in cryptos)
            {
                overallIncome += cr.usdIncome;
                overallSpent += cr.usdForAllToken;
            }
            if (overallSpent != 0)
            {
                overallPercent.Text = (overallIncome * 100 / overallSpent).ToString();
                overallincome.Text = overallIncome.ToString();
            }
            else overallPercent.Text = "0";
            tableOutput.Items.Refresh();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            refresher();
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.login);
        }

        private void insertCrypto_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (inputCryptoBought.Visibility == Visibility.Hidden)
            {
                MessageBox.Show("Заполните форму или нажмите на транзакцию в таблице!");
                inputCryptoBought.Visibility = Visibility.Visible;
                inputCryptoCount.Visibility = Visibility.Visible;
                inputCryptoName.Visibility = Visibility.Visible;
                labelName.Visibility = Visibility.Visible;
                labelCount.Visibility = Visibility.Visible;
                labelBought.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteCrypto();
                inputCryptoBought.Visibility = Visibility.Hidden;
                inputCryptoCount.Visibility = Visibility.Hidden;
                inputCryptoName.Visibility = Visibility.Hidden;
                labelName.Visibility = Visibility.Hidden;
                labelCount.Visibility = Visibility.Hidden;
                labelBought.Visibility = Visibility.Hidden;
                insertCrypto.Visibility = Visibility.Hidden;
            }
        }
        private void DeleteCrypto()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"DELETE FROM {currentLogin.curlog} WHERE cryptoCount={inputCryptoCount.Text} AND cryptoPrice={inputCryptoBought.Text}";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            refresher();
            insertCrypto_Copy.Visibility = Visibility.Hidden;
            inputCryptoBought.Text = null;
            inputCryptoCount.Text = null;
            inputCryptoName.Text = null;
        }
        private void tableOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tableOutput.SelectedItem != null)
            {
                inputCryptoBought.Visibility = Visibility.Visible;
                inputCryptoCount.Visibility = Visibility.Visible;
                labelCount.Visibility = Visibility.Visible;
                labelBought.Visibility = Visibility.Visible;
                insertCrypto_Copy.Visibility = Visibility.Visible;
                insertCrypto.Visibility = Visibility.Visible;
                changeCrypto.Visibility = Visibility.Visible;
                somecrypto = (Crypto)tableOutput.SelectedItem;
                inputCryptoCount.Text = somecrypto.allTokens.ToString();
                inputCryptoBought.Text = somecrypto.usdForTokenBought.ToString();
            }
            else
            {
                inputCryptoBought.Visibility = Visibility.Hidden;
                inputCryptoCount.Visibility = Visibility.Hidden;
                inputCryptoName.Visibility = Visibility.Hidden;
                labelName.Visibility = Visibility.Hidden;
                labelCount.Visibility = Visibility.Hidden;
                labelBought.Visibility = Visibility.Hidden;
                insertCrypto.Visibility = Visibility.Visible;
                insertCrypto_Copy.Visibility = Visibility.Hidden;
            }
        }

        private void changeCrypto_Click(object sender, RoutedEventArgs e)
        {
            if (inputCryptoBought.Visibility == Visibility.Hidden)
            {
                MessageBox.Show("Заполните форму или нажмите на транзакцию в таблице!");
                inputCryptoBought.Visibility = Visibility.Visible;
                inputCryptoCount.Visibility = Visibility.Visible;
                inputCryptoName.Visibility = Visibility.Visible;
                labelName.Visibility = Visibility.Visible;
                labelCount.Visibility = Visibility.Visible;
                labelBought.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeCrypto();
                inputCryptoBought.Visibility = Visibility.Hidden;
                inputCryptoCount.Visibility = Visibility.Hidden;
                inputCryptoName.Visibility = Visibility.Hidden;
                labelName.Visibility = Visibility.Hidden;
                labelCount.Visibility = Visibility.Hidden;
                labelBought.Visibility = Visibility.Hidden;
                insertCrypto.Visibility = Visibility.Hidden;

            }
        }
        private void ChangeCrypto()
        {
            changeCrypto.Visibility = Visibility.Hidden;
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=cryptoHelper;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = $"UPDATE {currentLogin.curlog} SET cryptoCount={inputCryptoCount.Text}, cryptoPrice={inputCryptoBought.Text} WHERE cryptoCount={somecrypto.allTokens.ToString()} AND cryptoPrice={somecrypto.usdForTokenBought.ToString()}";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            refresher();
            inputCryptoBought.Text = null;
            inputCryptoCount.Text = null;
            inputCryptoName.Text = null;
        }

        private void inputCryptoName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputCryptoName.Text == null || inputCryptoCount.Text == null || inputCryptoBought.Text == null)
            {
            }
            else
            {
                insertCrypto.Visibility = Visibility.Visible;
            }
        }
    }
}
