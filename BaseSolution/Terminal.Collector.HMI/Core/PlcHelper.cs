using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Terminal.Collector.IStore;
using Terminal.Collector.Store;

namespace Terminal.Collector.HMI.Core
{
    public class PlcHelper
    {
        #region --单例--
        public static PlcHelper Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            internal static readonly PlcHelper instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new PlcHelper();
            }
        }

        private PlcHelper()
        {
            Plcs = new ObservableCollection<Plc>();
            PlcIndex = new Dictionary<long, int>();

            ICollectorStore _store = new CollectorStoreImple(ConfigManager.Configuration["Configurations:ConnectionString"]);
            var list = _store.GetPlcListAsync().Result;

            var idx = 0;
            foreach(var entity in list)
            {
                Plc item = new Plc();
                item.Id = entity.Id;
                item.CpuType = entity.CpuType;
                item.Name = entity.Name;
                item.Ip = entity.Ip;
                item.Port = entity.Port;
                item.Rack = entity.Rack;
                item.Slot = entity.Slot;
                item.Status = "";

                Plcs.Add(item);
                PlcIndex.Add(item.Id, idx);
                idx += 1;
            }
        }

        #endregion

        public ObservableCollection<Plc> Plcs { private set; get; }

        private Dictionary<Int64, int> PlcIndex { set; get; }

        public void ModifyStatus(Int64 key, bool status)
        {
            if (PlcIndex.ContainsKey(key))
            {
                var plc = Plcs[PlcIndex[key]];
                if (plc != null)
                {
                    plc.Status = status?"已连接":"已断开";
                }
            }
        }
    }

    public class Plc: INotifyPropertyChanged
    {   
        public Int64 Id { set; get; }

        public string CpuType { get; set; }
                
        public string Name { get; set; }
        
        public string Ip { get; set; }
        
        public int Port { get; set; }
        
        public int Rack { get; set; }
        
        public int Slot { get; set; }

        private string _status;

        public string Status {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
