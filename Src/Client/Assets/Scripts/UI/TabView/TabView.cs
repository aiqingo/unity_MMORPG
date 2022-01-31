using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour
{

    public TabButton[] TabButtons;
    public GameObject[] TabPages;

    public int index = -1;


	// Use this for initialization
	IEnumerator Start () {
        for (int i = 0; i < TabButtons.Length; i++)
        {
            TabButtons[i].tabView = this;
            TabButtons[i].tabIndex = i;
        }

        yield return new WaitForEndOfFrame();
        SelectTab(0);
    }

    public void SelectTab(int index)
    {
        if (this.index!=index)
        {
            for (int i = 0; i < TabButtons.Length; i++)
            {
                TabButtons[i].Select(i == index);
                TabPages[i].SetActive(i==index);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
