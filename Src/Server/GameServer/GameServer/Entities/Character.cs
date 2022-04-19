using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;
using Network;

namespace GameServer.Entities
{
    class Character : CharacterBase,IPostResponser
    {
       
        public TCharacter Data;
        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;

        public Team Team;
        public double TeamUpdateTS;
        public Guild Guild;
        public double GuildUpdateTs;

        public Chat Chat;
        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Goid = cha.Gold;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);
            this.Info.Bag=new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unloked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);
            this.StatusManager = new StatusManager(this);
            this.FriendManager=new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);
            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);
            this.Chat=new Chat(this);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold==value)
                {
                    return;
                }

                this.StatusManager.AddGoldChange((int)(value-this.Data.Gold));
                this.Data.Gold = value;
            }
        }

        public void PostProcess(NetMessageResponse message)
        {
            this.FriendManager.PostProcess(message);

            if (this.Team!=null)
            {
                Log.InfoFormat("postProcess>Team:characterID:{0}:{1}  {2}<{3}",this.Id,this.Info.Name,TeamUpdateTS,this.Team.timestamp);
                if (TeamUpdateTS<this.Team.timestamp)
                {
                    TeamUpdateTS = Team.timestamp;
                    this.Team.PostProcess(message);
                }
            }

            if (this.Guild != null)
            {
                Log.InfoFormat("PostProcess>Guild:characterID:{0}:{1} {2}<{3}", this.Id,this.Info.Name,GuildUpdateTs,this.Guild.timestamp);
                if (this.Guild != null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter!=null)
                    {
                        GuildUpdateTs = Guild.timestamp;
                    }
                }

                if (GuildUpdateTs < this.Guild.timestamp && message.mapCharacterEnter == null)
                {
                    GuildUpdateTs = Guild.timestamp;
                    this.Guild.PostProcess(this,message);
                }
            }
            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }

            this.Chat.PostProcess(message);

        }

        public void Clear()
        {
            this.FriendManager.OfflineNotify();
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = Info.Id,
                Name = Info.Name,
                Class = Info.Class,
                Level = Info.Level,
            };
        }
    }
}
