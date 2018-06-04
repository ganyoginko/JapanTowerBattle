using UnityEngine;

public class CameraController : MonoBehaviour {

	public static bool isCollision;
	private void OnTriggerEnter2D(Collider2D collision) //カメラと物体が重なっている間毎フレームよびっ
	{
		isCollision = true;
		//Debug.Log ("CameraController.OntrrigerEnter2D");
	}

	private void OnTriggerExit2D(Collider2D collision) //カメラが物体と重なり終わったら一度呼び出し
	{
		isCollision = false;
		//Debug.Log ("CameraController.OnTriggerExit2D");
	}
}