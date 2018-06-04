using UnityEngine;
using System.Collections;

public class NetWork_01 : MonoBehaviour {

	public void connect() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("lobbyscene");
	}
	/*
	void OnJoinedLobby() {
		Debug.Log("ロビーに入りました。");
		// ルームに入室する
		UnityEngine.SceneManagement.SceneManager.LoadScene("lobbyscene");
	}
		
	void OnJoinedRoom() {
		Debug.Log("ルームへ入室しました。");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("VSnet");
	}

	
	void OnPhotonRandomJoinFailed() {
		Debug.Log("ルームの入室に失敗しました。");
		// ルームがないと入室に失敗するため、その時は自分で作る
		// 引数でルーム名を指定できる
		PhotonNetwork.CreateRoom("myRoomName");
	}*/
}