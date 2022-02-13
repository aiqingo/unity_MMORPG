﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{
    public Image mainImago;

    public Image secondImage;

    public Text mainText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMainIcon(string iconName, string text)
    {
        this.mainImago.overrideSprite = Resloader.Load<Sprite>(iconName);
        this.mainText.text = text;
    }
}
