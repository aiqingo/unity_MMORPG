﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;
using GameServer.Managers;
using Network;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer
{
    class GameServer
    {
        NetService network;
        Thread thread;
        bool running = false;
        //NetService network;

        public bool Init()
        {
            int Port = Properties.Settings.Default.ServerPort;
            network=new NetService();
            network.Init(8000);

            //HelloWoridService.Instance.Init();

            DBService.Instance.Init();
            DataManager.Instance.Load();
            MapService.Instance.Init();
            UserService.Instance.Init();
            ItemService.Instance.Init();
            QuestService.Instance.Init();
            FriendService.Instance.Init();
            TeamService.Instance.Init();
            GuildService.Instance.Init();
            ChatService.Instance.Init();
            //var a = DBService.Instance.Entities.Characters.Where(s => s.TID == 1);
            //Console.WriteLine("{0}", a.FirstOrDefault<TCharacter>().Name);
            thread = new Thread(new ThreadStart(this.Update));

           
            return true;
        }

        public void Start()
        {
            network.Start();
            //HelloWoridService.Instance.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();
            network.Stop();
        }

        public void Update()
        {
            var mapManager = MapManager.Instance;
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
                mapManager.Update();
            }
        }
    }
}
