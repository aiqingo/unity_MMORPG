using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;

    public Text avatarLevel;
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

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGamLeave();
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
}
