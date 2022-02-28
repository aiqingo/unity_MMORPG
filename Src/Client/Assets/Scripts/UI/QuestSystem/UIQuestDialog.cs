using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class UIQuestDialog : UIWindow
{

    public UIQuestInfo questInfo;

    public Quest quest;

    public GameObject openButtons;

    public GameObject submitButtons;
	// Use this for initialization
	void Start () {
		
	}

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info==null)
        {
            openButtons.SetActive(true);
            submitButtons.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status==SkillBridge.Message.QuestStatus.Complated)
            {
                openButtons.SetActive(true);
                submitButtons.SetActive(false);
            }
            else
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(false);
            }
        }
    }

    // Update is called once per frame
	void UpdateQuest () {
        if (this.quest!=null)
        {
            if (this.questInfo!=null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
	}
}
