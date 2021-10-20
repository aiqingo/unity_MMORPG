using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{

    public GameObject[] Characters;

    private int currentCharacter = 0;

    public int CurrectCharacter
    {
        get { return currentCharacter; }
        set
        {
            currentCharacter = value;

            this.UpdateCharacter();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //角色选择如果增加角色就要对应的增加角色选择
    void UpdateCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            Characters[i].SetActive(i==this.currentCharacter);
        }


    }

}
