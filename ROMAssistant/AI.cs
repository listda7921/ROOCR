using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    class AI
    {
        String windowTitle;
        public Interface Interface;
        public Action Action;
        public int hWnd;
        public int width = 1280;
        public int height = 720;
        public Settings Settings;
        public Log Log;

        public AI(Log Log)
        {
            this.Log = Log;
            Log.Info("Loading config.json...");
            try
            {
                this.Settings = Configurator.LoadConfig("config.json");
                this.windowTitle = this.Settings.windowTitle;
                this.Interface = new Interface(windowTitle);

                this.Action = new Action(this);
                Log.Info("Detecing Ragnarok Mobile Client...");
                if (hWnd <= 0) this.hWnd = FindNOX(windowTitle);
                if (hWnd <= 0) this.hWnd = FindMEMU(windowTitle);
                if (this.hWnd > 0)
                {
                    Log.Success("Found ROM Client [" + hWnd + "]");
                    Bitmap Screen = ImageProcessing.PrintWindow((IntPtr)hWnd);
                    Log.Info(StringProcessing.OCR(Screen));
                }
                else
                {
                    Log.Error("Cannot find ROM Client / Emulator");
                }
            }
            catch(Exception e) {
                Log.Error(e.ToString());
            }
        }
        private int FindMEMU(String windowTitle)
        {
            Log.Info("Finding MEMU Emulator...");
            int MEMU = Win32.FindWindow(null, windowTitle);
            int MainWindow = Win32.FindWindowEx((IntPtr)(MEMU), new IntPtr(0), "Qt5QWindowIcon", "MainWindowWindow");
            int CenterWidgetWindow = Win32.FindWindowEx((IntPtr)(MainWindow), new IntPtr(0), "Qt5QWindowIcon", "CenterWidgetWindow");
            int RenderWindowWindow = Win32.FindWindowEx((IntPtr)(CenterWidgetWindow), new IntPtr(0), "Qt5QWindowIcon", "RenderWindowWindow");
            if(RenderWindowWindow > 0)
            {
                Log.Success("Detected Emulator: MEMU");
            }
            return RenderWindowWindow;
        }

        private int FindNOX(String windowTitle)
        {
            Log.Info("Finding NOX Emulator...");
            int NOX = Win32.FindWindow(null, windowTitle);
            int MainWindow = Win32.FindWindowEx((IntPtr)(NOX), new IntPtr(0), "Qt5QWindowIcon", "ScreenBoardClassWindow");
            if (MainWindow > 0)
            {
                Log.Success("Detected Emulator: NOX");
            }
            return MainWindow;
        }

        public bool ClickImage(String fileName, int retries = 5)
        {
            for (int x = 0; x < retries; x++) {
                Point point = ImageSearch.Search(this.windowTitle, fileName);
                System.Diagnostics.Debug.WriteLine(fileName+": " +point.X +","+ point.Y);
                if (point.X > -1 && point.Y > -1) {
                    Win32.Click(this.hWnd, point.X+24, point.Y+24);
                    return true;
                }
            }
            return false;
        }
        public void SaveConfig()
        {
            Configurator.SaveConfig("config.json", Settings);
        }
    }
    class Interface
    {
        public Point BtnBag, BtnMore, BtnParty;
        public Point BtnMVP, BtnSettings;
        public Interface(String windowTitle)
        {
            this.BtnBag = ImageSearch.Search(windowTitle, "resources/bag.png");
            this.BtnMore = ImageSearch.Search(windowTitle, "resources/more.png");
            this.BtnParty = ImageSearch.Search(windowTitle, "resources/party.png");
        }
    }
    class Action
    {
        AI ai;
        public Action(AI ai)
        {
            this.ai = ai;
        }
        public async Task<bool> OpenMVP()
        {
            this.ai.ClickImage("resources/more.png");
            await Task.Delay(1000);
            //this.ai.ClickImage("resources/mvp.png");
            Win32.Click(this.ai.hWnd, (int)(0.69 * this.ai.width), (int)(0.45 * this.ai.height));
            return false;
        }
    }
}
