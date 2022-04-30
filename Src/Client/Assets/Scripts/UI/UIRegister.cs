using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class UIRegister : MonoBehaviour
{
    [SerializeField]
    private InputField username;
    [SerializeField]
    private InputField password;
    [SerializeField]
    private InputField passwordConfirm;
    [SerializeField]
    private Button buttonRegistor;

	// Use this for initialization
	void Start ()
    {
        UserService.Instance.OnRegister = this.OnRegister;
    }

    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
    }

    // Update is called once per frame
	void Update () {
		
	}

    public void OnClickRegistor()
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

        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }

        if (this.password.text!=passwordConfirm.text)
        {
            MessageBox.Show("两次密码输入不一致");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendRegister(this.username.text,this.password.text);


    }
}
