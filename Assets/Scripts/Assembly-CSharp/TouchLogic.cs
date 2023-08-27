using UnityEngine;

public class TouchLogic : MonoBehaviour
{
	public static int currTouch;

	private Ray ray;

	private RaycastHit rayHitInfo = default(RaycastHit);

	[HideInInspector]
	public int touch2Watch = 64;

	private void Update()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			currTouch = i;
			Debug.Log(currTouch);
			if (GetComponent<GUITexture>() != null && GetComponent<GUITexture>().HitTest(Input.GetTouch(i).position))
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					SendMessage("OnTouchBegan");
				}
				if (Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					SendMessage("OnTouchEnded");
				}
				if (Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					SendMessage("OnTouchMoved");
				}
				if (Input.GetTouch(i).phase == TouchPhase.Stationary)
				{
					SendMessage("OnTouchStayed");
				}
			}
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
			switch (Input.GetTouch(i).phase)
			{
			case TouchPhase.Began:
				SendMessage("OnTouchBeganAnyWhere");
				if (Physics.Raycast(ray, out rayHitInfo))
				{
					rayHitInfo.transform.gameObject.SendMessage("OnTouchBegan3D");
				}
				break;
			case TouchPhase.Ended:
				SendMessage("OnTouchEndedAnywhere");
				if (Physics.Raycast(ray, out rayHitInfo))
				{
					rayHitInfo.transform.gameObject.SendMessage("OnTouchEnded3D");
				}
				break;
			case TouchPhase.Moved:
				SendMessage("OnTouchMovedAnywhere");
				if (Physics.Raycast(ray, out rayHitInfo))
				{
					rayHitInfo.transform.gameObject.SendMessage("OnTouchMoved3D");
				}
				break;
			case TouchPhase.Stationary:
				SendMessage("OnTouchStayedAnywhere");
				if (Physics.Raycast(ray, out rayHitInfo))
				{
					rayHitInfo.transform.gameObject.SendMessage("OnTouchStayed3D");
				}
				break;
			}
		}
	}
}
