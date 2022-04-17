using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildApplyItem : ListView.ListViewItem
{

    public Text nickname;
    public Text @class;
    public Text level;

    public NGuildApplyInfo Info;
    // Use this for initialization
    public void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if (this.nickname != null) { nickname.text = this.Info.Name; }
        if (this.@class != null) { @class.text = this.Info.Class.ToString(); }
        if (this.level != null) { level.text = this.Info.Level.ToString(); }

    }

    public void OnAccept()
    {
        MessageBox.Show(String.Format("要通过【{0}】的申请吗？", this.Info.Name), "公会申请", MessageBoxType.Confirm, "同意", "拒绝")
                .OnYes =
            () =>
            {
                GuildService.Instance.SendGuildJoinApply(true,this.Info);
            };
    }
    public void OnDecline()
    {
        MessageBox.Show(String.Format("要拒绝【{0}】的申请吗？", this.Info.Name), "公会申请", MessageBoxType.Confirm, "同意", "拒绝")
                .OnNo =
            () =>
            {
                GuildService.Instance.SendGuildJoinApply(false, this.Info);
            };
    }


}
