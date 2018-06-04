using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class photontest : Photon.PunBehaviour{
	public GameObject[] prefs;
	public GameObject instaniate_button;
	public GameObject start_button;
	private GameObject genePref;
	private int prefshight =3;
	private PhotonView m_photonview;
	public Camera mainCamera;
	int pivotHeight = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 v = new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, pivotHeight);
		Debug.Log (v.x);
		if (Input.GetMouseButton (0)) {//ボタンが押されている間つまりどこに落とすか操作している時
			Debug.Log ("Input.GetMouseButton");
			genePref.transform.position = v;
		}
	}

	public void push_instantiate_button(){
		genePref = PhotonNetwork.Instantiate("aiti", new Vector2(0, prefshight), Quaternion.identity,0,null);
		genePref.GetComponent<Rigidbody2D>().isKinematic = true;//物理挙動をさせない状態にする
	}

	public void push_title_button(){
		PhotonNetwork.Disconnect ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("TitieScene");
	}
}
