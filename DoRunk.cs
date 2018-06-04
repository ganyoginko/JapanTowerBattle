using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
public class DoRunk : MonoBehaviour {
	public Text [] nameandscore;
	public GameObject bestsign;
	public GameObject start_messe;
	public GameObject option_messe;
	public GameObject net_messe;

	public void OnButtonRanking(){		

		if (!PlayerPrefs.HasKey ("name")) {
			//regname.checkname ();
		} else {
			Debug.Log ("have name :" + PlayerPrefs.GetString ("name"));
			saveScoreRanking (PlayerPrefs.GetString ("name"), PlayerPrefs.GetInt ("first"));
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		}
	}
}