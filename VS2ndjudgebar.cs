using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VS2ndjudgebar : MonoBehaviour {
	public static bool isCollision;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log ("judge_bar.ontriggerenter");//ここのコメントを取るとうまく言ったりする。
		isCollision = true;
		if (PhotonNetwork.isMasterClient && VSPref2nd.prefNum%2 == 0)
			VSPref2nd.loose = true;
		if (PhotonNetwork.isMasterClient && VSPref2nd.prefNum % 2 == 1)
			VSPref2nd.win = true;
	}
}