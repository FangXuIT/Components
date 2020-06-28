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
using System.Linq;
using Terminal.Collector.HMI.Core;

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

            dgTargetList.ItemsSource = TargetHelper.Instance.Targets;
        }
    }
}
