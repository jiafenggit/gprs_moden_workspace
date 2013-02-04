using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Configuration;
using Microsoft.Win32;

namespace moden_sms_reciver
{
    public partial class Form1 : Form
    {

        MainThread th;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            MyInitializeComponent();
        }

        private void MyInitializeComponent()
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    portNameComboBox.Items.Add(sValue);
                }
            } 
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (portNameComboBox.SelectedItem == null || BaudRateComboBox.SelectedItem == null || phoneNumTextBox.Text =="")
            {
                MessageBox.Show("请选择端口和波特率，并填入手机卡号！");
                return;
            }

            Moden md = new Moden();
            md.PortName = (string)portNameComboBox.SelectedItem;
            md.BaudRate = int.Parse((string)BaudRateComboBox.SelectedItem);
            md.DtrEnable = true;
            md.ReadBufferSize = 1024;
            md.ReadTimeout = 120 * 1000;
            md.RtsEnable = true;

            th = new MainThread(phoneNumTextBox.Text.Trim(),md, this);
            ThreadPool.QueueUserWorkItem(th.run);
            

            changeThreadState("运行中...");
            

        }

        public void receiveSms(string phoneNum, PDUData pduData)
        {
            DataGridViewRow d = new DataGridViewRow();
            d.CreateCells(dataGridView1, phoneNum, pduData.getOA(), pduData.getTimeStamp(), pduData.getMsg());
            dataGridView1.Rows.Insert(0,d);

            if (dataGridView1.Rows.Count - 1 > 5)
            {
                dataGridView1.Rows.RemoveAt(5);
            }

            
        }

        public void log(string msg)
        {
            Monitor.Enter(this);
            int len = logBox.Text.Length;
            if (len > 5000) len = 5000;
            logBox.Text = msg + "\r\n" + logBox.Text.Substring(0, len);
            Monitor.Exit(this);
        }

       

        public void changeThreadState(String msg)
        {
            this.label3.Text = msg;
        }

       
    }
}