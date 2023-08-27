using UnityEngine;

public class SwipeRotationPlayer : TouchLogic
{
	public float rotateSpeed = 15f;

	public int invertPitch = 1;

	public Transform player;

	private float pitch;

	private float yaw;

	private Vector3 oRotation;

	private void Start()
	{
		oRotation = player.eulerAngles;
		pitch = oRotation.x;
		yaw = oRotation.y;
	}

	private void OnTouchBegan()
	{
		touch2Watch = TouchLogic.currTouch;
	}

	public void OnTouchMoved()
	{
		pitch -= Input.GetTouch(touch2Watch).deltaPosition.y * rotateSpeed * (float)invertPitch * Time.deltaTime;
		yaw += Input.GetTouch(touch2Watch).deltaPosition.x * rotateSpeed * (float)invertPitch * Time.deltaTime;
		pitch = Mathf.Clamp(pitch, -30f, 30f);
		player.eulerAngles = new Vector3(pitch, yaw, 0f);
	}

	private void OnTouchEndedAnywhere()
	{
		if (TouchLogic.currTouch == touch2Watch || Input.touches.Length <= 0)
		{
			touch2Watch = 64;
		}
	}
}
