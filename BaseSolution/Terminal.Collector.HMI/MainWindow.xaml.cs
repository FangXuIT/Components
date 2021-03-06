﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Terminal.Collector.HMI.Core;
using Terminal.Collector.Store;

namespace Terminal.Collector.HMI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //实例化notifyIOC控件最小化托盘
        private NotifyIcon _notifyIcon = null;

        public MainWindow()
        {
            //InitialTray();

            RedisHelper.Subscribe(("Collector_Error", msg => LogError(msg.Body)));
            RedisHelper.Subscribe(("Collector_Value", msg => UpdateTarget(msg.Body)));
            RedisHelper.Subscribe(("Collector_Status", msg => ModifyStatus(msg.Body)));
        }

        private void LogError(string msg)
        {
            LogHelper.Instance.Error(msg);
        }

        private void UpdateTarget(string content)
        {
            if(!string.IsNullOrWhiteSpace(content))
            {
                var targets = content.Split(";");
                foreach (var target in targets)
                {
                    if(!string.IsNullOrWhiteSpace(target))
                    {
                        var kv = target.Split("=");
                        TargetHelper.Instance.ModifyValue(kv[0], kv[1]);
                    }
                }
            }
        }

        private void ModifyStatus(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var kv = content.Split("=");
                PlcHelper.Instance.ModifyStatus(Convert.ToInt64(kv[0]), Convert.ToBoolean(kv[1]));
            }
        }

        #region 最小化系统托盘
        private void InitialTray()
        {
            //隐藏主窗体
            //this.Visibility = Visibility.Hidden;
            //设置托盘的各个属性
            _notifyIcon = new NotifyIcon();
            _notifyIcon.BalloonTipText = "服务运行中...";//托盘气泡显示内容
            _notifyIcon.Text = "数据采集服务";
            _notifyIcon.Visible = true;//托盘按钮是否可见
            _notifyIcon.Icon = new Icon(@"favicon.ico");//托盘中显示的图标
            _notifyIcon.ShowBalloonTip(2000);//托盘气泡显示时间
            _notifyIcon.MouseDoubleClick += _notifyIcon_MouseDoubleClick; ;
            //窗体状态改变时触发
            this.StateChanged += MainWindow_StateChanged;
        }
        
        #endregion

        #region 窗口状态改变
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region 托盘图标鼠标单击事件
        private void _notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                }
            }
        }
        #endregion

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(-2);
        }
    }
}
