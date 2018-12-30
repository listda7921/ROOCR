using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROMAssistant
{
    public class Log
    {
        MainForm form;
        public Log(MainForm form)
        {
            this.form = form;
        }

        private void AppendTime()
        {
            this.form.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ", Color.Gray);
        }
        public void Message(String msg, Color color)
        {
            AppendTime();
            this.form.AppendText(msg, color);
        }
        public void Info(String msg, bool newLine = true)
        {
            AppendTime();
            this.form.AppendText("[Info] : ", Color.LightSkyBlue);
            if (newLine == true)
            {
                this.form.AppendText(msg + "\n", Color.White);
            }
            else
            {
                this.form.AppendText(msg, Color.White);
            }
        }
        public void Success(String msg, bool newLine = true)
        {
            AppendTime();
            this.form.AppendText("[Success] : ", Color.LightGreen);
            if (newLine == true)
            {
                this.form.AppendText(msg + "\n", Color.White);
            }
            else
            {
                this.form.AppendText(msg, Color.White);
            }
        }
        public void Error(String msg, bool newLine = true)
        {
            AppendTime();
            this.form.AppendText("[Error] : ", Color.LightPink);
            if (newLine == true)
            {
                this.form.AppendText(msg + "\n", Color.White);
            }
            else
            {
                this.form.AppendText(msg, Color.Black);
            }
        }
    }
}
