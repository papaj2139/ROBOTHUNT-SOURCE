using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EpicCitadelControl : MonoBehaviour
{
	public bool kJoystikEnabled = true;

	public float kJoystickSpeed = 0.5f;

	public bool kInverse;

	public float kMovementSpeed = 10f;

	private Transform ownTransform;

	private Transform cameraTransform;

	private CharacterController characterController;

	private Camera _camera;

	private int leftFingerId = -1;

	private int rightFingerId = -1;

	private Vector2 leftFingerStartPoint;

	private Vector2 leftFingerCurrentPoint;

	private Vector2 rightFingerStartPoint;

	private Vector2 rightFingerCurrentPoint;

	private Vector2 rightFingerLastPoint;

	private bool isRotating;

	private bool isMovingToTarget;

	private Vector3 targetPoint;

	private Rect joystickRect;

	private void MoveFromJoystick()
	{
		isMovingToTarget = false;
		Vector2 vector = leftFingerCurrentPoint - leftFingerStartPoint;
		if (vector.magnitude > 10f)
		{
			vector = vector.normalized * 10f;
		}
		characterController.SimpleMove(kJoystickSpeed * ownTransform.TransformDirection(new Vector3(vector.x, 0f, vector.y)));
	}

	private void MoveToTarget()
	{
		Vector3 vector = targetPoint - ownTransform.position;
		characterController.SimpleMove(vector.normalized * kMovementSpeed);
		if (new Vector3(vector.x, 0f, vector.z).magnitude < 0.1f)
		{
			isMovingToTarget = false;
		}
	}

	private void SetTarget(Vector2 screenPos)
	{
		Ray ray = _camera.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y));
		int layerMask = 256;
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMask))
		{
			targetPoint = hitInfo.point;
			isMovingToTarget = true;
		}
	}

	private void OnTouchBegan(int fingerId, Vector2 pos)
	{
		if (leftFingerId == -1 && kJoystikEnabled && joystickRect.Contains(pos))
		{
			leftFingerId = fingerId;
			leftFingerStartPoint = (leftFingerCurrentPoint = pos);
		}
		else if (rightFingerId == -1)
		{
			rightFingerStartPoint = (rightFingerCurrentPoint = (rightFingerLastPoint = pos));
			rightFingerId = fingerId;
			isRotating = false;
		}
	}

	private void OnTouchEnded(int fingerId)
	{
		if (fingerId == leftFingerId)
		{
			leftFingerId = -1;
		}
		else if (fingerId == rightFingerId)
		{
			rightFingerId = -1;
			if (!isRotating)
			{
				SetTarget(rightFingerStartPoint);
			}
		}
	}

	private void OnTouchMoved(int fingerId, Vector2 pos)
	{
		if (fingerId == leftFingerId)
		{
			leftFingerCurrentPoint = pos;
		}
		else if (fingerId == rightFingerId)
		{
			rightFingerCurrentPoint = pos;
			if ((pos - rightFingerStartPoint).magnitude > 2f)
			{
				isRotating = true;
			}
		}
	}

	private void Start()
	{
		joystickRect = new Rect((float)Screen.width * 0.02f, (float)Screen.height * 0.02f, (float)Screen.width * 0.2f, (float)Screen.height * 0.2f);
		ownTransform = base.transform;
		cameraTransform = Camera.main.transform;
		characterController = GetComponent<CharacterController>();
		_camera = Camera.main;
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			if (Input.GetMouseButtonDown(0))
			{
				OnTouchBegan(0, Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				OnTouchEnded(0);
			}
			else if (leftFingerId != -1 || rightFingerId != -1)
			{
				OnTouchMoved(0, Input.mousePosition);
			}
		}
		else
		{
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					OnTouchBegan(touch.fingerId, touch.position);
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					OnTouchMoved(touch.fingerId, touch.position);
				}
				else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
				{
					OnTouchEnded(touch.fingerId);
				}
			}
		}
		if (leftFingerId != -1)
		{
			MoveFromJoystick();
		}
		else if (isMovingToTarget)
		{
			MoveToTarget();
		}
		if (rightFingerId != -1 && isRotating)
		{
			Rotate();
		}
	}

	private void Rotate()
	{
		Vector3 direction = _camera.ScreenPointToRay(rightFingerLastPoint).direction;
		Vector3 direction2 = _camera.ScreenPointToRay(rightFingerCurrentPoint).direction;
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(direction, direction2);
		ownTransform.rotation *= Quaternion.Euler(0f, (!kInverse) ? (0f - quaternion.eulerAngles.y) : quaternion.eulerAngles.y, 0f);
		quaternion.SetFromToRotation(cameraTransform.InverseTransformDirection(direction), cameraTransform.InverseTransformDirection(direction2));
		cameraTransform.localRotation = Quaternion.Euler((!kInverse) ? (0f - quaternion.eulerAngles.x) : quaternion.eulerAngles.x, 0f, 0f) * cameraTransform.localRotation;
		rightFingerLastPoint = rightFingerCurrentPoint;
	}
}
