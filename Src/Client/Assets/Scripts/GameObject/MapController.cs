using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public Collider minimapBoundingbox;
	// Use this for initialization
	void Start () {
		MinimapManager.Instance.UpdataMinimap(minimapBoundingbox);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
