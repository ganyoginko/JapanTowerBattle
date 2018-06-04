using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon;

public class VSPrefGene : Photon.PunBehaviour{
	public GameObject[] prefs;
	public Camera mainCamera;
	//public Camera tmpmainCamera;
	public GameObject camconn;
	public GameObject Judge_bar;	
	public GameObject start_button;
	public GameObject title_button;
	public GameObject judge_string_object;
	//public Text best_int;
	public GameObject score_string;
	//public Text score_int;
	public GameObject tweet;
	public float pivotHeight = 3;
	public static int prefNum = 0;
	public static bool isGameOver = false;
	public static bool isGameOveropponent = false;
	private GameObject genePref;
	public bool isGene;
	public bool isFall;
	public Text judge_string;
	private PhotonView  m_photonView = null; // RPCのやつ

	string tmpfuck;

	private int sendint;
	private int reciveint;

	void Awake(){
		m_photonView    = GetComponent<PhotonView>();
	}

	private void Start()
	{
		Debug.Log ("VsprefGene_start");
		Init();
		//Debug.Log ("start");
	}

	void Init()//初期化
	{
		prefNum = 0;
		isGameOver = false;
		prefMove.isMoves.Clear();
		StartCoroutine(StateReset());
		pivotHeight = 3;
		//mainCamera = tmpmainCamera;
	}
		
	/**********************************************************************/
	void Update () {
		Debug.Log ("Update");
		if (PhotonNetwork.isMasterClient == true && prefNum % 2 == 0 || PhotonNetwork.isMasterClient == false && prefNum % 2 == 1) {
			Debug.Log ("your turn");
			if (isGameOver) {
				Debug.Log ("gameover");
				//Checkscore ();
				m_photonView.RPC ("SendIsGameOver", PhotonTargets.All,isGameOveropponent);//gameoverの場合相手のisgameoveropponentに送る
				judge_string.text = "ざんねん！　もっかい！";
				PlayerPrefs.SetInt ("winnum",PlayerPrefs.GetInt("winnum")+1);
				start_button.SetActive (true);
				title_button.SetActive (true);
				judge_string_object.SetActive (true);
				title_button.SetActive (true);
				
				tweet.SetActive (true);
				//		score_int.text = prefNum.ToString ();		
				// 	optionsound.soundinit();
				if (!isGameOver) {
					Init ();
				}
			} else if (isGameOveropponent) {
				judge_string.text = "やったね！　勝った！";
				PlayerPrefs.SetInt ("loosenum", PlayerPrefs.GetInt ("loose") + 1);
				start_button.SetActive (true);
				title_button.SetActive (true);
				judge_string_object.SetActive (true);
				title_button.SetActive (true);
				tweet.SetActive (true);
			}
		

			if (CheckMove (prefMove.isMoves)) {
				Debug.Log ("checkMove");
				return;//動いている物体がある時はここで判定中断、また上から判定
			}

		
			if (!isGene) {
				
				StartCoroutine (GeneratePref ());//こルーチンを動かす。
				isGene = true;
				return;
			}

			Vector2 v = new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, pivotHeight);

			if (Input.GetMouseButtonUp (0)) {//もしマウス左クリックが離されたら->落とす
				Debug.Log ("Input.GetMouseButtonUp(0)");
				if (!RotateButton.onButtonDown) {//ボタンをクリックしていたら反応させない
					Debug.Log ("落下開始");
	                genePref.transform.position = v;

					genePref.GetComponent<Rigidbody2D> ().isKinematic = false;//落下挙動を開始

					prefNum++;
					m_photonView.RPC ("SendPrefNum", PhotonTargets.All, prefNum);//RPC を用いた同期方法
					Debug.Log ("send" + prefNum);

					isFall = true;//落ちているという判定数
					Debug.Log ("fall down");
				}
				RotateButton.onButtonDown = false;
			} else if (Input.GetMouseButton (0)) {//ボタンが押されている間つまりどこに落とすか操作している時
				Debug.Log ("Input.GetMouseButton");
				genePref.transform.position = v;
			}      
		}
	}

	[PunRPC]
	private void SendPrefNum(int SendedData){//相手が送ってきたときに自動的に発動
		prefNum = SendedData;
		//相手から来たデータを自分の受け皿に上書き保存
		Debug.Log("recieve" + prefNum.ToString());
	}

	[PunRPC]
	public void SendIsGameOver(bool over){
		isGameOveropponent = over;
		Debug.Log ("receive Opponent is gameover");
	}
	/*************************************************************************************/

	
	public void RemovetoGame(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("lobbyscene");
	}

	public void MovetoTitieScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("TitieScene");
	}


	public void cutconnection(){
		PhotonNetwork.Disconnect();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("TitieScene");
	}

	/*void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting) {
			stream.SendNext (prefNum);
		}
		else{
			prefNum = stream.ReceiveNext();
		}
	}*/



	IEnumerator StateReset()
	{
		Debug.Log ("StateReset");
		while (!isGameOver)
		{
			Debug.Log ("StateReset_!isGameOver");
			yield return new WaitUntil(() => isFall);//落下するまで処理が止まる
			yield return new WaitForSeconds(0.1f);
			isFall = false;
			isGene = false;
		}
	}


	IEnumerator GeneratePref()
	{
		Debug.Log("GeneratePref");
		while (CameraController.isCollision)
		{
			Debug.Log ("Generatepref_CameraController.isCollision");
		
			yield return new WaitForEndOfFrame();//フレームの終わりまで待つ（無いと無限ループ）
			mainCamera.transform.Translate(0,0.1f,0);//カメラを少し上に移動
			camconn.transform.Translate(0,0.1f,0);
			pivotHeight += 0.1f;//生成位置も少し上に移動	
		}
		


		tmpfuck = prefs [Random.Range (0, prefs.Length)].name;
		
		genePref = PhotonNetwork.Instantiate(tmpfuck, new Vector2(0, pivotHeight), Quaternion.identity,0,null);//photon ネットワーク上で生成#1がstring型
		//genePref = PhotonNetwork.Instantiate(prefs[Random.Range(0, prefs.Length)], new Vector2(0, pivotHeight), Quaternion.identity,0);//photon ネットワーク上で生
		//genePref = Instantiate(prefs[9], new Vector2(0, pivotHeight), Quaternion.identity);//回転せずに生成
		genePref.GetComponent<Rigidbody2D>().isKinematic = true;//物理挙動をさせない状態にする
	}

	public void RotatePref()
	{
		Debug.Log ("RotatePref");
		if(!isFall)
			genePref.transform.Rotate(0,0,-30);//30度ずつ回転
	}

	bool CheckMove(List<Moving> isMoves)
	{
		Debug.Log ("CheckMove");
		if (isMoves == null)
		{
			return false;
		}
		foreach (Moving b in isMoves)
		{
			if (b.isMove)
			{
				return true;//移動中
			}
		}
		return false;
	}
}