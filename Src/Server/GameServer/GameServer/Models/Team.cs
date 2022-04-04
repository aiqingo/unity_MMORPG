﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Team
    {
        public int Id;
        public Character Leader;
        public List<Character> Members = new List<Character>();
        public int timestamp;

        public Team(Character leader)
        {
            this.AddMember(leader);
        }

        public void AddMember(Character member)
        {
            if (this.Members.Count==0)
            {
                this.Leader = member;
            }
            this.Members.Add(member);
            member.Team = this;
            timestamp = Time.timestamp;
        }

        public void Leave(Character member)
        {
            Log.InfoFormat("LeaveTeam:{0}:{1}",member.Id,member.Info.Name);
            this.Members.Remove(member);
            if (member==this.Leader)
            {
                if (this.Members.Count>0)
                {
                    this.Leader = this.Members[0];
                }
                else
                {
                    this.Leader = null;
                }

                member.Team = null;
                timestamp = Time.timestamp;
            }
        }


        public void PostProcess(NetMessageResponse message)
        {
            if (message.teamInfo==null)
            {
                message.teamInfo=new TeamInfoResponse();
                message.teamInfo.Result = Result.Success;
                message.teamInfo.Team=new NTeamInfo();
                message.teamInfo.Team.Id = this.Id;
                message.teamInfo.Team.Leader = this.Leader.Id;
                foreach (var member in this.Members)
                {
                    message.teamInfo.Team.Members.Add(member.GetBasicInfo());
                }
            }
        }
    }
}
