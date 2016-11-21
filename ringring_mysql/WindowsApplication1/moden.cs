using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Configuration;

namespace WindowsApplication1
{
    public class moden:System.IO.Ports.SerialPort
    {
        string ringState = "";
		bool ringThreadFinish = false;
        private const string endFlag = "\r\n";

        public moden()
            : base()
        {
            this.BaudRate = int.Parse(ConfigurationSettings.AppSettings["BaudRate"]);
            this.DtrEnable = true;
            this.ReadBufferSize = 1024;
            this.ReadTimeout = int.Parse(ConfigurationSettings.AppSettings["ReadTimeout"]);
            this.RtsEnable = true;
        }

        public void init()
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
                throw new Exception("设备连接异常:"+ex.Message);
            }
        }

        public bool checkSimCard()
        {
            this.Write("AT+CIMI"+endFlag);
            this.ReadTo("AT+CIMI" + endFlag);
            this.ReadTo(endFlag);

            string tmp=this.ReadTo(endFlag);

            if (tmp.Contains("ERROR")||!Regex.IsMatch(tmp, "4600\\d"))
            {
                throw new Exception("SIM卡连接失败");
            }
            this.ReadTo(endFlag);
            return this.ReadTo(endFlag).Contains("OK");
            
        }

        public int getModenState()
        {
            try
            {
                this.Write("AT+CPAS" + endFlag);

                this.ReadTo("AT+CPAS" + endFlag);
                this.ReadTo(endFlag);
                string stateTmp = this.ReadTo(endFlag).Replace(endFlag,"");


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
            this.Write("ATD"+mobile+";" + endFlag);
            return true;
        }

        public string getRingStat()
        {
            string stateTmp = "";
            ringState = "";
			Thread statusReadThread = new Thread (this.readThread);
			statusReadThread.Start ();
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
			this.ringThreadFinish = true;
			while (!statusReadThread.IsAlive);
            hangsUp();
            return stateTmp;
        }

		private void readThread(Object threadContext)
		{
			while (false == this.ringThreadFinish){
				readStatus ();
			}
		}

		private void readStatus(){
			try{
				this.ReadTimeout = 100;
				string line=this.ReadTo(endFlag);
				System.Console.WriteLine (line);
				if(line.StartsWith("+CLCC"))
				{
					string[] st = line.Replace("+CLCC: ", "").Split(new char[] { ',' });
					if (st[2] == "3" || st[2] == "0")
					{
						ringState = st[2];
					}
				}else if(line.StartsWith("NO ANSWER")||line.StartsWith("BUSY")||line.StartsWith("NO CARRIER")||line.StartsWith("ERROR"))
				{
					ringState=line;
				}
				else if (line.StartsWith("+CEER"))
				{
					ringState = line.Replace("+CEER: ", "");
				}
			}catch(Exception e){
				System.Console.WriteLine (e.Message);
			}
		}


    }
}
