using System;
using System.Collections.Generic;
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
using Terminal.Collector.Core;

namespace Terminal.Collector.HMI.View
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        private CollectorServer server;

        public SettingsPage()
        {
            InitializeComponent();
            server = (CollectorServer)Application.Current.MainWindow.DataContext;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if(!server.IsRuning) server.StartAsync();
        }
    }
}
