using Newtonsoft.Json;
using ROMAssistant.Monsters;
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
        bool firstRun = true;
        Bot bot = new Bot();
        
        public MainForm()
        {
            InitializeComponent();
            this.Log = new Log(this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                var v2 = true;
                if (v2)
                {
                    try
                    {
                        bot.RunBot(Log);
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"stopped: {ex}");
                    }
                }
                else
                {
                    this.Log.Info("Initializing ROM Assistant. Please wait...");
                    if(firstRun == true) {
                        this.AI = new AI(this.Log);
                        firstRun = false;
                    }
                    if (this.AI.hWnd > 0)
                    {
                        this.isRunning = true;
                        AI.isIdle = false;
                        //AI.huntMinis();
                        Task hunting = AI.ScanMini();
                        btnStart.Text = "Stop Bot";
                        Log.Info("ROM Assistant now hunting...");
                    }
                    else
                    {
                        this.Log.Error("Cannot find ROM Client / Emulator");
                    }
                }

            } else
            {
                Stop();
            }
        }

        public void Stop()
        {
            Log.Success("Bot succesfully stopped!");
            this.isRunning = false;
            AI.isHunting = false;
            AI.isIdle = true;
            btnStart.Text = "Start Bot";
        }

        public void Start() { 
        
        }

        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                await AI.Action.OpenMVP();
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
