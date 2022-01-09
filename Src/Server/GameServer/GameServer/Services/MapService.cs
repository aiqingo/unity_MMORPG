using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
        }


        public void Init()
        {
            MapManager.Instance.Init();
        }

        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapEntitySync: characterID: {0}:{1} Entity.Id: {2} Evt:{3} Entity::{4}",character.Id,character.Info.Name,request.entitySync.Id,request.entitySync.Event,request.entitySync.Entity.String());
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);

        }

        ////private void OnMapCharacterEnter(NetConnection<NetSession> sender,MapCharacterEnterRequestckmessage)
        //{
        
        //}


        public void SendEntityUpdata(NetConnection<NetSession> conn, NEntitySync entity)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entity);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data,0,data.Length);
        }
    }
}
