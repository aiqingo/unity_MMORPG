using System.Collections;
using System.Collections.Generic;
using Common;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour
{

    public Text guildNmae;
    public Text guildID;
    public Text leader;

    public Text notice;

    public Text memderNumber;


    private NGuildInfo info;

    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value;this.UpdateUI(); }
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void UpdateUI () {
        if (this.info == null)
        {
            this.guildNmae.text = "无";
            this.guildID.text = "ID:0";
            this.leader.text = "会长：无";
            this.notice.text = "";
            this.memderNumber.text = string.Format("成员数量：0/{0}", GameDefine.GuildMaxMemberCount);
        }
        else
        {
            this.guildNmae.text = this.Info.GuildName;
            this.guildID.text = "ID:"+this.Info.Id;
            this.leader.text = "会长："+this.Info.leaderName;
            this.notice.text = this.Info.Notice;
            this.memderNumber.text = string.Format("成员数量：{0}/{1}", this.info.memberCount,GameDefine.GuildMaxMemberCount);
        }
    }
}
