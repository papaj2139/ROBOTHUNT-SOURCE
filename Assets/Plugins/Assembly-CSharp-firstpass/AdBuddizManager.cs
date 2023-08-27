using UnityEngine;

public class AdBuddizManager : MonoBehaviour
{
	public delegate void DidCacheAd();

	public delegate void DidShowAd();

	public delegate void DidFailToShowAd(string adBuddizError);

	public delegate void DidClick();

	public delegate void DidHideAd();

	public static event DidCacheAd didCacheAd;

	public static event DidShowAd didShowAd;

	public static event DidFailToShowAd didFailToShowAd;

	public static event DidClick didClick;

	public static event DidHideAd didHideAd;

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			AdBuddizBinding.CacheAds();
		}
	}

	public void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	public void OnDidFailToShowAd(string adBuddizError)
	{
		if (AdBuddizManager.didFailToShowAd != null)
		{
			AdBuddizManager.didFailToShowAd(adBuddizError);
		}
	}

	public void OnDidCacheAd()
	{
		if (AdBuddizManager.didCacheAd != null)
		{
			AdBuddizManager.didCacheAd();
		}
	}

	public void OnDidShowAd()
	{
		if (AdBuddizManager.didShowAd != null)
		{
			AdBuddizManager.didShowAd();
		}
	}

	public void OnDidClick()
	{
		if (AdBuddizManager.didClick != null)
		{
			AdBuddizManager.didClick();
		}
	}

	public void OnDidHideAd()
	{
		if (AdBuddizManager.didHideAd != null)
		{
			AdBuddizManager.didHideAd();
		}
	}
}
