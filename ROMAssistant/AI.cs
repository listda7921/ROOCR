using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ROMAssistant
{
    class AI
    {
        public int width = 1280;
        public int height = 720;

        String windowTitle;
        public Interface Interface;
        public Action Action;
        public int hWnd;
        public Settings Settings;
        public Log Log;
        public int screenHandle;
        private OCR OCR = new OCR();
        private Scheduler Scheduler;
        public List<int> Timer_Mini = new List<int>();
        public AI ai;
        public bool isIdle = true;
        public bool isHunting = false;
        public readonly List<String> MobName_Mini = new List<String>(){
                    "Smokie (South Gate)",
                    "Eclipse (Labyrinth)",
                    "Eclipse (South Gate)",
                    "Mastering (Labyrinth)",
                    "Vocal (West Gate)",
        };
        public AI(Log Log)
        {
            this.Log = Log;
            this.ai = this;
            Log.Info("Loading config.json...");
            try
            {
                this.Settings = Configurator.LoadConfig("config.json");
                this.windowTitle = this.Settings.windowTitle;

                this.Action = new Action(this);
                this.Scheduler = new Scheduler(this);
                Log.Info("Detecing Ragnarok Mobile Client...");
                if (hWnd <= 0) this.hWnd = FindNOX(windowTitle);
                if (hWnd <= 0) this.hWnd = FindMEMU(windowTitle);
                if (this.hWnd > 0)
                {
                    this.Interface = new Interface((IntPtr)screenHandle);
                    Log.Success("Found ROM Client [" + hWnd + "]");
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
            this.screenHandle = MEMU;
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
            this.screenHandle = NOX;
            int MainWindow = Win32.FindWindowEx((IntPtr)(NOX), new IntPtr(0), "Qt5QWindowIcon", "ScreenBoardClassWindow");
            if (MainWindow > 0)
            {
                Log.Success("Detected Emulator: NOX");
            }
            return MainWindow;
        }
        public bool Click(Point point)
        {
            if (point.X > -1 && point.Y > -1)
            {
                Win32.Click(this.hWnd, point.X, point.Y);
                return true;
            }
            return false;
        }
        public bool ClickImage(String fileName, int retries = 5)
        {

            Point point = ImageSearch.SearchFromHandle((IntPtr)screenHandle, fileName);
            System.Diagnostics.Debug.WriteLine(fileName+": " +point.X +","+ point.Y);
            if (point.X > -1 && point.Y > -1) {
                Win32.Click(this.hWnd, point.X, point.Y+30); // NOX Constant
                return true;
            }
            return false;
        }
        public void SaveConfig()
        {
            Configurator.SaveConfig("config.json", Settings);
        }
        public async Task ScanMini(bool autoClose = true)
        {
            while (ai.isIdle == false)
            {
                Log.Info("Scanning for Mini Boss... Please wait...");
                await this.Action.OpenMVP();
                Point MonsterImage;
                await Task.Delay(2000);

                Bitmap bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
                MonsterImage = ImageSearch.SearchFromImage(bmp, "resources/smokie.png");

                Bitmap crop;
                if (MonsterImage.X == -1 && MonsterImage.Y == -1)
                {
                    Log.Error("Cannot find a reference point.");
                }
                for (int i = 0; i < 5; i++)
                {
                    Point TempPoint;
                    bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
                    TempPoint = new Point(MonsterImage.X + 360, MonsterImage.Y + 110 * i);
                    crop = ImageSearch.CropImage(bmp, TempPoint, 400, 100);
                    crop.Save($"mob{i}.bmp");
                    Timer_Mini.Add(OCR.ExtractTime(OCR.RawOCR(crop)));
                    Log.Info($"{MobName_Mini[i]}: {Timer_Mini[i].ToString()} minutes");
                }

                Log.Success("Successfully scanned!");
                if (autoClose)
                    this.ClickImage("resources/close-button.png");
                await Scheduler.ScheduleHunt(Timer_Mini);
            }
        }

        public async Task waitForSpawn(int milliseconds=100)
        {
            await Task.Delay(milliseconds);
            isHunting = true;
            Task huntingDelayTimer = Task.Delay(ai.Settings.huntingDelay * 1000);
            while (isHunting == true)
            {
                Log.Info("Searching for monster...");
                if (huntingDelayTimer.IsCompleted == true)
                {
                    isHunting = false;
                    Log.Error("Cannot find/attack monster over a given period (huntingDelay)...");
                } else
                {
                    await Task.Delay(1000);
                }
                // Use Fly Wing
                ai.Click(new Point(ai.Settings.flyWing[0], ai.Settings.flyWing[1]));
                await Task.Delay(500);
                // Open Auto
                await this.ai.Action.ClickAuto(500);
                // Search for mini indicator ()

                Bitmap Screen = ImageSearch.PrintWindow((IntPtr)screenHandle);
                Point spawnmini = ImageSearch.SearchFromImage(Screen, "resources/miniboss-indicator.png", 0.9);
                if (spawnmini.X >= 0 && spawnmini.Y >= 0)
                {
                    spawnmini.Y = spawnmini.Y - 25;
                    ai.Click(spawnmini);
                    Log.Success("Found target! Attacking...");
                    await Task.Delay(ai.Settings.attackDelay * 1000);
                    isHunting = false;
                }
                await Task.Delay(500);
                await this.ai.Action.ClickAuto(500); // Close Auto
            }
            isHunting = false;
            Log.Info("Monster probably dead by now... Idling...");
        }
    }
    class Interface
    {
        public Point BtnBag, BtnMore, BtnParty;
        public Point BtnMVP, BtnSettings;
        public Interface(IntPtr screenHandle)
        {
            Bitmap Screen = ImageSearch.PrintWindow(screenHandle);
            this.BtnBag = ImageSearch.SearchFromImage(Screen, "resources/bag.png");
            this.BtnMore = ImageSearch.SearchFromImage(Screen, "resources/more.png");
            this.BtnParty = ImageSearch.SearchFromImage(Screen, "resources/party.png");
        }
    }
    class Action
    {
        AI ai;
        public Action(AI ai)
        {
            this.ai = ai;
        }
        public async Task OpenMVP(int millisecondsDelay = 500)
        {
            this.ai.ClickImage("resources/close-button.png");
            await Task.Delay(500);
            this.ai.ClickImage("resources/more.png");
            await Task.Delay(1000);
            this.ai.ClickImage("resources/mvp.png");
            await Task.Delay(1000);
            //this.ai.ClickImage("resources/close-button.png");
        }
        public async Task ClickAuto(int millisecondsDelay)
        {
            this.ai.ClickImage("resources/button-auto.png");
            await Task.Delay(millisecondsDelay);
        }
        public async Task ButterflyWing(int millisecondsDelay = 10000)
        {
            ai.Click(new Point(ai.Settings.butterflyWing[0], ai.Settings.butterflyWing[1]));
            //Point butteryflyWing;
            //Bitmap bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
            //butteryflyWing = ImageSearch.SearchFromImage(bmp, "resources/smokie.png");
            //this.ai.ClickImage("resources/butterfly-wing.png");
            await Task.Delay(millisecondsDelay);
        }
    }
}
