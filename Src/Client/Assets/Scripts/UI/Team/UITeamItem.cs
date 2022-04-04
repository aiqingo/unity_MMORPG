using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{

    public Text nickname;

    public Image classIcon;

    public Image leaderIcon;

    public Image background;

    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? this : false;
    }


    public int idx;

    public NCharacterInfo Info;
    // Use this for initialization
	void Start ()
    {
        this.background.enabled = false;
    }
	
	// Update is called once per frame
	public  void SetMemberInfo (int idx,NCharacterInfo item,bool isLeader)
    {
        this.idx = idx;
        this.Info = item;
        if (this.nickname!=null)  this.nickname.text =this.Info.Level.ToString().PadRight(4) +this.Info.Name;
        if (this.classIcon != null)
            this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int) this.Info.Class - 1];
        if(this.leaderIcon!=null)this.leaderIcon.gameObject.SetActive(isLeader);
    }
}
