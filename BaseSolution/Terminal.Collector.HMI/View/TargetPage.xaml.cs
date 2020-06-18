using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Terminal.Collector.Core.Scan;
using System.Linq;

namespace Terminal.Collector.HMI.View
{
    /// <summary>
    /// TargetPage.xaml 的交互逻辑
    /// </summary>
    public partial class TargetPage : Page
    {
        public TargetPage()
        {
            InitializeComponent();

            cbChannelList.ItemsSource = TerminalClient.Instance.ChannelList;
            cbChannelList.DisplayMemberPath = "Name";
            cbChannelList.SelectedValuePath = "Id";
        }

        

        private void cbChannelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbChannelList.SelectedIndex>-1)
            {
                Int64 cid = Convert.ToInt64(cbChannelList.SelectedValue);
                var channel = (from u in TerminalClient.Instance.ChannelList where u.Id == cid select u).FirstOrDefault();

                dgTargetList.ItemsSource = channel.Nodes.Values.ToList();
            }
        }
    }
}
