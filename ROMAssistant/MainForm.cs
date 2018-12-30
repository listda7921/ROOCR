using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROMAssistant
{
    public partial class MainForm : Form
    {
        Log Log;
        AI AI;
        bool isRunning;
        public MainForm()
        {
            InitializeComponent();
            this.Log = new Log(this);
            this.AI = new AI(this.Log);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                if (this.AI.hWnd > 0)
                {
                    this.isRunning = true;
                    btnStart.Text = "Stop Bot";
                }
                else
                {
                    this.Log.Error("Cannot find ROM Client / Emulator");
                }
            } else
            {
                this.isRunning = false;
                btnStart.Text = "Start Bot";
            }
        }

        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                bool x = await AI.Action.OpenMVP();
            }
            finally { }
        }

        public void AppendText(string text, Color color)
        {
            txtLogs.SelectionStart = txtLogs.TextLength;
            txtLogs.SelectionLength = 0;

            txtLogs.SelectionColor = color;
            txtLogs.AppendText(text);
            txtLogs.SelectionColor = txtLogs.ForeColor;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AI.SaveConfig();
        }
    }
}
