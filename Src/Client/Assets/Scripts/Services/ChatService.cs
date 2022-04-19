using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Services
{
    class ChatService:Singleton<ChatService>
    {
        public void SendChat(ChatChannel sendChannel, string content, int toId, string toName)
        {
            throw new NotImplementedException();
        }
    }
}
