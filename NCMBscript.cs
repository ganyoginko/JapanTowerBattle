using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBscript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		NCMBObject testClass = new NCMBObject("Runking");

		// オブジェクトに値を設定

		testClass["score"] = 2;
		testClass ["fasng"] = 9;
		testClass ["name"] = "gige";
		// データストアへの登録
		testClass.SaveAsync();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}