using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
	public enum SwipeDirection
	{
		None = 0,
		Up = 1,
		Down = 2
	}

	public float comfortZone = 70f;

	public float minSwipeDist = 14f;

	public float maxSwipeTime = 0.5f;

	private float startTime;

	private Vector2 startPos;

	private bool couldBeSwipe;

	public SwipeDirection lastSwipe;

	public float lastSwipeTime;

	private void Update()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		Touch touch = Input.touches[0];
		switch (touch.phase)
		{
		case TouchPhase.Began:
			lastSwipe = SwipeDirection.None;
			lastSwipeTime = 0f;
			couldBeSwipe = true;
			startPos = touch.position;
			startTime = Time.time;
			break;
		case TouchPhase.Moved:
			if (Mathf.Abs(touch.position.x - startPos.x) > comfortZone)
			{
				Debug.Log("Not a swipe. Swipe strayed " + (int)Mathf.Abs(touch.position.x - startPos.x) + "px which is " + (int)(Mathf.Abs(touch.position.x - startPos.x) - comfortZone) + "px outside the comfort zone.");
				couldBeSwipe = false;
			}
			break;
		case TouchPhase.Ended:
		{
			if (!couldBeSwipe)
			{
				break;
			}
			float num = Time.time - startTime;
			float magnitude = (new Vector3(0f, touch.position.y, 0f) - new Vector3(0f, startPos.y, 0f)).magnitude;
			if (num < maxSwipeTime && magnitude > minSwipeDist)
			{
				float num2 = Mathf.Sign(touch.position.y - startPos.y);
				if (num2 > 0f)
				{
					lastSwipe = SwipeDirection.Up;
				}
				else if (num2 < 0f)
				{
					lastSwipe = SwipeDirection.Down;
				}
				lastSwipeTime = Time.time;
				Debug.Log("Found a swipe!  Direction: " + lastSwipe);
			}
			break;
		}
		case TouchPhase.Stationary:
			break;
		}
	}
}
