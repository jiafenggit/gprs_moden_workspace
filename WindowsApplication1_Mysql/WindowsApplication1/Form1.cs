using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using MySQLDriverCS;
using System.Configuration;

namespace WindowsApplication1
{
    

    public partial class Form1 : Form
    {
        
       
        bool continueFlag = true;
        long startTime;

         public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            int numOfModen = int.Parse(ConfigurationSettings.AppSettings["NumOfModen"]);
            for (int i = 1; i <= numOfModen; i++)
            {
                string portName = "COM" + i;
                DataGridViewRow d = new DataGridViewRow();
                d.CreateCells(portNameDataGridView, portName,"free");
                portNameDataGridView.Rows.Insert(0, d);
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            continueFlag = true;

            logBox.Text = "";


            if (portNameDataGridView.Rows.Count-1 <1)
            {
                log("未添加设备,请先添加设备端口;");
            }
            else
            {
                try
                {
                    
                   
                    enabledControls(false);
                    startTime = Environment.TickCount;

                    ThreadPool.QueueUserWorkItem(this.mainLoop);


                }

                catch (Exception ex)
                {
                    log(ex.Message);
                    enabledControls(true);
                }
                

            }
            
        }


        public void mainLoop(Object threadContext)
        {
           
            RequestBean req;
            try
            {
                while (continueFlag)
                {

                    if ((req = getNextRequest()) != null)
                    {

                        RingThread th = new RingThread(req, this);
                        ThreadPool.QueueUserWorkItem(th.doRing);

                    }
                    else
                    {
                        Thread.Sleep(1 * 1000);
                    }
                }

                log("已停止...");
                enabledControls(true);
            }
            catch (Exception e)
            {
                log(e.Message);
            }
        }

        
       

        public void finishTestHandler(RequestBean req, string ringState, bool hasError)
        {
           

            Monitor.Enter(this);
            
            try
            {
                log(req.portName+"完成测试:" + req.mobile + "  状态:" + ringState);
                

                if (hasError)
                {
                    reRing(req);
                    log(req.mobile + "重回拨测队列。");
                    errorModenCount.Text = int.Parse(errorModenCount.Text) + 1 + "";
                }
                else
                {

                    saveResult(req, ringState);
                    deleteRequest(req);
                    log(req.mobile + "保存结果，从拨测队列删除。");
                    portNameDataGridView.Rows[req.modenId].Cells[1].Value = "free"; 
                    
                }



                //多余的显示
                DataGridViewRow d = new DataGridViewRow();
                d.CreateCells(ringStatusGridView, req.mobile, ringState,Environment.TickCount);
                ringStatusGridView.Rows.Insert(0, d);
                if (ringStatusGridView.Rows.Count - 1 > 500)
                {
                    ringStatusGridView.Rows.RemoveAt(500);
                }

                if (ringStatusGridView.Rows.Count > 2)
                {

                    Console.WriteLine(ringStatusGridView.Rows[ringStatusGridView.Rows.Count - 2].Cells[2].Value);
                    long time = Environment.TickCount - long.Parse(ringStatusGridView.Rows[ringStatusGridView.Rows.Count - 2].Cells[2].Value.ToString());
                    double speed = (ringStatusGridView.Rows.Count - 1) / (time / 1000.00 / 60.00);
                    speedLabel.Text = speed.ToString("0.00") + "条/分";
                }
            }
            catch (Exception e)
            {
                log(e.Message);
            }
            

            Monitor.Exit(this);

        }

        private void reRing(RequestBean req)
        {
            MySQLConnection conn = getConn();
            string sqlstr2 = "update ring_request set doing=0 where id=" + req.mobileId;
            MySQLCommand comm2 = new MySQLCommand(sqlstr2, conn);
            comm2.ExecuteNonQuery();

        }

        private void deleteRequest(RequestBean req)
        {
            MySQLConnection conn = getConn();

            string sqlstr = "delete from ring_request where id="+req.mobileId;
            MySQLCommand comm = new MySQLCommand(sqlstr, conn);
            comm.ExecuteNonQuery();

        }

       
        private void saveResult(RequestBean req, string ringState)
        {
            MySQLConnection conn = getConn();

            string sqlstr = "insert into ring_state set "
                            +"batch_id="+req.batchId
                            +",mobile='"+req.mobile
                            +"',status='"+ringState
                            +"',moden_port='"+req.portName
                            +"',time=now()";
            MySQLCommand comm = new MySQLCommand(sqlstr, conn);
            comm.ExecuteNonQuery();
            
        }


        RequestBean getNextRequest()
        {
            Monitor.Enter(this);

            RequestBean bean =null;

            int modenId = -1;
            string portName = null;
            for (int i = 0; i < portNameDataGridView.RowCount - 1; i++)
            {
                if ("free" == (string)portNameDataGridView.Rows[i].Cells[1].Value)
                {
                   
                    portName = (string)portNameDataGridView.Rows[i].Cells[0].Value;
                    modenId = i;
                    break;
                }
            }

            if (modenId!=-1 && portName!=null) 
            {
                MySQLConnection conn = getConn();
                
                string sqlstr = "select * from ring_request where doing=0 order by priority desc,time limit 1";
                MySQLCommand comm = new MySQLCommand(sqlstr, conn);

                MySQLDataReader dbReader = comm.ExecuteReaderEx();
                if (dbReader.Read())
                {
                    portNameDataGridView.Rows[modenId].Cells[1].Value = "准备中.....";

                    string sqlstr2 = "update ring_request set doing=1 where id=" + dbReader.GetInt32(0);
                    MySQLCommand comm2 = new MySQLCommand(sqlstr2, conn);
                    comm2.ExecuteNonQuery();

                    bean = new RequestBean();
                    bean.modenId = modenId;
                    bean.portName = portName;
                    bean.batchId = dbReader.GetInt32(1);
                    bean.mobileId = dbReader.GetInt32(0);
                    bean.mobile = dbReader.GetString(2);

                }
                dbReader.Close();

                
               
            }
           

            Monitor.Exit(this);
            return bean;
        }

        
        MySQLConnection conn;
        MySQLConnection getConn()
        {
            Monitor.Enter(this);

            if (conn != null && conn.State == ConnectionState.Open)
            {
            }
            else
            {
                log("创建数据库链接");
                conn = new MySQLConnection(new MySQLConnectionString("203.81.26.23", "ringring", "root", "cellstar", 3306).AsString);              
                conn.Open();
            }

            Monitor.Exit(this);
            return conn;

        }



        public void log(string msg)
        {
            Monitor.Enter(this);
            int len = logBox.Text.Length;
            if (len > 5000) len = 5000;
            logBox.Text = "[" + DateTime.Now.ToString("T") + "]:" + msg + "\r\n" + logBox.Text.Substring(0, len);
            Monitor.Exit(this);
        }




        private void enabledControls(bool enabled)
        {

            button1.Enabled = enabled;
            portNameCheckBox.Enabled = enabled;
            button3.Enabled = enabled;
            button4.Enabled = !enabled;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (conn != null && conn.State == ConnectionState.Open)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    log(ex.Message);
                }
            }
           
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            continueFlag = false;
            button4.Enabled = false;
            log("正在停止.....");
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (portNameCheckBox.SelectedItem != null)
            {

                string portName = portNameCheckBox.SelectedItem.ToString();
                bool existed = false;
                for (int i = 0; i < portNameDataGridView.RowCount - 1; i++)
                {
                    if (portName == (string)portNameDataGridView.Rows[i].Cells[0].Value)
                    {
                        existed = true;
                        break;
                    }
                }
                if (!existed && portName != "")
                {
                    DataGridViewRow d = new DataGridViewRow();
                    d.CreateCells(portNameDataGridView, portName, "free");
                    portNameDataGridView.Rows.Insert(0, d);
                }
            }
        }

       

        private void portNameDataGridView_RowsAdded_1(object sender, DataGridViewRowsAddedEventArgs e)
        {
            modenCount.Text = portNameDataGridView.Rows.Count - 1 + "";
        }

       

    }
}