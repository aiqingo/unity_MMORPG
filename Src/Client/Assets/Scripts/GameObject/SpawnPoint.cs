using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour
{
    private Mesh mesh = null;

    public int ID;

	// Use this for initialization
	void Start ()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position + Vector3.up * this.transform.localScale.y * .5f;

        Gizmos.color = Color.red;

        if (this.mesh!=null)
        {
            Gizmos.DrawWireMesh(this.mesh,pos,this.transform.rotation,this.transform.localScale);

            UnityEditor.Handles.color=Color.red;
            UnityEditor.Handles.ArrowHandleCap(0,pos,this.transform.rotation,1f,EventType.Repaint);
            UnityEditor.Handles.Label(pos,"SpawnPoint:"+this.ID);
        }
    }
#endif
}
