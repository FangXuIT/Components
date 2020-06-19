﻿using System;
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
using Terminal.Collector.Core;
using System.Linq;

namespace Terminal.Collector.HMI.View
{
    /// <summary>
    /// InstancePage.xaml 的交互逻辑
    /// </summary>
    public partial class InstancePage : Page
    {
        public InstancePage()
        {
            InitializeComponent();

            var server = (CollectorServer)Application.Current.MainWindow.DataContext;
            gdIntance.ItemsSource = (from u in server.InstanceList select u.Channel).ToList();
        }
    }
}