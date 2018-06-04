using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class lobby : MonoBehaviour {
	
	void Start () {
		Debug.Log ("lobbyStart()");
		PhotonNetwork.playerName = PlayerPrefs.GetString("name");//同じ名前にすると狂う
		PhotonNetwork.ConnectUsingSettings (null);

	}

	void OnFailedToConnectToPhoton(){
		Debug.Log ("Onfailedtoconnecttophoton");
		PhotonNetwork.ConnectUsingSettings (null);
	}

	void OnJoinedLobby(){
		Debug.Log ("OnJoinLobby");
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log ("OnPhotonRamdomJoinFailed");
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.isVisible = true;
		roomOptions.isOpen = true;
		roomOptions.maxPlayers = 4;

		PhotonNetwork.CreateRoom (PlayerPrefs.GetString("name"),roomOptions,null);
	}

	void OnJoinedRoom(){
		Debug.Log ("OnJoinedRoom");
	}

	void OnPhotonPlayerConnected(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("VsNet2nd");
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (PhotonNetwork.room.playerCount);
		if (PhotonNetwork.room.playerCount == 2) { //ルームが作られる前まではエラーが出る60フレームくらい
			//if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "lobbyscene")
			//	UnityEngine.SceneManagement.SceneManager.LoadScene ("VsNet2nd");
			//else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "testlobby")
				PhotonNetwork.sendRate = 1;
				PhotonNetwork.sendRateOnSerialize = 1;
				Debug.Log ("VSNet");
				UnityEngine.SceneManagement.SceneManager.LoadScene ("VSNet");
		}
	}
		
	public void MovetoTitieScene(){
		PhotonNetwork.Disconnect();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("TitieScene");
	}

}