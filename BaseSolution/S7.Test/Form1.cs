using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S7.Test
{
    public partial class Form1 : Form
    {
        private Plc plc;

        public Form1()
        {
            InitializeComponent();
        }

        private CpuType GetPlcType()
        {
            var result = CpuType.S71500;
            switch (cbPlcType.SelectedItem.ToString())
            {
                case "S7-1500":
                    result = CpuType.S71500;
                    break;
                case "S7-1200":
                    result = CpuType.S71200;
                    break;
                case "S7-200":
                    result = CpuType.S7200;
                    break;
                case "S7-300":
                    result = CpuType.S7300;
                    break;
                case "S7-400":
                    result = CpuType.S7400;
                    break;

            }
            return result;
        }

        private DataType GetDataType()
        {
            var result = DataType.DataBlock;

            switch(cbDataType.SelectedItem.ToString())
            {
                case "Counter":
                    result = DataType.Counter;
                    break;
                case "DataBlock":
                    result = DataType.DataBlock;
                    break;
                case "Input":
                    result = DataType.Input;
                    break;
                case "Memory":
                    result = DataType.Memory;
                    break;
                case "Output":
                    result = DataType.Output;
                    break;
                case "Timer":
                    result = DataType.Timer;
                    break;
            }

            return result;
        }

        private VarType GetVarType()
        {
            var result = VarType.DInt;

            switch(cbVarType.SelectedItem.ToString())
            {
                case "Bit":
                    result = VarType.Bit;
                    break;
                case "Byte":
                    result = VarType.Byte;
                    break;
                case "Counter":
                    result = VarType.Counter;
                    break;
                case "DateTime":
                    result = VarType.DateTime;
                    break;
                case "DInt":
                    result = VarType.DInt;
                    break;
                case "DWord":
                    result = VarType.DWord;
                    break;
                case "Int":
                    result = VarType.Int;
                    break;
                case "Real":
                    result = VarType.Real;
                    break;
                case "String":
                    result = VarType.String;
                    break;
                case "StringEx":
                    result = VarType.StringEx;
                    break;
                case "Timer":
                    result = VarType.Timer;
                    break;
                case "Word":
                    result = VarType.Word;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                lbVarValue.Items.Clear();
                plc = new Plc(GetPlcType(), txtIP.Text.Trim(),102, Convert.ToInt16(nudRack.Value), Convert.ToInt16(nudSlot.Value));
                plc.Open();

                if (!plc.IsConnected)
                {
                    button1.Enabled = true;
                    this.label6.Text = "未连接";
                }
                else
                {
                    cbDataType.Enabled = true;
                    cbVarType.Enabled = true;
                    nudDB.Enabled = true;
                    nudVarLength.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    this.label6.Text = "已连接";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                button1.Enabled = true;
            }
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                lbVarValue.Items.Clear();
                //plc.Close();
                this.label6.Text = "未连接";

                button1.Enabled = true;
                button2.Enabled = false;

                cbDataType.Enabled = false;
                cbVarType.Enabled = false;
                nudDB.Enabled = false;
                nudVarLength.Enabled = false;
                button3.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (plc.IsConnected)
                {
                    var result = plc.Read(GetDataType(), Convert.ToInt32(nudDB.Value), 45, GetVarType(), Convert.ToInt32(nudVarLength.Value));
                    lbVarValue.Items.Add("test result:" + result.ToString());
                }
                else
                {
                    if (!plc.IsConnected)
                    {
                        MessageBox.Show("PLC未建立连接");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
