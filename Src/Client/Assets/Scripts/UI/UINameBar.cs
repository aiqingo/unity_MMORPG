using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    public Text avanerName;

    public Character character;
	// Use this for initialization
	void Start () {
        if (this.character!=null)
        {
            
        }
	}

    // Update is called once per frame
    void Update()
    {
        this.UpdataInfo();

        print(000);
        this.transform.forward = Camera.main.transform.forward;
        //this.transform.LookAt(Camera.main.transform,Vector3.up);
    }

    void UpdataInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + "  Lv" + this.character.Info.Level;
            if (name!=this.avanerName.text)
            {
                this.avanerName.text = name;
            }
        }


    }
}
