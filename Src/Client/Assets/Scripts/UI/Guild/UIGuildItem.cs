using System.Collections;
using System.Collections.Generic;
using Common;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public Text GuildId;
    public Text Name;

    public Text memderNumber;
    public Text leader;

    public NGuildInfo Info;

    public void SetGuildInfo(NGuildInfo item)
    {
        Info = item;
        if (this.Name!=null) { Name.text = item.GuildName; }
        if (this.GuildId!=null) { GuildId.text =item.Id.ToString(); }
        if (this.leader!=null) { leader.text = item.leaderName; }

        if (this.memderNumber!=null)
        {
            memderNumber.text=item.memberCount+"/"+GameDefine.GuildMaxMemberCount;
        }
    }
}
