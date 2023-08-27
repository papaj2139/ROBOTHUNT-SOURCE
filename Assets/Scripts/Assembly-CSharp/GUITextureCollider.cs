using UnityEngine;

[RequireComponent(typeof(FingerManager))]
public class GUITextureCollider : MonoBehaviour
{
	private void Awake()
	{
		FingerManager fingerManager = (FingerManager)Object.FindObjectOfType(typeof(FingerManager));
		fingerManager.SendMessage("AddFingerListener", GetComponent<GUITexture>(), SendMessageOptions.RequireReceiver);
	}

	private void FingerBegin()
	{
		Debug.Log("FingerBegin");
	}

	private void FingerMove()
	{
		Debug.Log("FingerMove");
	}

	private void FingerEnd()
	{
		Debug.Log("FingerEnd");
	}

	private void FingerCancel()
	{
		Debug.Log("FingerCancel");
	}
}
