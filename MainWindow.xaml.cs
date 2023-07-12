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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Crypto_HelperV4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(pages.login);
        }
        public string currentLogin { get; set; }
        public enum pages
        {
            login,
            regin,
            workspace
        }
        public void OpenPage(pages pages)
        {
            if (pages == pages.login)
            {
                frame.Navigate(new login(this));
            }
            else
            {
                if (pages == pages.regin)
                {
                    frame.Navigate(new regin(this));
                }
                else if (pages == pages.workspace)
                {
                    frame.Navigate(new workspace(this));
                }
            }
        }       
    }
}
