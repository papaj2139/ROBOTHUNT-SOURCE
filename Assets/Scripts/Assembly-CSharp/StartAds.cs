using UnityEngine;

public class StartAds : MonoBehaviour
{
	private void Start()
	{
		AdBuddizBinding.SetAndroidPublisherKey("64c4ab08-a22d-4df0-a21d-06f3d4a9ee8b");
		AdBuddizBinding.CacheAds();
	}

	private void Update()
	{
	}
}
