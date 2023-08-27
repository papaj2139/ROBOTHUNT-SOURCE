using System.Collections;
using UnityEngine;

public class FingerManager : MonoBehaviour
{
	public ArrayList elements = new ArrayList();

	public ArrayList fingers = new ArrayList();

	public void AddFingerListener(GUIElement element)
	{
		elements.Add(element);
	}

	public void RemoveFingerListener(GUIElement element)
	{
		elements.Remove(element);
	}

	private void Update()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began)
			{
				Finger finger = new Finger();
				finger.touch = touch;
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit[] array = Physics.RaycastAll(ray);
				RaycastHit[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					RaycastHit raycastHit = array2[j];
					finger.colliders.Add(raycastHit.collider);
					GameObject gameObject = raycastHit.collider.gameObject;
					gameObject.SendMessage("FingerBegin", touch, SendMessageOptions.DontRequireReceiver);
				}
				foreach (GUITexture element in elements)
				{
					if (element.HitTest(touch.position))
					{
						finger.elements.Add(element);
						GameObject gameObject2 = element.gameObject;
						gameObject2.SendMessage("FingerBegin", touch, SendMessageOptions.DontRequireReceiver);
					}
				}
				fingers.Add(finger);
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				for (int k = 0; k < fingers.Count; k++)
				{
					Finger finger2 = (Finger)fingers[k];
					if (finger2.touch.fingerId != touch.fingerId)
					{
						continue;
					}
					finger2.moved = true;
					foreach (Collider collider3 in finger2.colliders)
					{
						if (!(collider3 == null))
						{
							GameObject gameObject3 = collider3.gameObject;
							gameObject3.SendMessage("FingerMove", touch, SendMessageOptions.DontRequireReceiver);
						}
					}
					foreach (GUITexture element2 in finger2.elements)
					{
						if (!(element2 == null))
						{
							GameObject gameObject4 = element2.gameObject;
							gameObject4.SendMessage("FingerMove", touch, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					continue;
				}
				Ray ray2 = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit[] array = Physics.RaycastAll(ray2);
				int num = 0;
				while (num < fingers.Count)
				{
					Finger finger3 = (Finger)fingers[num];
					if (finger3.touch.fingerId == touch.fingerId)
					{
						foreach (Collider collider4 in finger3.colliders)
						{
							if (collider4 == null)
							{
								continue;
							}
							bool flag = true;
							RaycastHit[] array3 = array;
							foreach (RaycastHit raycastHit2 in array3)
							{
								if (raycastHit2.collider == collider4)
								{
									flag = false;
									GameObject gameObject5 = collider4.gameObject;
									gameObject5.SendMessage("FingerEnd", touch, SendMessageOptions.DontRequireReceiver);
								}
							}
							if (flag)
							{
								GameObject gameObject6 = collider4.gameObject;
								gameObject6.SendMessage("FingerCancel", touch, SendMessageOptions.DontRequireReceiver);
							}
						}
						foreach (GUITexture element3 in finger3.elements)
						{
							if (!(element3 == null))
							{
								bool flag2 = true;
								if (element3.HitTest(touch.position))
								{
									flag2 = false;
									GameObject gameObject7 = element3.gameObject;
									gameObject7.SendMessage("FingerEnd", touch, SendMessageOptions.DontRequireReceiver);
								}
								if (flag2)
								{
									GameObject gameObject8 = element3.gameObject;
									gameObject8.SendMessage("FingerCancel", touch, SendMessageOptions.DontRequireReceiver);
								}
							}
						}
						fingers[num] = fingers[fingers.Count - 1];
						fingers.RemoveAt(fingers.Count - 1);
					}
					else
					{
						num++;
					}
				}
			}
		}
	}
}
