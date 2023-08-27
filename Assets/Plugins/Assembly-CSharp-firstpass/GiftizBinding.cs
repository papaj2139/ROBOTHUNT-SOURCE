using UnityEngine;

public class GiftizBinding
{
	public enum GiftizButtonState
	{
		Invisible = 0,
		Naked = 1,
		Badge = 2,
		Warning = 3
	}

	public static GiftizButtonState giftizButtonState;

	private static AndroidJavaObject _GiftizPlugin;

	static GiftizBinding()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin = new AndroidJavaObject("com.purplebrain.giftiz.sdk.GiftizUnityBinding");
		}
	}

	public static void onResume()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("onResume");
		}
	}

	public static void onPause()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("onPause");
		}
	}

	public static void missionComplete()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("missionComplete");
		}
	}

	public static void inAppPurchase(float amountPayedByUser)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("inAppPurchase", amountPayedByUser);
		}
	}

	public static void getButtonStatus()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("getButtonStatus");
		}
	}

	public static void buttonClicked()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_GiftizPlugin.Call("buttonClicked");
		}
	}
}
