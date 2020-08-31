using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    public class Emulator
    {
        public int screenHandle;
        public Log _log;
        public Emulator(Log log)
        {
            _log = log;
        } 
        public int FindLDPlayer(string windowTitle)
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
                _log.Success("Detected Emulator: LD Player");
            }




            return this.screenHandle;
        }

        public int FindGameWindow()
        {
            Process[] processlist = GetProcesses();

            var ld = processlist.Where(p => p.MainWindowTitle == "LDPlayer").FirstOrDefault();

            var allChildWindows = new WindowHandleInfo(ld.MainWindowHandle).GetAllChildHandles();
            var allChildWindows1 = new WindowHandleInfo(allChildWindows[0]).GetAllChildHandles();
            int MainWindow = (int)allChildWindows[0];
            if (MainWindow > 0)
            {
                _log.Success("Detected Emulator: LD Player");
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
    }
}
