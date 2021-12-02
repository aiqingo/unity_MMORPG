using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEngine;
using  UnityEngine.UI;

public class UILogin : MonoBehaviour {
    [SerializeField]
    private InputField username;
    [SerializeField]
    private InputField password;
    // Use this for initialization
    void Start () {
		UserService.Instance.OnLogin = this.OnLogin;
	}

   
    // Update is called once per frame
    void Update () {
		
	}
    public void OnClickLongin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return; 
        }

        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }

        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    void OnLogin(Result result, string message)
    {
        if (result==Result.Success)
        {
            //登入成功，进入角色选择
            //MessageBox.Show("登入成功，准备选择角色" + message, "提示", MessageBoxType.Information);
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
      
    }
   

}
