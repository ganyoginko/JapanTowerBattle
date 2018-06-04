using UnityEngine;

public class judge_bar : MonoBehaviour {
	public static bool isCollision;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log ("judge_bar.ontriggerenter");//ここのコメントを取るとうまく言ったりする。
		isCollision = true;
		prefGene.isGameOver = true;
	}
}
