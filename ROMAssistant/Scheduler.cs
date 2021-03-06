﻿using ROMAssistant.Monsters;
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
            //for (int i=0;i<minutes.Count;i++)
            //{
            //    if (minutes[i] <= minimumIndex)
            //    {
            //        minimumIndex = i; // Set Minimum
            //    }
            //}

            //minimumIndex = 4;

            var spawnedMinis = new List<int>();
            bool alreadyOnMap = false;

            for (int i = 0; i < minutes.Count; i++)
            {
                if (minutes[i] == 0)
                {
                    spawnedMinis.Add(i);
                }
                if (minutes[i] <= minimumIndex)
                {
                    minimumIndex = i; // Set Minimum
                }
            }

            var location = await ai.Action.GetCurrentLocation();
            ai.Log.Info($"Location: {location}...");

            if (spawnedMinis.Count() > 1)
            {
                if (spawnedMinis.Where(m => m == 0 || m == 2).Any() && location == "Prontera South Gate")
                {
                    minimumIndex = spawnedMinis.Where(m => m == 0 || m == 2).First();
                    alreadyOnMap = true;
                }
                if (spawnedMinis.Where(m => m == 1 || m == 3).Any() && location == "Labyrinth Forest")
                {
                    minimumIndex = spawnedMinis.Where(m => m == 1 || m == 3).First();
                    alreadyOnMap = true;
                }
            }



            // Act if idle
            if (isIdle == true)
            {



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
                    await ButterFlyWing();

                    var teleport = false;
                    if (teleport)
                    {
                        //var rotarTime = await ai.FindRotarZairo();
                        ////await ai.Action.GoToKafraAgent();
                        ////await ai.Action.ClickScript();
                        ////532, 331
                        //if (rotarTime == 0)
                        //{
                        //    await ai.Action.teleportToGoblinForest();
                        //    await ai.waitForSpawn(Math.Max(0, (0)));//- delay
                        //}
                        var active = true;
                        while (active)
                        {
                            //RotarZairo rotoZairo = new RotarZairo(ai);
                            Smokie smokie = new Smokie(ai);
                            MonsterList monsterList = new MonsterList(
                                //rotoZairo, 
                                smokie
                                );
                            ScanMini scanMini = new ScanMini(ai, new OCR(), ai.Log);

                            var _hunt = new Hunt(scanMini);
                            var mobs = monsterList.GetMonsterList();
                            await _hunt.GatherMobTimes(mobs);
                            await _hunt.HuntMonsters(mobs);

                        }
                    }
                    else
                    {
                        await RouteToMob(minimumIndex);

                    }
                    //int delay = getDelay(minimumIndex);
                    //await Task.Delay(delay);
                    await DelayOnLocation(minimumIndex);
                    await ai.waitForSpawn(Math.Max(0, (minutes[minimumIndex] * 1000 * 60)));//- delay
                    isIdle = true;
                }
            }
            return true;
        
         }

        private async Task ButterFlyWing()
        {
            var currentLocation = await ai.Action.GetCurrentLocation();
            if (currentLocation != "Prontera")
            {
                await Task.Delay(500);
                await ai.Action.ButterflyWing(4000);
                //int milliSecondsToLoad = 25000;
                //await Task.Delay(milliSecondsToLoad);
                await DelayOnLocation(-1);
            }
        }

        private async Task RouteToMob(int minimumIndex)
        {
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
        }

        public int getDelay(int minimumIndex)
        {
            int delay = 0;

            switch (minimumIndex)
            {
                case 0: //smokie SP
                    delay = 35;
                    break;
                case 1: //eclipse LF
                    delay = 60;
                    break;
                case 2: //eclipse SP
                    delay = 35;
                    break;
                case 3: //poring LF
                    delay = 60;
                    break;
                case 4: //rocker WP
                    delay = 35;
                    break;
            }
            return delay * 1000;
        }

        public async Task DelayOnLocation(int? index, MonsterType? monsterType = null)
        {
            var arrived = false;
            int i = 0;
            while(arrived == false)
            {
                if (i > 60) break;

                var currentLocation = await ai.Action.GetCurrentLocation();
                
                ai.Log.Info($"Location: {currentLocation}...");
                
                if (index == -1 && currentLocation == "Prontera")
                {
                    arrived = true;
                    break;
                }
                if ((index == 0 || index == 2) && currentLocation == "Prontera South Gate")
                {
                    arrived = true;
                    break;
                }
                if ((index == 1 || index == 3) && currentLocation == "Labyrinth Forest") {
                    arrived = true;
                    break;
                }
                if(index == 4 && currentLocation == "Prontera West Gate")
                {
                    arrived = true;
                    break;
                }
                if(monsterType == MonsterType.RotorZario && currentLocation == "Goblin Forest")
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
        
    }

    public class MonsterList {
        //public RotarZairo _rotoZairo;
        public Smokie _smokie;
        public MonsterList(
            //RotarZairo rotoZairo 
            Smokie smokie
            ) {
           // _rotoZairo = rotoZairo;
            _smokie = smokie;
        }

        public List<Monster> GetMonsterList()
        {
            var monsters = new List<Monster>();
            
            //monsters.Add(_rotoZairo);
            monsters.Add(_smokie);

            return monsters;
        }
    }

    public class Hunt
    {
        public ScanMini _scanMini;
        public Hunt(
            ScanMini scanMini
            ) {
            _scanMini = scanMini;
        }

        public async Task GatherMobTimes(List<Monster> monsters)
        {
            await _scanMini.ScanAllMonsters(monsters);
        }

        public async Task HuntMonsters(List<Monster> monsters)
        {
            IEnumerable<Monster> mobs = monsters.OrderBy(m => m.MinutesToSpawn);
            foreach (var mob in mobs)
            {
               await mob.GoToLocation();
               await mob.Hunt();
            }
        }

        public async Task HuntMonster(Monster monster)
        {
            await monster.GoToLocation();
            await monster.Hunt();
        }







    }
}
