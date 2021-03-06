﻿using ControlzEx.Standard;
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

namespace Terminal.Collector.HMI.View
{
    /// <summary>
    /// HamburgerMenu.xaml 的交互逻辑
    /// </summary>
    public partial class HamburgerMenu : UserControl
    {
        public HamburgerMenu()
        {
            InitializeComponent();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, MahApps.Metro.Controls.HamburgerMenuItemInvokedEventArgs e)
        {
            this.HamburgerMenuControl.Content = e.InvokedItem;

            if (!e.IsItemOptions && this.HamburgerMenuControl.IsPaneOpen)
            {
                // close the menu if a item was selected
                this.HamburgerMenuControl.IsPaneOpen = false;
            }
        }
    }
}
