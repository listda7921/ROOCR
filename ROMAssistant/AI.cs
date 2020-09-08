using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROMAssistant
{
    public class AI
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
                if (hWnd <= 0) this.hWnd = FindLDPlayer(windowTitle);
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

        private int FindLDPlayer(string windowTitle)
        {
            Process[] processlist = GetProcesses();

            var ld = processlist.Where(p => p.MainWindowTitle == "LDPlayer").FirstOrDefault();

            var allChildWindows = new WindowHandleInfo(ld.MainWindowHandle).GetAllChildHandles();
            var allChildWindows1 = new WindowHandleInfo(allChildWindows[0]).GetAllChildHandles();
            var allChildWindows2 = new WindowHandleInfo(allChildWindows[1]).GetAllChildHandles();

            //Bitmap bmp = ImageSearch.PrintWindow((IntPtr)allChildWindows[0]);
            //Bitmap bmp1 = ImageSearch.PrintWindow((IntPtr)ld.MainWindowHandle);
            //Bitmap bmp2 = ImageSearch.PrintWindow((IntPtr)allChildWindows1.FirstOrDefault());
            ////Bitmap bmp2 = ImageSearch.PrintWindow((IntPtr)allChildWindows2.FirstOrDefault());
            //bool script = this.ai.ClickImage("resources/script.png");
            ////var x = ai.Action.ClickScript();



            //Bitmap Screen = ImageSearch.PrintWindow(ld.MainWindowHandle);
            //var BtnBag = ImageSearch.SearchFromImage(Screen, "resources/bag.png");
            //var BtnMore = ImageSearch.SearchFromImage(Screen, "resources/more.png");
            //var BtnParty = ImageSearch.SearchFromImage(Screen, "resources/party.png");

            ////Point point = ImageSearch.SearchFromHandle((IntPtr)ld.MainWindowHandle, "resources/more.png");
            //Point point = new Point(1300, 196);
            ////Win32.Click((int)allChildWindows1.FirstOrDefault(), point.X, point.Y + 30); // NOX Constant
            ////Win32.Click((int)allChildWindows[0], point.X, point.Y + 30); // NOX Constant//win
            ////Win32.Click((int)allChildWindows[1], point.X, point.Y + 30); // NOX Constant
            //Win32.Click((int)ld.MainWindowHandle, point.X, point.Y + 30); // NOX Constant//win


            //var recorder = new WindowHandleInfo(ld.MainWindowHandle).GetAllChildHandles();//GetProcesses();//.Where(p => p.MainWindowTitle == "Operation Recorder").FirstOrDefault();
            //var recorder2 = new WindowHandleInfo(allChildWindows[0]).GetAllChildHandles();

            //var operationrecorder = recorder2[0];
            //var opchild = new WindowHandleInfo(operationrecorder).GetAllChildHandles();
            //var opchild2 = new WindowHandleInfo(ld.MainWindowHandle).GetAllChildHandles();
            //var opchild3 = new WindowHandleInfo(opchild2[1]).GetAllChildHandles();
            //var handle = GetProcesses();
            //        //.SingleOrDefault(x => x.Handle == operationrecorder);//MainWindowTitle.Contains(wName))
            //        //?.Handle;


            //Bitmap bmp3 = ImageSearch.PrintWindow((IntPtr)operationrecorder);
            //Win32.Click((int)operationrecorder, 415, 452); // NOX Constant//win
            //                                               //816 532
            //Bitmap bmp4 = ImageSearch.PrintWindow((IntPtr)ld.MainWindowHandle);
            //Win32.Click((int)ld.MainWindowHandle, 816, 532);

            //Bitmap bmp5 = ImageSearch.PrintWindow((IntPtr)opchild2[1]);
            //Win32.Click((int)opchild2[1], 816, 532);



            //var x = ai.Action.ClickScript((int)allChildWindows[0]);
            //Win32.SendY((int)allChildWindows[0]);

            this.screenHandle = (int)ld.MainWindowHandle;
            int MainWindow = (int)allChildWindows[0];
            if (MainWindow > 0)
            {
                Log.Success("Detected Emulator: LD Player");
            }




            return MainWindow;
        }

        private static Process[] GetProcesses()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Window title: {0}, handle {1}", process.MainWindowTitle, process.MainWindowHandle);
                }
            }

            return processlist;
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
        public bool ScrollDown(Point point)
        {
            if (point.X > -1 && point.Y > -1)
            {
                Win32.ScrollDown(this.hWnd, point.X, point.Y);
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

                Console.WriteLine("Press the Enter key to exit the program at any time... ");
                Console.ReadLine();

                Log.Info("Scanning for Mini Boss... Please wait...");
                await this.Action.OpenMVP();
                Point MonsterImage;
                //await Task.Delay(2000);

                Bitmap bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
                MonsterImage = ImageSearch.SearchFromImage(bmp, "resources/smokie.png");
                Timer_Mini = new List<int>();

                Bitmap crop;
                if (MonsterImage.X == -1 && MonsterImage.Y == -1)
                {
                    Log.Error("Cannot find a reference point.");
                }
                //TODO export loop to aync function
                for (int i = 0; i < 5; i++)
                {
                    try
                    {

                    Point TempPoint;
                    bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
                    TempPoint = new Point(MonsterImage.X + 360, MonsterImage.Y + 110 * i);
                    crop = ImageSearch.CropImage(bmp, TempPoint, 180, 50);
                    crop.Save($"mob{i}.bmp");
                    Timer_Mini.Add(OCR.ExtractTime(OCR.RawOCR(crop)));
                    Log.Info($"{MobName_Mini[i]}: {Timer_Mini[i].ToString()} minutes");
                    await Task.Delay(400);
                    }
                    catch(Exception e)
                    {
                        Log.Info($"Error {e}");
                    }
                    
                }

                //TODO
                //var loop = asyncFunctionCall()
                //check timer: while loop is not resolved?
                // if timer > 10 seconds: continue to line 123
                // await loop

                Log.Success("Successfully scanned!");
                if (autoClose)
                    this.ClickImage("resources/close-button.png");
                await Scheduler.ScheduleHunt(Timer_Mini);
            }
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }

        public async Task waitForSpawn(int milliseconds=100)
        {
            await Task.Delay(milliseconds);
            isHunting = true;
            Task huntingDelayTimer = Task.Delay(ai.Settings.huntingDelay * 1000);

            

            while (isHunting == true)
            {
                var mapOpen = await ai.Action.IsMapOpen();
                if (mapOpen)
                    ai.Click(new Point(1243, 134));//close map
                Log.Info("Searching for monster...");
                if (huntingDelayTimer.IsCompleted == true)
                {
                    isHunting = false;
                    Log.Error("Cannot find/attack monster over a given period (huntingDelay)...");
                } else
                {
                    await Task.Delay(250);
                }
                // Use Fly Wing
                ai.Click(new Point(ai.Settings.flyWing[0], ai.Settings.flyWing[1]));
                await Task.Delay(400);
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
            await this.ai.Action.CancelAuto(500);
        }

        public async Task waitForSpawn(int milliseconds = 100, int fightTime = 30000)
        {
            await Task.Delay(milliseconds);
            isHunting = true;
            Task huntingDelayTimer = Task.Delay(ai.Settings.huntingDelay * 1000);



            while (isHunting == true)
            {
                var mapOpen = await ai.Action.IsMapOpen();
                if (mapOpen)
                    ai.Click(new Point(1243, 134));//close map
                Log.Info("Searching for monster...");
                if (huntingDelayTimer.IsCompleted == true)
                {
                    isHunting = false;
                    Log.Error("Cannot find/attack monster over a given period (huntingDelay)...");
                }
                else
                {
                    await Task.Delay(250);
                }
                // Use Fly Wing
                ai.Click(new Point(ai.Settings.flyWing[0], ai.Settings.flyWing[1]));
                await Task.Delay(400);
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
                    await Task.Delay(fightTime);
                    isHunting = false;
                }
                await Task.Delay(500);
                await this.ai.Action.ClickAuto(500); // Close Auto
            }
            isHunting = false;
            Log.Info("Monster probably dead by now... Idling...");
            await this.ai.Action.CancelAuto(500);
        }
        public async Task<Point> FindMonsterImage(string imageName)
        {
           // var Timer_Mini_extra = 30;
            Bitmap bmp = ImageSearch.PrintWindow((IntPtr)ai.screenHandle);
            var MonsterImage = ImageSearch.SearchFromImage(bmp, imageName);

            Point TempPoint;
            //bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
            //TempPoint = new Point(MonsterImage.X + 360, MonsterImage.Y + 110);
            //var crop = ImageSearch.CropImage(bmp, TempPoint, 180, 50);
            //crop.Save($"mob{imageName}.bmp");
            //Timer_Mini_extra = OCR.ExtractTime(this.ai.OCR.RawOCR(crop));
            //Log.Info($"{imageName}: {Timer_Mini_extra} minutes");
            //await Task.Delay(400);
            return MonsterImage;
        }

        public async Task<int> GetMonsterSpawnTime(Point point)
        {
            var bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
            var timeArea = new Point(point.X + 360, point.Y);
            var crop = ImageSearch.CropImage(bmp, timeArea, 180, 50);
            //crop.Save($"mob{imageName}.bmp");
            int waitTime = OCR.ExtractTime(this.ai.OCR.RawOCR(crop));
            //Log.Info($"{imageName}: {Timer_Mini_extra} minutes");
            await Task.Delay(400);
            return waitTime;
        }

        public async Task<int> FindRotarZairo()
        {
            await this.ai.Action.OpenMVP();

            int spawnTime = 30;

            bool found = false;
            while (!found)
            {
                var MonsterImage = await FindMonsterImage("resources/rotar-zairo.png");
                if (MonsterImage.X == -1 && MonsterImage.Y == -1)
                {
                    //Log.Error("Cannot find a reference point.");
                    await ai.Action.ClickScript();
                    await Task.Delay(750);
                }
                else
                {
                    found = true;
                    spawnTime = await GetMonsterSpawnTime(MonsterImage);
                }
            }

            Log.Info($"Rotar Zairo: {spawnTime} minutes");
            this.ClickImage("resources/close-button.png");
            return spawnTime;
        }

        public string getOCRText(Bitmap crop)
                {
                    return OCR.RawOCR(crop);
                }

        public void LogError(string message)
        {
            Log.Error(message);
        }
    }
    public class Interface
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
    public class Action
    {
        AI ai;
        public Action(AI ai)
        {
            this.ai = ai;
        }
        public async Task OpenMVP(int millisecondsDelay = 500)
        {
            //for (int i = 0; i < 5; i++)
            //{
            //Bitmap bmp = ImageSearch.PrintWindow((IntPtr)ai.screenHandle);
            //bool script = this.ai.ClickImage("resources/script.png");
            bool closeButton = this.ai.ClickImage("resources/close-button.png");
            await Task.Delay(500);
            bool more = this.ai.ClickImage("resources/more.png");
            await Task.Delay(250);
            bool mvp = this.ai.ClickImage("resources/mvp.png");
            await Task.Delay(1000);
            //this.ai.ClickImage("resources/close-button.png");
            //    if (closeButton && more && mvp) continue;
            //    else i++;
            //}
            await Task.Delay(100);
        }
        public async Task CloseMVP()
        {
            var closed = ai.ClickImage("resources/close-button.png");
            if (!closed)
            {
                await Task.Delay(300);
                ai.ClickImage("resources/close-button.png");
            }
            await Task.Delay(300);
        }
        public async Task ClickAuto(int millisecondsDelay)
        {
            this.ai.ClickImage("resources/button-auto.png");
            await Task.Delay(millisecondsDelay);
        }
        public async Task CancelAuto(int millisecondsDelay)
        {
            //this.ai.ClickImage("resources/cancel-auto.png");
            ai.Click(new Point(ai.Settings.cancelAuto[0], ai.Settings.cancelAuto[1]));
            await Task.Delay(millisecondsDelay);
        }
        public async Task ButterflyWing(int millisecondsDelay = 1000)
        {
            ai.Click(new Point(ai.Settings.butterflyWing[0], ai.Settings.butterflyWing[1]));
            //Point butteryflyWing;
            //Bitmap bmp = ImageSearch.PrintWindow((IntPtr)screenHandle);
            //butteryflyWing = ImageSearch.SearchFromImage(bmp, "resources/smokie.png");
            //this.ai.ClickImage("resources/butterfly-wing.png");
            await Task.Delay(millisecondsDelay);
        }

        public async Task<string> GetCurrentLocation()
        {

            await Task.Delay(200);

            var isMapOpen = await IsMapOpen();

            var map = new Point(1188, 49);
            if (isMapOpen)
            {
                ai.Click(new Point(1243, 134));//close map
                await Task.Delay(200);
                ai.Click(map); //1188, 49
            }
            else
            {
                ai.Click(map); //1188, 49
            }

            await Task.Delay(300);


            Bitmap bmp2 = new Bitmap(ImageSearch.PrintWindow((IntPtr)this.ai.screenHandle));

            var TempPoint = new Point(940, 150);
            Bitmap crop = ImageSearch.CropImage(bmp2, TempPoint, 220, 40);

            var text = ai.getOCRText(crop);
            Regex regex = new Regex(@"([a-zA-Z\s]+)");
            Match match = regex.Match(text);
            var val = match.Groups[0].Value;

            string location = "";
            if (val.Length < 10 && val.Contains("tera")) location = "Prontera";
            else if (val.Contains("West")) location = "Prontera West Gate";
            else if (val.Contains("Lab")) location = "Labyrinth Forest";
            else if (val.Contains("SDuth")) location = "Prontera South Gate";
            else if (val.Contains("GDblin FDrest")
                || val.Contains("GDblin")
                || val.Contains("Goblin")
                || val.Contains("Goblin Forest")) location = "Goblin Forest";
            else if (val.Contains("Desert")) location = "Sograt Desert";
            else if (val.Contains("Cave")) location = "Underwater Cave";
            var westgate = val.Length;

            bmp2.Dispose();

            await Task.Delay(100);
            ai.Click(new Point(1243, 134));//close map

            return location;
        }

        public async Task<bool> IsMapOpen()
        {
            Bitmap bmp;
            Point worldMapImage;

            bmp = new Bitmap(ImageSearch.PrintWindow((IntPtr)this.ai.screenHandle));
            //await Task.Delay(100);
            worldMapImage = ImageSearch.SearchFromImage(bmp, "resources/world-map.png");
           // await Task.Delay(100);
            return !(worldMapImage.X == -1 && worldMapImage.Y == -1);
        }
        public async Task OpenMap()
        {
            var map = new Point(1188, 49);
            ai.Click(map); //1188, 49
            await Task.Delay(100);
        }
        public async Task CloseMap()
        {
            var isMapOpen = await IsMapOpen();
            if(isMapOpen)
                ai.Click(new Point(1243, 134));//close map
        }
        //990,395
        public async Task GoToKafraAgent()
        {
            await Task.Delay(500);
            await ai.Action.ButterflyWing();
            await Task.Delay(4000);
            await DelayOnLocation();
            await OpenMap();
            await Task.Delay(600);
            var kafraLocationOnMap = new Point(990, 395);
            ai.Click(kafraLocationOnMap);
            await CloseMap();
            await Task.Delay(7000);

            Bitmap bmp = ImageSearch.PrintWindow((IntPtr)ai.screenHandle);
            await Task.Delay(250);
            Point kafraImg = await GetKafraImage(bmp);
            await Task.Delay(250);
            Bitmap bmp2 = ImageSearch.CropImage(bmp, kafraImg);
            ai.Click(new Point(kafraImg.X + 2, kafraImg.Y - 2));//adjusted click from image

            await Task.Delay(1000);
            //ai.ClickImage("resources/teleport.png");
            ai.Click(new Point(1112, 472));//teleport
            await Task.Delay(2000);
        }

        private async Task<Point> GetKafraImage(Bitmap bmp)
        {
            var kafraImg = ImageSearch.SearchFromImage(bmp, "resources/kafra2.png");
            if (kafraImg.X == -1 && kafraImg.Y == -1)
            {
                for (int i = 0; i <= 20; i++)
                {
                    await Task.Delay(500);
                    Bitmap bmp2 = ImageSearch.PrintWindow((IntPtr)ai.screenHandle);
                    kafraImg = ImageSearch.SearchFromImage(bmp2, "resources/kafra2.png");
                    if (kafraImg.X != -1 && kafraImg.Y != -1) continue;
                    if (i == 10) ai.LogError("Couldnt find Kafra image");
                }
            }
            return kafraImg;
        }

        public async Task teleportToGoblinForest()
        {
            await ai.Action.GoToKafraAgent();
            ai.Click(new Point(532, 331)); //geffen area
            await Task.Delay(300);
            //send u
            Win32.SendU(this.ai.hWnd);
            await Task.Delay(2000);
            //1057, 542
            ai.Click(new Point(1057, 542)); //goblin forest
            await CloseTeleportWindowIfOpen(4000);
        }

        public async Task teleportToDesert()
        {
            await ai.Action.GoToKafraAgent();
            ai.Click(new Point(530, 400)); //morroc 530 400
            await Task.Delay(300);
            ai.Click(new Point(1056, 570)); //desert 1056 570
            await CloseTeleportWindowIfOpen(4000);
        }

        public async Task teleportToUnderWaterCave()
        {
            await ai.Action.GoToKafraAgent();

            for (int i = 0; i < 5; i++)
            {
                //send u
                Win32.SendU(this.ai.hWnd);
                await Task.Delay(2000);
            }
            
            //1057, 542
            ai.Click(new Point(1060, 488)); //underwater cave
            await CloseTeleportWindowIfOpen(4000);
        }

        public async Task CloseTeleportWindowIfOpen(int milliseconds)
        {
            await Task.Delay(milliseconds);
            Bitmap bmp2 = ImageSearch.PrintWindow((IntPtr)ai.screenHandle);
            var midgard = ImageSearch.SearchFromImage(bmp2, "resources/Midgaurd-logo.png");
            if (midgard.X != -1 && midgard.Y != -1)
            {
                ai.Click(new Point(50, 50)); //back
            }
        }

        public async Task ClickScript()
        {
            Win32.SendY(this.ai.hWnd);
            await Task.Delay(800);
        }

        public async Task ScrollDown()
        {
            Win32.ScrollTo((IntPtr)this.ai.hWnd, 10);
        }

        public async Task GetMoreMinis()
        {

        }
        public async Task DelayOnLocation(MonsterType? monsterType = null)
        {
            var arrived = false;
            int i = 0;
            while (arrived == false)
            {
                if (i > 60) break;

                var currentLocation = await ai.Action.GetCurrentLocation();

                ai.Log.Info($"Location: {currentLocation}...");
                
                if (monsterType == null && currentLocation == "Prontera")
                {
                    arrived = true;
                    break;
                }
                if (monsterType == MonsterType.RotorZario && currentLocation == "Goblin Forest")
                {
                    break;
                }
                if (monsterType == MonsterType.Smokie && currentLocation == "Prontera South Gate")
                {
                    break;
                }
                if (monsterType == MonsterType.EclipseS && currentLocation == "Prontera South Gate")
                {
                    break;
                }
                if (monsterType == MonsterType.EclipseL && currentLocation == "Labyrinth Forest")
                {
                    break;
                }
                if (monsterType == MonsterType.VocalL && currentLocation == "Labyrinth Forest")
                {
                    break;
                }
                if (monsterType == MonsterType.Mastering && currentLocation == "Labyrinth Forest")
                {
                    break;
                }
                if (monsterType == MonsterType.VocalWG && currentLocation == "Prontera West Gate")
                {
                    break;
                }
                if (monsterType == MonsterType.VagabondWolf && currentLocation == "Sograt Desert")
                {
                    break;
                }
                if (monsterType == MonsterType.Toad && currentLocation == "Underwater Cave")
                {
                    break;
                }
                i++;
                await Task.Delay(1000);
            }

            await Task.Delay(500);
            var mapOpen = await ai.Action.IsMapOpen();
            if (mapOpen)
                ai.Click(new Point(1243, 134));//close map

        }

        public async Task RouteToMob(int minimumIndex, MonsterType? type = null)
        {
            // Open MVP Interface
           // ai.Log.Info($"Hunting {ai.MobName_Mini[minimumIndex]}...");
            await ai.Action.OpenMVP();

            await Task.Delay(500);

            // Click Selected MVP
            Point ReferencePoint = new Point(110, 125); // Smokie
            ReferencePoint = new Point(ReferencePoint.X + 110, ReferencePoint.Y + (110 * minimumIndex));//-55
            ai.Click(ReferencePoint);
            await Task.Delay(500);
            ai.Click(new Point(950, 690)); // Click Go
            await DelayOnLocation(type);
        }

        public async Task UseButterFlyWing()
        {
            //var currentLocation = await ai.Action.GetCurrentLocation();
            //if (currentLocation != "Prontera")
            //{
                await Task.Delay(500);
                await ai.Action.ButterflyWing(4000);
                //int milliSecondsToLoad = 25000;
                //await Task.Delay(milliSecondsToLoad);
                await DelayOnLocation();
            //}
        }

    }
}
