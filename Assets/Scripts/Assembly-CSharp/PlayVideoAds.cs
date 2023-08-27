using UnityEngine;
using UnityEngine.Advertisements;

public class PlayVideoAds : MonoBehaviour
{
	private int randomNumber;

	private void Start()
	{
		Advertisement.Initialize("32339");
	}

	private void Awake()
	{
		int num = Random.Range(1, 4);
		Debug.Log("Random number: " + num);
		if (Advertisement.isReady())
		{
			switch (num)
			{
			case 1:
				Advertisement.Show();
				MonoBehaviour.print("Touch has began on image");
				break;
			case 2:
				AdBuddizBinding.ShowAd();
				break;
			default:
				MonoBehaviour.print("No ads!");
				break;
			}
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		if (level != 0)
		{
		}
	}
}
