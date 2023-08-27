using UnityEngine;

public class AdBuddizExample : MonoBehaviour
{
	private void Start()
	{
		AdBuddizBinding.SetLogLevel(AdBuddizBinding.ABLogLevel.Info);
		AdBuddizBinding.SetAndroidPublisherKey("TEST_PUBLISHER_KEY_ANDROID");
		AdBuddizBinding.SetIOSPublisherKey("TEST_PUBLISHER_KEY_IOS");
		AdBuddizBinding.SetTestModeActive();
		AdBuddizBinding.CacheAds();
	}

	public void OnGUI()
	{
		if (GUI.Button(new Rect(40f, 40f, Screen.width - 80, 80f), "Show Ad"))
		{
			AdBuddizBinding.ShowAd();
		}
	}

	private void OnEnable()
	{
		AdBuddizManager.didFailToShowAd += DidFailToShowAd;
		AdBuddizManager.didCacheAd += DidCacheAd;
		AdBuddizManager.didShowAd += DidShowAd;
		AdBuddizManager.didClick += DidClick;
		AdBuddizManager.didHideAd += DidHideAd;
	}

	private void OnDisable()
	{
		AdBuddizManager.didFailToShowAd -= DidFailToShowAd;
		AdBuddizManager.didCacheAd -= DidCacheAd;
		AdBuddizManager.didShowAd -= DidShowAd;
		AdBuddizManager.didClick -= DidClick;
		AdBuddizManager.didHideAd -= DidHideAd;
	}

	private void DidFailToShowAd(string adBuddizError)
	{
		AdBuddizBinding.LogNative("DidFailToShowAd: " + adBuddizError);
		Debug.Log("Unity: DidFailToShowAd: " + adBuddizError);
	}

	private void DidCacheAd()
	{
		AdBuddizBinding.LogNative("DidCacheAd");
		Debug.Log("Unity: DidCacheAd");
	}

	private void DidShowAd()
	{
		AdBuddizBinding.LogNative("DidShowAd");
		Debug.Log("Unity: DidShowAd");
	}

	private void DidClick()
	{
		AdBuddizBinding.LogNative("DidClick");
		Debug.Log("Unity: DidClick");
	}

	private void DidHideAd()
	{
		AdBuddizBinding.LogNative("DidHideAd");
		Debug.Log("Unity: DidHideAd");
	}
}
