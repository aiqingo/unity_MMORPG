using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Guild
    {
        public int Id { get { return this.Data.Id; } }

        public string name { get { return this.Data.Name; } }

        public double timestamp;

        public TGuild Data;

        public Guild(TGuild guild)
        {
            this.Data = guild;
        }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }

            var dbApply = DBService.Instance.Entities.GuildApplies.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.GuildApplies.Add(dbApply);
            this.Data.Applies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
            {
                return false;
            }

            oldApply.Result = (int)apply.Result;
            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int characterid, string mane, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterid,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now,
            };
            this.Data.Members.Add(dbMember);
            var character = CharacterManager.Instance.GetCharacter(characterid);
            if (character!=null)
            {
                character.Data.GuildId = this.Id;
            }
            else
            {
                TCharacter dbChar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterid);
                dbChar.GuildId = this.Id;
            }
            timestamp = TimeUtil.timestamp;

        }
        //逻辑错的自己增加
        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Guild:{0}:{1}", member.Id, member.Info.Name);

            if (member.Guild==null)
            {
                return;
            }
            member.Guild = null;


        }

        public void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild=new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        internal NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info=new NGuildInfo()
            {
                Id=this.Id,
                GuildName = this.name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderNmae,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count,
            };
            if (from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == this.Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }

        List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach (var member in this.Data.Members)
            {
                var memberInfo=new NGuildMemberInfo()
                {
                    Id=member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime=DateTime.Now;
                
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                   
                }
                members.Add(memberInfo);
            }
            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id=member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in this.Data.Applies)
            {
                if (apply.Result!=(int)ApplyResult.None)
                {
                    continue;
                }
                applies.Add(new NGuildApplyInfo()
                {
                    characterId = apply.CharacterId,
                    GuildId = apply.GuildId,
                    Class = apply.Class,
                    Level = apply.Level,
                    Name = apply.Name,
                    Result = (ApplyResult)apply.Result

                });
            }

            return applies;
        }

        TGuildMember GetDBMember(int characterid)
        {
            foreach (var member in this.Data.Members)
            {
                if (member.CharacterId ==characterid)
                {
                    return member;
                }
            }
            return null;
        }

        internal void ExecuteAdmin(GuildAdminCommand Command, int targetId, int sourceId)
        {
            var target = GetDBMember(targetId);
            var souecr = GetDBMember(sourceId);
            switch (Command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int) GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    target.Title = (int) GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int) GuildTitle.President;
                    souecr.Title = (int) GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderNmae = target.Name;
                    break;
                case GuildAdminCommand.Kickout:
                    //踢人

                    break;
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
        }
    }
}
