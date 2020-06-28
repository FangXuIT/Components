using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Terminal.Collector.IStore;
using Terminal.Collector.Store;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace Terminal.Collector.HMI.Core
{
    public class TargetHelper
    {
        #region --单例--
        public static TargetHelper Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            internal static readonly TargetHelper instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new TargetHelper();
            }
        }

        private TargetHelper()
        {
            Targets = new ObservableCollection<Target>();
            TargetIndex = new Dictionary<string, int>();

            ICollectorStore _store = new CollectorStoreImple(ConfigManager.Configuration["Configurations:ConnectionString"]);

            var list = _store.GetTargetListAsync().Result;
            int idx = 0;
            foreach(var item in list)
            {
                Target tag = new Target();
                tag.PlcID = item.PlcId;
                tag.Address = item.Address;
                tag.TagName = item.Name;
                tag.VarType = GetVarType(item.VarType);
                tag.Value = "";
                tag.ReadTime = "";

                Targets.Add(tag);
                TargetIndex.Add(item.Address, idx);

                idx += 1;
            }
        }

        #endregion

        public ObservableCollection<Target> Targets { private set; get; }

        private Dictionary<string,int> TargetIndex { set; get; }

        public void ModifyValue(string key,string value)
        {
            if(TargetIndex.ContainsKey(key))
            {
                var target = Targets[TargetIndex[key]];
                if (target != null)
                {
                    target.Value = value;
                    target.ReadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        private string GetVarType(int type)
        {
            string result = "Int";
            switch (type)
            {
                case 0:
                    result = "Bit";
                    break;
                case 1:
                    result = "Byte";
                    break;
                case 2:
                    result = "Word";
                    break;
                case 3:
                    result = "DWord";
                    break;
                case 4:
                    result = "Int";
                    break;
                case 5:
                    result = "DInt";
                    break;
                case 6:
                    result = "Real";
                    break;
                case 7:
                    result = "String";
                    break;
                case 8:
                    result = "StringEx";
                    break;
                case 9:
                    result = "Timer";
                    break;
                case 10:
                    result = "Counter";
                    break;
                case 11:
                    result = "DateTime";
                    break;
            }
            return result;
        }
    }

    public class Target: INotifyPropertyChanged
    {
        public Int64 PlcID { get; set; }

        public string Address { get; set; }
  
        public string TagName { get; set; }

        public string VarType { get; set; }

        private string _value { set; get; }
        public string Value 
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        private string _readTime { set; get; }

        public string ReadTime 
        {
            get { return _readTime; }
            set { _readTime = value; OnPropertyChanged("ReadTime"); }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
