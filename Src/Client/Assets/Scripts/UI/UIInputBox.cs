using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour
{

    public Text tiile;
    public Text message;
    public Text tips;
    public Button buttonYes;
    public Button buttonNo;
    public InputField Input;

    public Text buttonYesTitle;
    public Text buttonNotitle;

    public delegate bool SubmitHandler(string inputText, out string tips);
    public event SubmitHandler OnSubmit;
    public UnityAction OnCancel;

    public string emptyTips;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.tiile.text = title;
        }
        this.message.text = message;
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnOK))
        {
            this.buttonYesTitle.text = title;
        }

        if (!string.IsNullOrEmpty(btnCancel))
        {
            this.buttonNotitle.text = title;
        }

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);
    }

    void OnClickYes()
    {
        this.tips.text = "";
        if (string.IsNullOrEmpty(Input.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }

        if (OnSubmit!=null)
        {
            string tips;
            if (!OnSubmit(this.Input.text,out tips))
            {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
    }

    void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnCancel!=null)
        {
            this.OnClickNo();
        }
    }

}
