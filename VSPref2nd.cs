using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VSPref2nd : MonoBehaviour {
	public GameObject[] prefs;
	public Camera mainCamera;
	
	public GameObject camconn;
	public GameObject Judge_bar;	
	public GameObject start_button;
	public GameObject title_button;
	//public GameObject best_string;
	public GameObject score_string;
	public GameObject tweet;
	private GameObject genePref;
	public GameObject backimage;
	public float pivotHeight = 3;
	public static int prefNum = 0;
	//public Text prefname;
	//public Text best_text,score_text;
	public Text score_text;
	public static bool win = false;
	public static bool loose = false;

	public bool isGene;
	public bool isFall;

	private PhotonView m_photonview = null;
	private float Opponentmousepoint = 0;
	private bool OpponentMouseButtonUp;
	private void Start()
	{
		Debug.Log ("VSPref2nd");
		m_photonview = GetComponent<PhotonView>();
		Init();
		//Debug.Log ("start");
	}

	void Init()//初期化
	{
		prefNum = 0;
		win = false;
		loose = false;
		prefMove.isMoves.Clear();
		StartCoroutine(StateReset());
		pivotHeight = 3;
	}

	void Update () {
		if (loose) {
			if (PhotonNetwork.isMasterClient)
				m_photonview.RPC ("SendOpponentWin", PhotonTargets.All);
			Debug.Log ("loose");
			start_button.SetActive (true);
			title_button.SetActive (true);			
			score_text.text = prefNum.ToString ();
			score_string.SetActive (true);
			tweet.SetActive (true);
			if (!loose) {
				Init ();
				Debug.Log ("こんなところくるはずがない");
			}
		}
		else if (win) {
			if (PhotonNetwork.isMasterClient)
			m_photonview.RPC ("SendOpponentLoose",PhotonTargets.All);
			Debug.Log ("win");
			start_button.SetActive (true);
			title_button.SetActive (true);			
			score_text.text = prefNum.ToString ();
			score_string.SetActive (true);
			tweet.SetActive (true);
		}

		if (PhotonNetwork.isMasterClient == true && prefNum % 2 == 0) {//masterかつmasterの時
			Debug.Log ("your turn,Master");
			if (CheckMove (prefMove.isMoves)) {
				return;
			}
			if (!isGene) {
				StartCoroutine (GeneratePref ());
				isGene = true;
				return;
			}

			Vector2 v = new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, pivotHeight);

			if (Input.GetMouseButtonUp (0)) {//もしマウス左クリックが離されたら->落とす
				if (!RotateButton.onButtonDown) {//ボタンをクリックしていたら反応させない
					genePref.transform.position = v;
					genePref.GetComponent<Rigidbody2D> ().isKinematic = false;//落下挙動を開始
					//		Invoke("display_prefname",1.0f);
					//prefNum++;//ここではやらないほうがいい、ismove()関数でprefNum++ 
					isFall = true;//落ちているという判定数
				}
				RotateButton.onButtonDown = false;
			} else if (Input.GetMouseButton (0)) {//ボタンが押されている間つまりどこに落とすか操作している時
				genePref.transform.position = v;
			}
		}

		if (PhotonNetwork.isMasterClient == false && prefNum % 2 == 1) {//opponentかつopponentの時
			Debug.Log ("your turn,opponent");
			m_photonview.RPC ("SendOpponentmousepointToMaster", PhotonTargets.MasterClient, mainCamera.ScreenToWorldPoint (Input.mousePosition).x);
			if (Input.GetMouseButtonUp (0)) {
				m_photonview.RPC ("SendOpponentMouseButtonUpToMaster", PhotonTargets.All, true);
			}
		}

		if (PhotonNetwork.isMasterClient == true && prefNum % 2 == 1) { //masterかつopponentの番
			Debug.Log("you are master, opponent turn");
			if (CheckMove (prefMove.isMoves)) {
				return;
			}
			if (!isGene) {
				StartCoroutine (GeneratePref ());
				isGene = true;
				return;
			}
			Vector2 v = new Vector2 (Opponentmousepoint,pivotHeight);
			if (OpponentMouseButtonUp) {
				genePref.GetComponent<Rigidbody2D> ().isKinematic = false;//落下挙動を開始
				isFall = true;
			}
		}

		/*
		if ((PhotonNetwork.isMasterClient == true && prefNum % 2 == 0) ||
			(PhotonNetwork.isMasterClient == false && prefNum % 2 == 1)) {//マスターでない場合マスターにマウスの座標だけを送る
			if (PhotonNetwork.isMasterClient)
				Debug.Log ("you're master and your turn");
			else
				Debug.Log ("you're player and your turn");			

			if (CheckMove (prefMove.isMoves)) {
				return;//動いている物体がある時はここで判定中断、また上から判定
			}

			if (!isGene) {
				StartCoroutine (GeneratePref ());//こルーチンを動かす。
				isGene = true;
				return;
			}

			Vector2 v;

			if (!PhotonNetwork.isMasterClient) {
				m_photonview.RPC ("SendOpponentmousepointToMaster", PhotonTargets.MasterClient, mainCamera.ScreenToWorldPoint (Input.mousePosition).x);
			}

			if (prefNum % 2 == 0) {
				v = new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, pivotHeight);
			} else
				v = new Vector2 (Opponentmousepoint, pivotHeight); //ここの.xにopponentからの受信した変数を代入if~
			
			if (Input.GetMouseButtonUp (0) && PhotonNetwork.isMasterClient) {//もしマウス左クリックが離されたら->落とす
				if (!RotateButton.onButtonDown) {//ボタンをクリックしていたら反応させない
					genePref.transform.position = v;
					genePref.GetComponent<Rigidbody2D> ().isKinematic = false;//落下挙動を開始
					//		Invoke("display_prefname",1.0f);
					//prefNum++;//ここではやらないほうがいい、ismove()関数でprefNum++ 
					isFall = true;//落ちているという判定数
				}
				RotateButton.onButtonDown = false;
			} else if (Input.GetMouseButton (0)) {//ボタンが押されている間つまりどこに落とすか操作している時
				genePref.transform.position = v;
			}
			*/
	}


	[PunRPC]
	private void SendOpponentmousepointToMaster(float x){
		Opponentmousepoint = x;
	}
	[PunRPC]
	private void SendPrefNumToOpponent(int num){//prefnumをopponentに送信
		prefNum = num;
	}
	[PunRPC]
	public void SendOpponentWin(){//masterが勝った場合looseをopponentに送る
		win = true;
	}
	[PunRPC]
	public void SendOpponentLoose(){
		loose = true;
	}
	[PunRPC]
	public void SendRotatePrefToMaster(){
		RotatePref ();
	}
	[PunRPC]
	public void SendOpponentMouseButtonUpToMaster(bool b){
		OpponentMouseButtonUp = b;
	}
	IEnumerator StateReset()
	{
		while (!win && !loose)
		{
			yield return new WaitUntil(() => isFall);//落下するまで処理が止まる
			yield return new WaitForSeconds(0.1f);
			isFall = false;
			isGene = false;
		}
	}

	IEnumerator GeneratePref()
	{
		//int i = 0;
		while (CameraController.isCollision)
		{
			//i++;
			yield return new WaitForEndOfFrame();//フレームの終わりまで待つ（無いと無限ループ）
			mainCamera.transform.Translate(0,0.1f,0);//カメラを少し上に移動
			camconn.transform.Translate(0,0.1f,0);
			backimage.transform.Translate (0,0.1f,0);
			pivotHeight += 0.1f;//生成位置も少し上に移動
		}
		string tmpfuck;
		tmpfuck = prefs [Random.Range (0, prefs.Length)].name;
		genePref = PhotonNetwork.Instantiate(tmpfuck, new Vector2(0, pivotHeight), Quaternion.identity,0,null);//photon ネットワーク上で生成#1がstring型
		genePref.GetComponent<Rigidbody2D>().isKinematic = true;//物理挙動をさせない状態にする
	}

	public void RotatePref()
	{
		if (!isFall) {
			if (PhotonNetwork.isMasterClient) {
				genePref.transform.Rotate (0, 0, -30);//30度ずつ回転
			} else {
				m_photonview.RPC ("SendRotatePrefToMaster", PhotonTargets.All, true);
			}
		}
	}
	bool CheckMove(List<Moving> isMoves)
	{
		if (isMoves == null) {
			prefNum++;//動くものがいなくなったらprefNumを１たす
			Debug.Log (prefNum);
			return false;
		}
		foreach (Moving b in isMoves) {
			if (b.isMove) {
				return true;//移動中
			}
		}
		if (!isGene) {
			prefNum++;
			m_photonview.RPC ("SendPrefNumToOpponent", PhotonTargets.All,prefNum);
			Debug.Log ("prefNum:" + prefNum.ToString ());
		}
		return false;
	}

	public void RemovetoGame(){
		prefNum = 0;
		win = false;
		loose = false;
		pivotHeight = 3;
		SceneManager.LoadScene ("VsNet2nd");
	}

	public void MovetoTitieScene(){
		PhotonNetwork.Disconnect ();
		SceneManager.LoadScene ("TitieScene");
	}
}