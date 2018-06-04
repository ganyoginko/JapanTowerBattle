using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using NCMB;
public class regname : MonoBehaviour {
	public InputField inputField;
	string inputValue;
	public GameObject GOB_inputfield;
	public Text PlaceHolder;

	public Text [] nameandscore;
	public GameObject bestsign;
	public GameObject start_messe;
	public GameObject option_messe;
	public GameObject net_messe;
	public Text myranktext;
	void Start(){
		Debug.Log ("regname_start");
		if (PlayerPrefs.HasKey ("name"))
			Destroy (inputField);
		//inputField = GetComponent<InputField> ();
	}

	public void InitInputField(){
		inputField.ActivateInputField();
	}

	public void EndInput(){
		inputValue = inputField.text;
		Debug.Log ("input is " + inputValue);
		//InitInputField ();
		//ここでダブりチェック
		if(inputValue.Length > 7 || inputValue.Length < 2 ) {
			inputField.text = " ";
			InitInputField();
			return;
		}

		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking"); // NCMB上のScoreRankingクラスを取得	
		query.WhereEqualTo ("playername", inputValue); // プレイヤー名でデータを絞る//ここでqueryの設定
		query.FindAsync ((List<NCMBObject> objList,NCMBException e) => {
			if (e == null) { // データの検索が成功したら、
				if( objList.Count == 0 ){ // ハイスコアが未登録の場合
					Debug.Log("cannot find the name");
					PlayerPrefs.SetString("name",inputValue);
					GOB_inputfield.SetActive(false);
					Destroy(inputField);
				}else{ // ハイスコアが登録済みの場合
					Debug.Log("can find the name,one more");
					PlaceHolder.text = "登録済みの名前だった！";
					inputField.text = "";
					InitInputField();
				}
			}
		});
	}

	public void push_name(){
		if (!PlayerPrefs.HasKey ("name")) {
			if (!GOB_inputfield.activeSelf) {
				GOB_inputfield.SetActive (true);
				InitInputField ();
			} else
				GOB_inputfield.SetActive (false);
		} else
			Debug.Log ("You have name, cant chandge");
	}

	public void OnButtonRanking(){		

		if (!bestsign.activeSelf) {
			if (!PlayerPrefs.HasKey ("name")) {
				push_name ();
				PlaceHolder.text = "名前を決めてね！";
			} else {
				Debug.Log ("have name :" + PlayerPrefs.GetString ("name"));
				saveScoreRanking (PlayerPrefs.GetString ("name"), PlayerPrefs.GetInt ("first"));
			}
		} else
			DisplayRunking ();		
	}

	public void saveScoreRanking(string playerName, int score){
		Debug.Log ("saveScoreRanking");
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking"); // NCMB上のScoreRankingクラスを取得	
		query.WhereEqualTo ("playername", playerName); // プレイヤー名でデータを絞る//ここでqueryの設定
		query.FindAsync ((List<NCMBObject> objList,NCMBException e) => {
			if (e == null) { // データの検索が成功したら、
				if( objList.Count == 0 ){ // ハイスコアが未登録の場合
					Debug.Log("cannot find your date");
					NCMBObject cloudObj = new NCMBObject("Ranking");
					cloudObj ["playername"] = playerName;
					cloudObj ["score"] = score;
					cloudObj.SaveAsync(); // セーブ
				}else{ // ハイスコアが登録済みの場合
					Debug.Log("find your date,update");
					int cloudScore = System.Convert.ToInt32(objList[0]["score"]); // クラウド上のスコアを取得
					if(score > cloudScore){ // 今プレイしたスコアの方が高かったら、
						objList[0]["score"] = score; // それを新しいスコアとしてデータを更新
						objList[0].SaveAsync(); // セーブ
					}
				}
			}
			getScoreRanking ();
		});
	}

	public void getScoreRanking(){
		Debug.Log ("getScoreRanking");
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
		query.OrderByDescending ("score"); // スコアを降順に並び替える
		query.Limit = 10; // 上位10件のみ取得
		List<string> nameList = new List<string>(); // 名前のリスト
		List<int> scoreList = new List<int>(); // スコアのリスト
		query.FindAsync ((List<NCMBObject> objList ,NCMBException e) => {//ラムダ式同時処理
			if(e == null){ //検索成功したら
				for(int i = 0; i < objList.Count; i++){
					string s = System.Convert.ToString(objList[i]["playername"]); // 名前を取得
					int n = System.Convert.ToInt32(objList[i]["score"]); // スコアを取得
					nameList.Add(s); // リストに突っ込む
					scoreList.Add(n);
					Debug.Log("name is" + nameList[i] + "score is" + scoreList[i] );
					nameandscore[i].text = nameList[i]  + " " + scoreList[i];
				}
			}
			DisplayRunking();
			SearchMyRank(PlayerPrefs.GetString("name"));
		});							
	}
		
	public void DisplayRunking(){
		if (!bestsign.activeSelf) {
			bestsign.SetActive (true);
			start_messe.SetActive (false);
			option_messe.SetActive (false);
			net_messe.SetActive (false);
			
		} else {
			bestsign.SetActive (false);
			start_messe.SetActive (true);
			option_messe.SetActive (true);
			net_messe.SetActive (true);
			myranktext.text = " ";
		}
	}

	void SearchMyRank(string myname){
		int myrank = 0;
		NCMBQuery<NCMBObject> query= new NCMBQuery<NCMBObject> ("Ranking");
		query.OrderByDescending ("score");
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			if (e == null){
				for (int i = 0; i < objList.Count; i++) {
					if (objList [i] ["playername"] == myname){
						myrank = i;
						break;	
					}
				}
			}
			DisplayMyRank(myrank);
		});
	}

	void DisplayMyRank(int rank){
		Debug.Log ("DisplayMyrank");
		myranktext.text = "あなたは\n" + (rank+1).ToString () + "番!!";
	}
}