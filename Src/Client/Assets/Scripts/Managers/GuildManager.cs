﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using SkillBridge.Message;
using UnityEngine.Analytics;

namespace Managers
{
    class GuildManager:Singleton<GuildManager>
    {
        public NGuildInfo guildInfo;

        public NGuildMemberInfo myMemberInfo;
        public bool HasGuild
        {
            get { return this.guildInfo != null; }
        }

        public void Init(NGuildInfo guild)
        {
            this.guildInfo = guild;
            if (guild==null)
            {
                myMemberInfo = null;
                return;
            }

            foreach (var mem in guild.Members)
            {
                if (mem.characterId==User.Instance.CurrentCharacter.Id)
                {
                    myMemberInfo = mem;
                    break;
                }
            }
        }

        public void ShowGuild()
        {
            if (this.HasGuild)
            {
                UIManager.Instance.Show<UIGuild>();
            }
            else
            {
                var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
                win.OnClose += PopNoGuild_OnClose;
            }
        }

        private void PopNoGuild_OnClose(UIWindow sender,UIWindow.WindowResult result)
        {
            if (result==UIWindow.WindowResult.Yes)
            {
                UIManager.Instance.Show<UIGuildPopCreate>();
            }
            else if(result==UIWindow.WindowResult.NO)
            {
                UIManager.Instance.Show<UIGuildList>();
            }

        }
    }
}
