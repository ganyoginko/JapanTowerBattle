using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class moveTogame : MonoBehaviour {
	public GameObject start_messe;
	public GameObject option_messe;
	public GameObject net_messe;

	public Text myname;

	void Start(){
	}

	void Update(){
		if (PlayerPrefs.HasKey ("name")) {
			Debug.Log ("you has name");
			myname.text = PlayerPrefs.GetString ("name");
		}
	}

	public void movetogame(){
		SceneManager.LoadScene ("PrefTowerBattle");
	}

	public void debug_reset(){
		PlayerPrefs.DeleteKey ("name");
		PlayerPrefs.DeleteKey ("first");
	}

	public void Deleterunking(){
		PlayerPrefs.DeleteAll ();
	}

	public void push_photontest(){
		SceneManager.LoadScene ("testlobby");
	}

	public void push_opotion(){
		if (start_messe.activeSelf) {
			start_messe.SetActive (false);
			option_messe.SetActive (true);
			net_messe.SetActive (false);
		} else {
			start_messe.SetActive (true);
			option_messe.SetActive (false);
			net_messe.SetActive (true);
		}
	}
}