using UnityEngine;

public class Joystick : TouchLogic
{
	public enum JoystickType
	{
		Movement = 0,
		LookRotation = 1,
		SkyColor = 2
	}

	public JoystickType joystickType;

	public Transform player;

	public float playerSpeed = 2f;

	public float maxJoyDelta = 0.05f;

	public float rotateSpeed = 100f;

	private Vector3 oJoyPos;

	private Vector3 joyDelta;

	private Transform joyTrans;

	public CharacterController troller;

	private float pitch;

	private float yaw;

	private Vector3 oRotation;

	private void Start()
	{
		joyTrans = base.transform;
		oJoyPos = joyTrans.position;
		oRotation = player.eulerAngles;
		pitch = oRotation.x;
		yaw = oRotation.y;
	}

	private void OnTouchBegan()
	{
		touch2Watch = TouchLogic.currTouch;
	}

	private void OnTouchMovedAnywhere()
	{
		if (TouchLogic.currTouch == touch2Watch)
		{
			joyTrans.position = MoveJoyStick();
			ApplyDeltaJoy();
		}
	}

	private void OnTouchStayedAnywhere()
	{
		if (TouchLogic.currTouch == touch2Watch)
		{
			ApplyDeltaJoy();
		}
	}

	private void OnTouchEndedAnywhere()
	{
		if (TouchLogic.currTouch == touch2Watch || Input.touches.Length <= 0)
		{
			joyTrans.position = oJoyPos;
			touch2Watch = 64;
		}
	}

	private void ApplyDeltaJoy()
	{
		switch (joystickType)
		{
		case JoystickType.Movement:
			troller.Move((player.forward * joyDelta.z + player.right * joyDelta.x) * playerSpeed * Time.deltaTime);
			break;
		case JoystickType.LookRotation:
			pitch -= Input.GetTouch(touch2Watch).deltaPosition.y * rotateSpeed * Time.deltaTime;
			yaw += Input.GetTouch(touch2Watch).deltaPosition.x * rotateSpeed * Time.deltaTime;
			pitch = Mathf.Clamp(pitch, -80f, 80f);
			player.eulerAngles += new Vector3(pitch, yaw, 0f);
			break;
		case JoystickType.SkyColor:
			Camera.main.backgroundColor = new Color(joyDelta.x, joyDelta.z, joyDelta.x * joyDelta.z);
			break;
		}
	}

	private Vector3 MoveJoyStick()
	{
		float value = Input.GetTouch(touch2Watch).position.x / (float)Screen.width;
		float value2 = Input.GetTouch(touch2Watch).position.y / (float)Screen.height;
		Vector3 result = new Vector3(Mathf.Clamp(value, oJoyPos.x - maxJoyDelta, oJoyPos.x + maxJoyDelta), Mathf.Clamp(value2, oJoyPos.y - maxJoyDelta, oJoyPos.y + maxJoyDelta), 0f);
		joyDelta = new Vector3(result.x - oJoyPos.x, 0f, result.y - oJoyPos.y).normalized;
		return result;
	}

	private void LateUpdate()
	{
		if (!troller.isGrounded)
		{
			troller.Move(Vector3.down * 2f);
		}
	}
}
