
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsApplication1
{
    class RingThread
    {
        RequestBean bean;
        Form1 form1;

        public RingThread(RequestBean bean,Form1 form1)
        {
            this.bean = bean;
            this.form1 = form1;

          
        }

        public void doRing(Object threadContext)
        {

           
            string ringState="";
            bool hasError = false;
            moden md = new moden();

            try
            {

               
                md.PortName =bean.portName;
                md.init();
                md.openCmdEcho();

                //AT+CPAS
                log("查询设备状态...");
                if (md.getModenState() != 0)
                {
                    log("设备忙，挂断");
                    md.hangsUp();
                }
                md.reset();
                md.checkSimCard();

                if (Regex.IsMatch(bean.mobile, "\\d{11}"))
                {
                   
                    log("拨号:" + bean.mobile);
                    md.ring(bean.mobile);

                    log("等待结果:" + bean.mobile);
                    ringState = md.getRingStat();
                }
                else
                {
                    ringState = "手机号码格式不正确";
                }

                log("完成!");

                

            }
            catch (Exception ex)
            {
                log(ex.Message);
                ringState += ex.Message;
                hasError = true;
            }
            finally
            {

                if (md != null && md.IsOpen)
                {
                    try
                    {
                        md.Close();
                    }
                    catch (Exception ex)
                    {
                        log(ex.Message);
                    }
                }

            }

            form1.finishTestHandler(bean, ringState, hasError);
            


        }

        

        private void log(string msg)
        {
            form1.log(this.bean.portName + ":" + msg);
            form1.portNameDataGridView.Rows[this.bean.modenId].Cells[1].Value = msg;
        }
        

    }
}
