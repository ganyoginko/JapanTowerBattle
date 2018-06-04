using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkbest : MonoBehaviour {
	public Text text;

	// Use this for initialization
	void Start () {
		text.text = PlayerPrefs.GetInt("first").ToString();
	}

	// Update is called once per frame
	void Update () {
		
	}
}