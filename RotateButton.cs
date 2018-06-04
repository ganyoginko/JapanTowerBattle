using UnityEngine;
using UnityEngine.EventSystems;

public class RotateButton : MonoBehaviour, IPointerDownHandler{
//	public static int hensu1=0,hensu2=100;
	public static bool onButtonDown;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		onButtonDown = true;
	}
}