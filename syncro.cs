using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncro : Photon.MonoBehaviour {
	
	[PunRPC]
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log ("OnphotonSerrializeView");
		if (stream.isWriting) {
			//データの送信
			stream.SendNext(VSPrefGene.prefNum);																																				
			Debug.Log ("send"+VSPrefGene.prefNum.ToString());
		} else {
			//データの受信
			VSPrefGene.prefNum = (int)stream.ReceiveNext();
			Debug.Log ("receive"+VSPrefGene.prefNum.ToString());
		}
	}
}
