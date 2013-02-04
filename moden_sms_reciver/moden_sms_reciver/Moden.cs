using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;

namespace moden_sms_reciver
{
    public class Moden : System.IO.Ports.SerialPort
    {
        string ringState = "";
        private const string endFlag = "\r\n";



        public Moden()
            : base()
        {
            
            this.DtrEnable = true;
            this.ReadBufferSize = 1024;
            this.RtsEnable = true;
        }

        public void open()
        {
            
            this.Open();
            this.DiscardInBuffer();
            this.DiscardOutBuffer();
        }

        public bool reset()
        {
            this.Write("AT&F0" + endFlag);
            this.ReadTo("AT&F0" + endFlag);
            this.ReadTo(endFlag);


            if (!this.ReadTo(endFlag).Contains("OK"))
            {
                throw new Exception("设备重置失败");
            }
            return true;
        }

        public String readLien()
        {
            return this.ReadTo(endFlag);
        }

        public void setSmsMode(int type)
        {
            this.Write("AT+CMGF=" + type + endFlag);
            this.ReadTo("AT+CMGF=" + type + endFlag);
            this.ReadTo("OK" + endFlag);
        }

        public int getSmsMode()
        {
            this.Write("AT+CMGF?" + endFlag);
            string tmp;
            while (!(tmp = this.ReadTo(endFlag)).StartsWith("+CMGF:")) { };
            this.ReadTo(endFlag+"OK" + endFlag);

            return int.Parse(tmp.Replace("+CMGF: ", ""));
        }

        public void openCmdEcho()
        {
            this.Write("ATE1" + endFlag);
            this.ReadTo("OK" + endFlag);
        }


        

        public bool checkModenConnect()
        {
            try
            {
                this.Write("AT" + endFlag);
                this.ReadTo("OK" + endFlag);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("设备连接异常:" + ex.Message);
            }
        }

        public bool checkSimCard()
        {
            this.Write("AT+CIMI" + endFlag);
            this.ReadTo("AT+CIMI" + endFlag);
            this.ReadTo(endFlag);

            string tmp = this.ReadTo(endFlag);

            if (tmp.Contains("ERROR") || !Regex.IsMatch(tmp, "4600\\d"))
            {
                throw new Exception("SIM卡连接失败");
            }
            this.ReadTo(endFlag);
            return this.ReadTo(endFlag).Contains("OK");

        }

        public void setSmsReachInfoFormat(String format)
        {
            this.Write("AT+CNMI=" +format+ endFlag);
            this.ReadTo("AT+CNMI=" + format + endFlag);
            this.ReadTo("OK" + endFlag);
        }

        public PDUData getSms(int id)
        {
                 
            this.Write("AT+CMGR=" + id + endFlag);
            this.ReadTo("AT+CMGR=" + id + endFlag);
            this.ReadTo(endFlag);

            PDUData data =null;
            string line;

            while (true)
            {
                line = this.ReadTo(endFlag);
                Console.WriteLine(line);

                if (line.StartsWith("OK"))
                {
                    return data;
                }
                else if (line.StartsWith("ERROR"))
                {
                    throw new Exception("获取短信["+id+"]失败");
                }
                else if (line.StartsWith("+CMGR:"))
                {
                    line = this.ReadTo(endFlag);
                    Console.WriteLine(line);
                    data = PDUData.parseSmsDeliverPUD(line);
                    
                }
            }



        } 

        public ArrayList getAllSms()
        {
            
            string p="4";
            if (this.getSmsMode().Equals("1")) p = "\"ALL\"";

            this.Write("AT+CMGL=" + p + endFlag);
            this.ReadTo("AT+CMGL=" + p + endFlag);
            this.ReadTo(endFlag);

            ArrayList smsArray = new ArrayList();
            string line;

            while (true)
            {
                line = this.ReadTo(endFlag);

                if(line.StartsWith("OK"))
                {
                    return smsArray;
                }else if(line.StartsWith("ERROR"))
                {
                    throw new Exception("获取所有短信失败");
                }else if(line.StartsWith("+CMGL:"))
                {
                    line = this.ReadTo(endFlag);
                    Console.WriteLine(line);
                    PDUData data = PDUData.parseSmsDeliverPUD(line);
                    smsArray.Add(data);
                }
            }

            
            
        }

        public void deleteSms(int id)
        {
            int timeoutBak = this.ReadTimeout;
            this.ReadTimeout = 60 * 1000;

            this.Write("AT+CMGD=" + id + endFlag);
            this.ReadTo("OK" + endFlag);

            this.ReadTimeout = timeoutBak;
        }

        public void deleteAllSms()
        {
            int timeoutBak = this.ReadTimeout;
            this.ReadTimeout = 60 * 1000;

            this.Write("AT+CMGD=1,4" + endFlag);
            this.ReadTo("OK" + endFlag);

            this.ReadTimeout = timeoutBak;
        }

        public int getModenState()
        {
            try
            {
                this.Write("AT+CPAS" + endFlag);

                this.ReadTo("AT+CPAS" + endFlag);
                this.ReadTo(endFlag);
                string stateTmp = this.ReadTo(endFlag).Replace(endFlag, "");


                if (stateTmp.Contains("ERROR"))
                {
                    throw new Exception("获取设备状态失败：ERROR");
                }
                this.ReadTo(endFlag);
                this.ReadTo(endFlag);
                return int.Parse(stateTmp.Replace("+CPAS: ", ""));
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException)
                {
                    throw new Exception("设备连接超时:" + ex.Message);
                }
                return -1;
            }

        }

        public bool hangsUp()
        {
            this.Write("ATH" + endFlag);
            this.ReadTo("ATH" + endFlag);
            this.ReadTo(endFlag);

            if (!this.ReadTo(endFlag).Contains("OK"))
            {
                throw new Exception("挂机失败");
            }
            return true;
        }

        public bool ring(string mobile)
        {
            ringState = "";
            this.Write("ATD" + mobile + ";" + endFlag);
            return true;
        }

        public string getRingStat()
        {

            this.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(readEvnetHandler);
            string stateTmp = "";
            ringState = "";
            while (ringState == "")
            {
                this.Write("AT+CLCC" + endFlag);
                Thread.Sleep(500);
            }
            stateTmp = ringState;
            ringState = "";
            if (stateTmp.StartsWith("NO ANSWER") || stateTmp.StartsWith("BUSY") || stateTmp.StartsWith("NO CARRIER") || stateTmp.StartsWith("ERROR"))
            {
                while (ringState == "")
                {
                    this.Write("AT+CEER" + endFlag);
                    Thread.Sleep(500);
                }
                stateTmp += ": " + ringState;
            }


            this.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(readEvnetHandler);
            hangsUp();
            return stateTmp;
        }

        private void readEvnetHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string line = this.ReadTo(endFlag);
            if (line.StartsWith("+CLCC"))
            {
                string[] st = line.Replace("+CLCC: ", "").Split(new char[] { ',' });
                if (st[2] == "3" || st[2] == "0")
                {
                    ringState = st[2];
                }
            }
            else if (line.StartsWith("NO ANSWER") || line.StartsWith("BUSY") || line.StartsWith("NO CARRIER") || line.StartsWith("ERROR"))
            {
                ringState = line;
            }
            else if (line.StartsWith("+CEER"))
            {
                ringState = line.Replace("+CEER: ", "");
            }


        }


        


    }
}

