using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefMove : MonoBehaviour {
		public static List<Moving> isMoves = new List<Moving>();//移動してる動物がいないかチェックするリスト

		Rigidbody2D rigid;
		Moving moving = new Moving();
		private void Start()
		{
			rigid = GetComponent<Rigidbody2D>();
			isMoves.Add(moving);
		}

		
		void FixedUpdate () {
			if(rigid.velocity.magnitude > 0.01f)
			{
				
				moving.isMove = true;
			}
			else
			{
				moving.isMove = false;
			}
		}
}
public class Moving
{
	public bool isMove;
}