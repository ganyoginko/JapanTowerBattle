using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class prefGene : MonoBehaviour {
	public GameObject[] prefs;
	public Camera mainCamera;
	//public Camera tmpmainCamera;
	public GameObject camconn;
	public GameObject Judge_bar;	
	public GameObject start_button;
	public GameObject title_button;
	public GameObject best_string;
	public GameObject score_string;
	public GameObject tweet;
	private GameObject genePref;
	public GameObject backimage;
	public float pivotHeight = 3;
	public static int prefNum;
	public Text prefname;
	public Text best_text,score_text;
	public static bool isGameOver = false;
	
	public bool isGene;
	public bool isFall;

	private void Start()
	{
		Debug.Log ("prefGene_start");
		if (PlayerPrefs.GetInt ("fifth") == null) {
			Debug.Log ("init_runking");
			PlayerPrefs.SetInt ("first", 0);//ローカルデータに保存されるランキング
			PlayerPrefs.SetInt ("second", 0);//この方法で音量などのデータも保存される予定
			PlayerPrefs.SetInt ("third", 0);
			PlayerPrefs.SetInt ("fourth", 0);
			PlayerPrefs.SetInt ("fifth", 0);
		}
		Init();
		//Debug.Log ("start");
	}
		
	void Init()//初期化
	{
		prefNum = -1;
		isGameOver = false;
		prefMove.isMoves.Clear();
		StartCoroutine(StateReset());
		pivotHeight = 3;
	}

	void Update () {		 
	if (isGameOver)
		{
			Debug.Log("gameover");
			Checkscore ();
			start_button.SetActive (true);
			title_button.SetActive (true);
			best_string.SetActive (true);
			title_button.SetActive (true);
			score_text.text = prefNum.ToString ();
			best_text.text = PlayerPrefs.GetInt ("first").ToString ();
			best_string.SetActive (true);
			score_string.SetActive (true);
			tweet.SetActive (true);
			if (!isGameOver) {
				Init ();
			}
		}

		if (CheckMove(prefMove.isMoves))
		{
			return;//動いている物体がある時はここで判定中断、また上から判定
		}

		
		if (!isGene)
		{
			StartCoroutine(GeneratePref());//こルーチンを動かす。
			isGene = true;
			return;
		}

		Vector2 v = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, pivotHeight);

		if (Input.GetMouseButtonUp(0))//もしマウス左クリックが離されたら->落とす
		{
			if (!RotateButton.onButtonDown)//ボタンをクリックしていたら反応させない
			{
				genePref.transform.position = v;
				genePref.GetComponent<Rigidbody2D>().isKinematic = false;//落下挙動を開始
				Invoke("display_prefname",1.0f);
				//prefNum++;ここじゃないほうがいいかも->ismoveへ
				isFall = true;//落ちているという判定数
			}
			RotateButton.onButtonDown = false;
		}
		else if(Input.GetMouseButton(0))//ボタンが押されている間つまりどこに落とすか操作している時
		{
			genePref.transform.position = v;
		}
	}


	public void RemovetoGame(){

		prefNum = 0;
		isGameOver = false;
		pivotHeight = 3;

		SceneManager.LoadScene ("PrefTowerBattle");
	}

	public void MovetoTitieScene(){
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
		SceneManager.LoadScene ("TitieScene");
	}

	IEnumerator StateReset()
	{
		while (!isGameOver)
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
		genePref = Instantiate(prefs[Random.Range(0, prefs.Length)], new Vector2(0, pivotHeight), Quaternion.identity);//回転せずに生成
		genePref.GetComponent<Rigidbody2D>().isKinematic = true;//物理挙動をさせない状態にする
	}
		
	public void RotatePref()
	{
		if(!isFall)
			genePref.transform.Rotate(0,0,-30);//30度ずつ回転
	}


	bool CheckMove(List<Moving> isMoves)
	{
		if (isMoves == null)
		{
			Debug.Log ("assd");//なんか知らんがここにはこない?
			return false;
		}
		foreach (Moving b in isMoves)
		{
			if (b.isMove)
			{
				Debug.Log ("move true");
				return true;//移動中
			}
		}
		Debug.Log ("move false");
		if (!isGene) {
			prefNum++;
			Debug.Log ("move false & !isGene" + "" + prefNum.ToString ());
		}
		return false;
	}
		
	void display_prefname(){
		string tmp = "abc";
		if (genePref.name.Contains ("aiti"))
			tmp = "愛知県　名古屋";
		else if (genePref.name.Contains ("akita"))
			tmp = "秋田県　秋田";
		else if (genePref.name.Contains ("aomori"))
			tmp = "青森県 青森";
		else if (genePref.name.Contains ("ehime"))
			tmp = "愛媛県 松山";
		else if (genePref.name.Contains ("fukui"))
			tmp = "福井県 福井";
		else if (genePref.name.Contains ("fukuoka"))
			tmp = "福岡県　福岡";
		else if (genePref.name.Contains ("fukusima"))
			tmp = "福島県 福島";
		else if (genePref.name.Contains ("gifu"))
			tmp = "岐阜県 岐阜";
		else if (genePref.name.Contains ("gunma"))
			tmp = "群馬県 前橋";
		else if (genePref.name.Contains ("hirosima"))
			tmp = "広島県 広島";
		else if (genePref.name.Contains ("hokkaido"))
			tmp = "北海道 札幌";
		else if (genePref.name.Contains ("hyougo"))
			tmp = "兵庫県 神戸";
		else if (genePref.name.Contains ("ibaraki"))
			tmp = "茨城県 水戸";
		else if (genePref.name.Contains ("isikawa"))
			tmp = "石川県 金沢";
		else if (genePref.name.Contains ("iwate"))
			tmp = "岩手県 盛岡";
		else if (genePref.name.Contains ("kagawa"))
			tmp = "香川県 高松";
		else if (genePref.name.Contains ("kanagawa"))
			tmp = "神奈川県 横浜";
		else if (genePref.name.Contains ("kouti"))
			tmp = "高知県 高知";
		else if (genePref.name.Contains ("kumamoto"))
			tmp = "熊本県 熊本";
		else if (genePref.name.Contains ("kyoto"))
			tmp = "京都府 京都";
		else if (genePref.name.Contains ("mie"))
			tmp = "三重県 津";
		else if (genePref.name.Contains ("miyazaki"))
			tmp = "宮崎県 宮崎";
		else if (genePref.name.Contains ("miyagi"))
			tmp = "宮城県 仙台";
		else if (genePref.name.Contains ("nagano"))
			tmp = "長野県 長野";
		else if (genePref.name.Contains ("nagasaki"))
			tmp = "長崎県 長崎";
		else if (genePref.name.Contains ("nara"))
			tmp = "奈良県 奈良";
		else if (genePref.name.Contains ("niigata"))
			tmp = "新潟県 新潟";
		else if (genePref.name.Contains ("okayama"))
			tmp = "岡山県 岡山";
		else if (genePref.name.Contains ("ooita"))
			tmp = "大分県 大分";
		else if (genePref.name.Contains ("osaka"))
			tmp = "大阪府 大阪";
		else if (genePref.name.Contains ("saga"))
			tmp = "佐賀県 佐賀";
		else if (genePref.name.Contains ("saitama"))
			tmp = "埼玉県 さいたま";
		else if (genePref.name.Contains ("siga"))
			tmp = "滋賀県 大津";
		else if (genePref.name.Contains ("sizuoka"))
			tmp = "静岡県 静岡";
		else if (genePref.name.Contains ("tiba"))
			tmp = "千葉県 千葉";
		else if (genePref.name.Contains ("tokusima"))
			tmp = "徳島県 徳島";
		else if (genePref.name.Contains ("totigi"))
			tmp = "栃木県 宇都宮";
		else if (genePref.name.Contains ("tottori"))
			tmp = "鳥取県 鳥取";
		else if (genePref.name.Contains ("toyama"))
			tmp = "富山県 富山";
		else if (genePref.name.Contains ("wakayama"))
			tmp = "和歌山県 和歌山";
		else if (genePref.name.Contains ("yamagata"))
			tmp = "山形県 山形";
		else if (genePref.name.Contains ("yamaguti"))
			tmp = "山口県 山口";
		else if (genePref.name.Contains ("yamanashi"))
			tmp = "山梨県 甲府";
		prefname.text = tmp;
	}

	void Checkscore(){//スコアをチェックする
		if (PlayerPrefs.GetInt ("fifth") < prefNum) {//はじめわ5番目だけ確認すればいい
			if (PlayerPrefs.GetInt ("fourth") > prefNum) {
				Debug.Log ("CALL THIS 1");
				PlayerPrefs.SetInt ("fifth", prefNum);

			} else if (PlayerPrefs.GetInt ("third") > prefNum) {
				Debug.Log ("CALL THIS 2");
				PlayerPrefs.SetInt ("fifth", PlayerPrefs.GetInt ("fourth"));
				PlayerPrefs.SetInt ("fourth", prefNum);
			
			} else if (PlayerPrefs.GetInt ("second") > prefNum) {
				Debug.Log ("CALL THIS 3");
				PlayerPrefs.SetInt ("fifth", PlayerPrefs.GetInt ("fourth"));
				PlayerPrefs.SetInt ("fourth", PlayerPrefs.GetInt ("third"));
				PlayerPrefs.SetInt ("third", prefNum);
			
			} else if (PlayerPrefs.GetInt ("first") > prefNum) {
				Debug.Log ("CALL THISS 4");
				PlayerPrefs.SetInt ("fifth", PlayerPrefs.GetInt ("fourth"));
				PlayerPrefs.SetInt ("fourth", PlayerPrefs.GetInt ("third"));
				PlayerPrefs.SetInt ("third", PlayerPrefs.GetInt ("second"));
				PlayerPrefs.SetInt ("second", prefNum);
			
			} else {
				Debug.Log ("CALL THIS 5");
				PlayerPrefs.SetInt ("fifth", PlayerPrefs.GetInt ("fourth"));
				PlayerPrefs.SetInt ("fourth", PlayerPrefs.GetInt ("third"));
				PlayerPrefs.SetInt ("third", PlayerPrefs.GetInt ("second"));
				PlayerPrefs.SetInt ("second", PlayerPrefs.GetInt ("first"));
				PlayerPrefs.SetInt ("first", prefNum);
			}
		}
		return;
	}
}
