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
                log("��ѯ�豸״̬...");
                if (md.getModenState() != 0)
                {
                    log("�豸æ���Ҷ�");
                    md.hangsUp();
                }
                log("���У�����...");
                md.reset();
                log("������ϣ����sim��...");
                md.checkSimCard();
                log("sim��������������Ϣ�����ʽ...");
                md.setSmsReachInfoFormat("1,1,2");
                log("������Ϣ�����ʽ��ɣ����ö���ģʽ");
                md.setSmsMode(0);
                log("���ö���ΪPDUģʽ��ɡ�");

                


                log("��ȡ������Ϣ...");
                getAllArrivedSms();

                log("�ȴ����ŵ�����...");
                while (running)
                {
                    try
                    {
                        String line = md.readLien();
                        log("RECV:" + line);
                        if (line.StartsWith("+CMTI:"))
                        {

                            int smsId = int.Parse(line.Replace("+CMTI: \"SM\",", ""));
                            log("�յ�����[" + smsId + "]����ȡ��...");

                            PDUData data = md.getSms(smsId);
                            if (data != null)
                            {
                                receiveSms(data);
                            }

                            md.deleteSms(smsId);

                            log("����[" + smsId + "]�ѱ��棬ɾ��...");
                        }


                        Thread.Sleep(500);
                    }
                    catch (TimeoutException ex)
                    {
                        log("�ȴ����ų�ʱ����ȡ������Ϣ...");
                        getAllArrivedSms();
                    }
                }


                log("����ֹͣ��");
                form.changeThreadState("����ֹͣ��");


            }
            catch (Exception ex)
            {

                log("�˿�" + md.PortName + "�쳣��" + ex.Message);
                form.changeThreadState("�쳣�˳�������");
            }
            finally
            {
                if (md.IsOpen)
                {
                    md.Close();
                }

                log("�˿�" + md.PortName + "�رգ��˳�...");
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
                log("��ȡ��" + pduDatas.Count + "������");
                md.deleteAllSms();
                log("ɾ��ȫ���������.");
            }
            else
            {
                log("��������Ϣ.");
            }
        }



        private void receiveSms(PDUData pduData)
        {
            
            log("RECV:[" + pduData.ToString() + "]");
            log("PDUDATA:[" + pduData.getPUDType() + "],[" + pduData.getOA() + "],[" + pduData.getDcs()+ "],[" + pduData.getMsg() + "],[" + pduData.getTimeStamp() + "]");
            string saveRs = saveSms(pduData);
            log("���" + saveRs);

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
                log("�������ݿ�����");
                conn = new MySQLConnection(new MySQLConnectionString("203.81.26.54", "gate_999", "root", "cellstar", 3306).AsString);
                conn.Open();
            }

            Monitor.Exit(this);
            return conn;

        }

    }
}
