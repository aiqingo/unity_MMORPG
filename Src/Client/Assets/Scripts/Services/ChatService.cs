using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Managers;
using Network;
using SkillBridge.Message;
using UnityEngine;

namespace Services
{
    class ChatService:Singleton<ChatService>
    {
        public void Init()
        {
        }

        public ChatService()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(this.OnChat);
        }

        public void SendChat(ChatChannel channel, string content, int toId, string toName)
        {
            Debug.Log("SendBuyItem");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.Message=new ChatMessage();
            message.Request.Chat.Message.Channel = channel;
            message.Request.Chat.Message.ToId = toId;
            message.Request.Chat.Message.ToName = toName;
            message.Request.Chat.Message.Message = content;
            NetClient.Instance.SendMessage(message);
        }

        void OnChat(object sender,ChatResponse message)
        {
            if (message.Result==Result.Success)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Local,message.localMessages);

                ChatManager.Instance.AddMessages(ChatChannel.World, message.worldMessages);
                ChatManager.Instance.AddMessages(ChatChannel.System, message.systemMssages );
                ChatManager.Instance.AddMessages(ChatChannel.Private , message.privateMessages );
                ChatManager.Instance.AddMessages(ChatChannel.Team , message.teamMessages );
                ChatManager.Instance.AddMessages(ChatChannel.Guild , message.guildMessages );
            }
            else
            {
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }


    }
}
