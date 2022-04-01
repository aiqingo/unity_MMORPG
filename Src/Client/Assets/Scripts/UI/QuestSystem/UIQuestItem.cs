using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIQuestItem : ListView.ListViewItem
{
    public Text title;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Quest quest;
    // Use this for initialization
	void Start () {
		
	}

    private bool isEquiped = false;

    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title!=null) this.title.text = this.quest.Define.Name;
        
    }

    void Update () {
		
	}
}
