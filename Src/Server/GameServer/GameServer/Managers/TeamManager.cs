using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Models;

namespace GameServer.Managers
{
    class TeamManager:Singleton<TeamManager>
    {
        public List<Team> Teams = new List<Team>();
        public Dictionary<int,Team> CharacterTeams=new Dictionary<int, Team>();
        public void Init()
        {
        }

        public Team GetTeamByCharacter(int characterId)
        {
            Team team = null;
            this.CharacterTeams.TryGetValue(characterId, out team);
            return team;
        }

        public void AddTeamMember(Character leader, Character member)
        {
            if (leader.Team==null)
            {
                leader.Team = CreateTame(leader);
            }
            leader.Team.AddMember(member);
        }

        Team CreateTame(Character leader)
        {
            Team team = null;
            for (int i = 0; i < this.Teams.Count; i++)
            {
                team = this.Teams[i];
                if (team.Members.Count==0)
                {
                    team.AddMember(leader);
                    return team;
                }
            }
            team = new Team(leader);
            this.Teams.Add(team);
            team.Id = this.Teams.Count;
            return team;
        }
    }
}
