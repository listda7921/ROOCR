using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ROMAssistant
{
    /// <summary>
    /// Schedules action for the bot
    /// </summary>
    class Scheduler
    {
        private bool isIdle = true;
        private AI ai;
        public Scheduler(AI reference)
        {
            this.ai = reference;
        }
        /*
         * Action:
         * 1.) Butterfly Wing
         * 2.) More->MVP->Click Icon(find smokie+array*110-24)
         * 3.) Click Go
         * 4.) Wait for timer
         * 5.) Find~
         * 
         *
         */
         public async Task<bool> ScheduleHunt(List<int> minutes)
         {

            // Find Minimum
            int minimumIndex = 0;
            for (int i=0;i<minutes.Count;i++)
            {
                if (minutes[i] <= minimumIndex)
                {
                    minimumIndex = i; // Set Minimum
                }
            }

            //minimumIndex = 4;

            // Act if idle
            if (isIdle == true)
            {
                
                await Task.Delay(500);

                var location = await ai.Action.GetCurrentLocation();

                bool alreadyOnMap = false;
                if (minimumIndex == 0 && location == "Prontera South Gate") alreadyOnMap = true;
                else if (minimumIndex == 2 && location == "Prontera South Gate") alreadyOnMap = true;
                else if (minimumIndex == 1 && location == "Labyrinth Forest") alreadyOnMap = true;
                else if (minimumIndex == 3 && location == "Labyrinth Forest") alreadyOnMap = true;
                else if (minimumIndex == 4 && location == "Prontera West Gate") alreadyOnMap = true;

                if (alreadyOnMap)
                {
                    isIdle = false;
                    ai.Log.Info($"Hunting {ai.MobName_Mini[minimumIndex]}...");
                    await ai.waitForSpawn((minutes[minimumIndex] * 1000 * 60));
                    isIdle = true;
                }
                else
                {
                    isIdle = false;
                    await Task.Delay(500);
                    await ai.Action.ButterflyWing();
                    int milliSecondsToLoad = 15000;
                    await Task.Delay(milliSecondsToLoad);
                    
                    // Open MVP Interface
                    ai.Log.Info($"Hunting {ai.MobName_Mini[minimumIndex]}...");
                    await ai.Action.OpenMVP();

                    await Task.Delay(500);

                    // Click Selected MVP
                    Point ReferencePoint = new Point(110, 125); // Smokie
                    ReferencePoint = new Point(ReferencePoint.X + 110, ReferencePoint.Y + (110 * minimumIndex));//-55
                    ai.Click(ReferencePoint);
                    await Task.Delay(500);
                    ai.Click(new Point(950, 690)); // Click Go

                    int delay = getDelay(minimumIndex);
                    await Task.Delay(delay);

                    await ai.waitForSpawn(Math.Max(0,(minutes[minimumIndex] * 1000 * 60) - delay));
                    isIdle = true;
                }
            }
            return true;
        
         }

        public int getDelay(int minimumIndex)
        {
            int delay = 0;

            switch (minimumIndex)
            {
                case 0: //smokie SP
                    delay = 30;
                    break;
                case 1: //eclipse LF
                    delay = 60;
                    break;
                case 2: //eclipse SP
                    delay = 30;
                    break;
                case 3: //poring LF
                    delay = 60;
                    break;
                case 4: //rocker WP
                    delay = 30;
                    break;
            }
            return delay * 1000;
        }
        
    }
}
