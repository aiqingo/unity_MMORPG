﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class ChatService:Singleton<ChatService>
    {
        public ChatService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ChatRequest>(this.OnChat);
        }

        public void Init()
        {
            ChatManager.Instance.Init();
        }

        void OnChat(NetConnection<NetSession> sender, ChatRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnCht::character:{0}:Channel:{1} Message:{2}",character.Id,request.Message.Channel,request.Message.Message);

            if (request.Message.Channel==ChatChannel.Private)
            {
                var chatTo = SessionManager.Instance.GetSession(request.Message.ToId);
                if (chatTo==null)
                {
                    sender.Session.Response.Chat = new ChatResponse();
                    sender.Session.Response.Chat.Result = Result.Failed;
                    sender.Session.Response.Chat.Errormsg = "玩家不在线";
                    sender.Session.Response.Chat.privateMessages.Add(request.Message);
                    sender.SendResponse();
                }
                else
                {
                    if (chatTo.Session.Response.Chat==null)
                    {
                        chatTo.Session.Response.Chat=new ChatResponse();
                    }

                    request.Message.FromId = character.Id;
                    request.Message.FromName = character.Name;
                    chatTo.Session.Response.Chat.Result = Result.Success; chatTo.Session.Response.Chat.privateMessages.Add(request.Message);
                    chatTo.SendResponse();

                    if (sender.Session.Response.Chat==null)
                    {
                        sender.Session.Response.Chat=new ChatResponse();
                    }

                    sender.Session.Response.Chat.Result = Result.Success;
                    sender.Session.Response.Chat.privateMessages.Add(request.Message);
                    sender.SendResponse();
                }

            }
            else
            {
                sender.Session.Response.Chat=new ChatResponse();
                sender.Session.Response.Chat.Result = Result.Success;
                ChatManager.Instance.AddMessage(character, request.Message);
                sender.SendResponse();
            }

        }
    }
}
