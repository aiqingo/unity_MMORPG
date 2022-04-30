﻿using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;
public class UICharacterSelect : MonoBehaviour {

    public GameObject panelCreate;
    public GameObject panelSelect;

    public GameObject btnCreateCancel;

    public InputField charName;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    public Image[] titles;

    public Text descs;


    public Text[] names;

    private int selectCharacterIdx = -1;

    public UICharacterView characterView;

    // Use this for initialization
    void Start()
    {
        //DataManager.Instance.Load();
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }


    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();
            for (int i = 0; i < User.Instance.Info.Player.Characters.Count;i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() => {
                    OnSelectCharacter(idx);
                });
                uiChars.Add(go);
                go.SetActive(true);
            }
        
        }
    }

    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        OnSelectClass(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text) )
        {
            MessageBox.Show("请输入角色名称");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (charClass==0)
        {
            MessageBox.Show("没有选择角色，请选择角色");
            return;
        }
      
        UserService.Instance.SendCharacterCreate(this.charName.text,this.charClass);
    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrectCharacter = charClass - 1;

        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descs.text = DataManager.Instance.Characters[charClass].Description;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }


    void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);

        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        characterView.CurrectCharacter = ((int)cha.Class-1);

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {

            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    public void OnClickPlay()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectCharacterIdx >= 0)
        {
           UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }



   
}
