using UnityEngine;
using System.Collections;

public class NodeMaterial : MonoBehaviour, ICovering  {

	//public Texture[] cover;
    public Renderer rend;
	private MeshRenderer mesh;
	public Material[] mat;
	public int cov; //0 - не покрыто



	void Start () {
		rend = GetComponent<Renderer> ();
		mesh = GetComponent<MeshRenderer> ();
		//rend.enabled = true;
		cov = 0;
		UpdateCover ();
//		rend.material.mainTexture = cover [0];
	}

	public void Cover(int x)
	{
		cov += x;
		//rend.enabled = true;
		UpdateCover ();
	}

	public void UnCover(int x)
	{
		if (cov != 0) {
			
			cov -= x;

		} 
		UpdateCover ();
	}

	void UpdateCover()
	{
		if (cov != 0) {
			mesh.enabled = true;

		}
		if (cov == 0) {
			rend.material = mat [0];
			mesh.enabled = false;
			//rend.enabled = false;
		} else if (cov < 2) {
			rend.material = mat [3];
		} else if (cov < 3) {
			rend.material = mat [2];
		} else if (cov >= 3) {
			rend.material = mat [1];
		} else {
			rend.material = mat [0];
		}
	}

}
