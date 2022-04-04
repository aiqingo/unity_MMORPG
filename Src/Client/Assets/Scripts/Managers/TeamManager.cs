using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class TeamManager:Singleton<TeamManager>
    {
        public void Init()
        {
        }

        public void UpdateTeamInfo(NTeamInfo team)
        {
            User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }

        private void ShowTeamUI(bool show)
        {
            if (UIMain.Instance!=null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }
    }
}
