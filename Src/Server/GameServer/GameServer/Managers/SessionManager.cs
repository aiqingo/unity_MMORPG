using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;

namespace GameServer.Managers
{
      class SessionManager : Singleton<SessionManager>
    {

       public Dictionary<int ,NetConnection<NetSession>> Sessions=new Dictionary<int, NetConnection<NetSession>>();


        public void AddSession(int characherId, NetConnection<NetSession> session)
        {
            this.Sessions[characherId] = session;
        }


        public void RemoveSession(int characterId)
        {
            this.Sessions.Remove(characterId);
        }

        public NetConnection<NetSession> GetSession(int characterId)
        {
            NetConnection<NetSession> session = null;
            this.Sessions.TryGetValue(characterId, out session);
            return session;
        }

    }
}
