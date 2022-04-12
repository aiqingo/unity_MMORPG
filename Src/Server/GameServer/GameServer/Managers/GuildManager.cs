using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class GuildManager:Singleton<GuildManager>
    {
        public Dictionary<int ,Guild> Guids=new Dictionary<int, Guild>();
        private HashSet<string> GuildNames=new HashSet<string>();
        public void Init()
        {
            this.Guids.Clear();
            foreach (var guild in DBService.Instance.Entities.Guilds)
            {
                this.AddGuild(new Guild(guild));
            }
        }

        private void AddGuild(Guild guild)
        {
            this.Guids.Add(guild.Id,guild);
            this.GuildNames.Add(guild.name);
            guild.timestamp = TimeUtil.timestamp;
        }

        public bool CheckNameExisted(string name)
        {
            return GuildNames.Contains(name);
        }

        public bool CreateGuild(string name, string notice, Character leader)
        {
            DateTime now=DateTime.Now;
            TGuild dbGuild = DBService.Instance.Entities.Guilds.Create();
            dbGuild.Name = name;
            dbGuild.Notice = notice;
            dbGuild.LeaderID = leader.Id;
            dbGuild.LeaderNmae = leader.Name;
            dbGuild.CreateTime = now;
            DBService.Instance.Entities.Guilds.Add(dbGuild);

            Guild guild=new Guild(dbGuild);
            guild.AddMember(leader.Id,leader.Name,leader.Data.Class,leader.Data.Level,GuildTitle.President);
            leader.Guild = guild;
            DBService.Instance.Save();
            leader.Data.GuildId = dbGuild.Id;
            DBService.Instance.Save();
            this.AddGuild(guild);
            return  true;
        }

        internal Guild GetGuild(int guildId)
        {
            if (guildId==0)
            {
                return null;
            }

            Guild guild = null;
            this.Guids.TryGetValue(guildId, out guild);
            return guild;
        }

        internal List<NGuildInfo> GetGuildsInfo()
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach (var kv in this.Guids)
            {
                result.Add(kv.Value.GuildInfo(null));
            }

            return result;
        }
    }
}
