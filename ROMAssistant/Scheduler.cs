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

            minimumIndex = 4;

            // Act if idle
            if (isIdle == true)
            {
                isIdle = false;
                await Task.Delay(500);

                //await ai.Action.ButterflyWing();

                //await Task.Delay(10000);


                // Open MVP Interface
                ai.Log.Info($"Hunting {ai.MobName_Mini[minimumIndex]}...");
                await ai.Action.OpenMVP();

                await Task.Delay(500);

                // Click Selected MVP
                Point ReferencePoint = new Point(110, 125); // Smokie
                ReferencePoint = new Point(ReferencePoint.X+110, ReferencePoint.Y + (110 * minimumIndex));//-55
                ai.Click(ReferencePoint);
                await Task.Delay(500);
                ai.Click(new Point(950, 690)); // Click Go
                int delay = minutes[minimumIndex] * 1000 * 60 - 10000;
                //delay = 0;
                if (delay > 0)
                    await Task.Delay(delay);

                await ai.waitForSpawn(minutes[minimumIndex]);
                isIdle = true;
            }
            return true;
        
         }

    }
}
