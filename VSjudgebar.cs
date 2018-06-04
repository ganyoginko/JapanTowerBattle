using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSjudgebar : MonoBehaviour {
	public static bool isCollision;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log ("judge_bar.ontriggerenter");//ここのコメントを取るとうまく言ったりする。
		isCollision = true;
		VSPrefGene.isGameOver = true;
		
	}
}