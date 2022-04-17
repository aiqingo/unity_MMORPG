using System.Collections;
using System.Collections.Generic;
using Managers;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;

    public Text avatarLevel;

    public UITeam TeamWindow;

	// Use this for initialization
	protected override  void OnStart ()
    {
        this.UpdataAvater();

    }

    void UpdataAvater()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name,
            User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();

    }

    // Update is called once per frame
	void Update () {
		
	}


    public void OnClickTest()
    {
       UITest uiTest=  UIManager.Instance.Show<UITest>();
       uiTest.SetTitle("这是JSJ");
       uiTest.OnClose += Test_Onclse;
    }

    private void Test_Onclse(UIWindow sender,UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框：" + result, "对话框相应结果", MessageBoxType.Confirm);
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickCharEquio()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }

    public void OnClickSkill()
    {
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
}
