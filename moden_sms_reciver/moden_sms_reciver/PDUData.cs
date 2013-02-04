using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace moden_sms_reciver
{
    public class PDUData
    {
        string sca;
        string pduType;
        string mr;
        string oa;
        string da;
        string pid;
        string dcs;
        string scts;
        string vp;
        string udl;
        string ud;

        string pduStr;

        public static PDUData parseSmsDeliverPUD(string pduStr)
        {


            PDUData data = new PDUData();
            data.pduStr = pduStr;
            try
            {
                string[] charsArray = toCharsArray(pduStr);

                int index = 0;

                int scaLen = Convert.ToInt32(charsArray[index], 16);
                data.sca = charsArray[index] + string.Join("", charsArray, index + 1, scaLen);
                index += scaLen + 1;
                if (index > charsArray.Length) return data;

                data.pduType = charsArray[index];
                index += 1;

                int oaLen = Convert.ToInt32(charsArray[index], 16);
                if (oaLen % 2 == 1)
                {
                    oaLen = (oaLen + 1) / 2 + 1;
                }
                else
                {
                    oaLen = oaLen / 2 + 1;
                }
                data.oa = charsArray[index] + string.Join("", charsArray, index + 1, oaLen);
                index += oaLen + 1;

                data.pid = charsArray[index];
                index += 1;

                data.dcs = charsArray[index];
                index += 1;

                data.scts = string.Join("", charsArray, index, 7);
                index += 7;

                data.udl = charsArray[index];
                index += 1;

                int udLen = Convert.ToInt32(data.udl, 16);
                data.ud = string.Join("", charsArray, index, udLen);
                index += udLen;


                return data;
            }
            catch (Exception ex)
            {
                return data;
            }


        }

        public static string[] toCharsArray(string pduStr)
        {
            string[] rs = new string[pduStr.Length / 2];
            for (int i = 0; i < pduStr.Length/2; i ++)
            {
                rs[i] = pduStr.Substring(i*2, 2);
            }

            return rs;
        }


        public override string ToString()
        {
            return this.sca + "\t"
                    + this.pduType + "\t"
                    + this.mr + "\t"
                    + this.oa + "\t"
                    + this.da + "\t"
                    + this.pid + "\t"
                    + this.dcs + "\t"
                    + this.scts + "\t"
                    + this.vp + "\t"
                    + this.udl + "\t"
                    + this.ud + "\t";

        }

        public string getSmsCenterAddr()
        {
            if (this.sca == null) return null;

            string rs = "";
            string tmp = this.sca.Substring(4);
            char[] charList = tmp.ToCharArray();
            for (int i = 0; i < charList.Length; i += 2)
            {
                rs += charList[i + 1];
                rs += charList[i];
            }
            rs=rs.Replace("F", "");
            return rs;
        }

        public string getOA()
        {
            if (this.oa == null) return null;

            string rs = "";
            string tmp = this.oa.Substring(4);
            char[] charList = tmp.ToCharArray();
            for (int i = 0; i < charList.Length; i += 2)
            {
                rs += charList[i + 1];
                rs += charList[i];
            }
            rs = rs.Replace("F", "");
            return rs;
        }

        public string getMsg()
        {
            if (this.ud == null) return null;

            if (this.dcs.Equals("00"))
            {
                return pduTo7BitAscii(this.ud);
            }
            else if (this.dcs.Equals("08"))
            {
                return pdu2Unicode(this.ud);
            }
            else if (this.dcs.Equals("04"))
            {
                return "MMSDATA";
            }
            else
            {
                return pduTo8BitAscii(this.ud);
            }
        }

        public string getTimeStamp()
        {
            if (this.scts == null) return null;

            string rs = "";
            string tmp = this.scts;
            char[] charList = tmp.ToCharArray();
            for (int i = 0; i < charList.Length; i += 2)
            {
                rs += charList[i + 1];
                rs += charList[i];
            }
            
            return rs;
        }

        public string getPDUStr()
        {
            return this.pduStr;
        }

        public string getPUDType()
        {
            return this.pduType;
        }

        public string getDcs()
        {
            return this.dcs;
        }

        private string pdu2Unicode(string pduStr)
        {
            string rs = "";

            for (int i = 0; i < pduStr.Length; i+=4)
            {
                rs += (char)short.Parse(pduStr.Substring(i, 4), NumberStyles.HexNumber);
            }

            return rs;
        }

        private string pduTo8BitAscii(string pduStr)
        {
            string rs = "";

            for (int i = 0; i < pduStr.Length; i += 2)
            {
                rs += (char)short.Parse(pduStr.Substring(i, 2), NumberStyles.HexNumber);
            }

            return rs;
        }

        private string pduTo7BitAscii(string pduStr)
        {
            string rs = "";
            string binStr = "";

            for (int i = 0; i < pduStr.Length; i += 2)
            {
                binStr = Convert.ToString(Convert.ToInt32(pduStr.Substring(i, 2), 16), 2).PadLeft(8, '0')+binStr;
           
            }

            for (int i = 0; i < binStr.Length/7; i++)
            {
                rs  += Convert.ToChar(Convert.ToInt16("0" + binStr.Substring(binStr.Length - (i + 1) * 7, 7), 2));
            }

            return rs;
        }




    }
}
