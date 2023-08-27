using UnityEngine;

public class AdBuddizBinding
{
	public enum ABLogLevel
	{
		Info = 0,
		Error = 1,
		Silent = 2
	}

	private static AndroidJavaObject adBuddizPlugin;

	static AdBuddizBinding()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			adBuddizPlugin = new AndroidJavaObject("com.purplebrain.adbuddiz.sdk.AdBuddizUnityBinding");
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameObject gameObject = GameObject.Find("AdBuddizManager");
			if (gameObject == null)
			{
				new GameObject("AdBuddizManager").AddComponent<AdBuddizManager>();
			}
		}
	}

	public static void SetLogLevel(ABLogLevel level)
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("setLogLevel", level.ToString());
		}
	}

	public static void SetAndroidPublisherKey(string publisherKey)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			adBuddizPlugin.Call("setPublisherKey", publisherKey);
		}
	}

	public static void SetIOSPublisherKey(string publisherKey)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
		}
	}

	public static void SetTestModeActive()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("setTestModeActive");
		}
	}

	public static void CacheAds()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("cacheAds");
		}
	}

	public static bool IsReadyToShowAd()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return adBuddizPlugin.Call<bool>("isReadyToShowAd", new object[0]);
		}
		return false;
	}

	public static bool IsReadyToShowAd(string placementId)
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return adBuddizPlugin.Call<bool>("isReadyToShowAd", new object[1] { placementId });
		}
		return false;
	}

	public static void ShowAd()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("showAd");
		}
	}

	public static void ShowAd(string placementId)
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("showAd", placementId);
		}
	}

	public static void LogNative(string text)
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			adBuddizPlugin.Call("logNative", text);
		}
	}
}
