using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO;
using MySQLDriverCS;
using System.Data;

namespace moden_sms_reciver
{
    class MainThread 
    {
        Moden md;
        Form1 form;
        string phoneNum;

        bool running = true;

        public MainThread(string phoneNum,Moden md, Form1 form)
        {
            this.phoneNum = phoneNum;
            this.md = md;
            this.form = form;
        }

        public void stop()
        {
            this.running = false;
        }

        public void run(Object threadContext)
        {

            try
            {

                md.open();
                md.openCmdEcho();

                //AT+CPAS
                log("查询设备状态...");
                if (md.getModenState() != 0)
                {
                    log("设备忙，挂断");
                    md.hangsUp();
                }
                log("空闲，重置...");
                md.reset();
                log("重置完毕，检查sim卡...");
                md.checkSimCard();
                log("sim卡正常，设置消息到达格式...");
                md.setSmsReachInfoFormat("1,1,2");
                log("设置消息到达格式完成，设置短信模式");
                md.setSmsMode(0);
                log("设置短信为PDU模式完成。");

                


                log("获取已有信息...");
                getAllArrivedSms();

                log("等待短信到达中...");
                while (running)
                {
                    try
                    {
                        String line = md.readLien();
                        log("RECV:" + line);
                        if (line.StartsWith("+CMTI:"))
                        {

                            int smsId = int.Parse(line.Replace("+CMTI: \"SM\",", ""));
                            log("收到短信[" + smsId + "]，获取中...");

                            PDUData data = md.getSms(smsId);
                            if (data != null)
                            {
                                receiveSms(data);
                            }

                            md.deleteSms(smsId);

                            log("短信[" + smsId + "]已保存，删除...");
                        }


                        Thread.Sleep(500);
                    }
                    catch (TimeoutException ex)
                    {
                        log("等待短信超时，获取已有信息...");
                        getAllArrivedSms();
                    }
                }


                log("正常停止！");
                form.changeThreadState("正常停止！");


            }
            catch (Exception ex)
            {

                log("端口" + md.PortName + "异常：" + ex.Message);
                form.changeThreadState("异常退出！！！");
            }
            finally
            {
                if (md.IsOpen)
                {
                    md.Close();
                }

                log("端口" + md.PortName + "关闭，退出...");
            }
            
        }

        private void getAllArrivedSms()
        {
            
            ArrayList pduDatas = md.getAllSms();
            foreach (PDUData data in pduDatas)
            {
                if (data != null)
                {
                    receiveSms(data);
                }
            }
            if (pduDatas.Count > 0)
            {
                log("获取到" + pduDatas.Count + "条短信");
                md.deleteAllSms();
                log("删除全部短信完成.");
            }
            else
            {
                log("无已有信息.");
            }
        }



        private void receiveSms(PDUData pduData)
        {
            
            log("RECV:[" + pduData.ToString() + "]");
            log("PDUDATA:[" + pduData.getPUDType() + "],[" + pduData.getOA() + "],[" + pduData.getDcs()+ "],[" + pduData.getMsg() + "],[" + pduData.getTimeStamp() + "]");
            string saveRs = saveSms(pduData);
            log("入表：" + saveRs);

            form.receiveSms(phoneNum, pduData);
        }

        private void log(String msg)
        {
            msg = "[" + DateTime.Now.ToString("T") + "]:" + msg;
            writelog(msg);
            form.log(msg);
        }

        private void writelog(string msg)
        {
            
            StreamWriter sw =null;
            DirectoryInfo d = new DirectoryInfo(".\\logs");
            if (!d.Exists)
            {
                d.Create();
            }
            try
            {
                sw = File.AppendText(".\\logs\\" + md.PortName + "_" + DateTime.Now.ToString("D") + ".txt");
                sw.WriteLine(msg);
                sw.Flush();

            }
            finally
            {
                if(sw!=null) sw.Close();
            }
            

        }

        private string saveSms(PDUData pduData)
        {
            string sqlstr = "insert into mo_999 (dest,src,cmd,time) values (" +
                                "'" + this.phoneNum + "'"
                                + ",'" + pduData.getOA() + "'"
                                + ",'" + pduData.getMsg() + "'"
                                + ",now())";
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaa"+sqlstr);   
        
            MySQLConnection conn = getConn();
            MySQLCommand comm = new MySQLCommand(sqlstr, conn);
            comm.ExecuteNonQuery();

            return sqlstr;

        }


        MySQLConnection conn;
        private MySQLConnection getConn()
        {
            Monitor.Enter(this);

            if (conn != null && conn.State == ConnectionState.Open)
            {
            }
            else
            {
                log("创建数据库链接");
                conn = new MySQLConnection(new MySQLConnectionString("203.81.26.54", "gate_999", "root", "cellstar", 3306).AsString);
                conn.Open();
            }

            Monitor.Exit(this);
            return conn;

        }

    }
}
