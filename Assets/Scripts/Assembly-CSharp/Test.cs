using UnityEngine;

public class Test : MonoBehaviour
{
	public static Rect m_screenRect = new Rect(0f, 0f, 640f, 960f);

	public Texture2D m_textureInvisible;

	public Texture2D m_textureNaked;

	public Texture2D m_textureBadge;

	public Texture2D m_textureWarning;

	public GUIStyle m_styleButton;

	public Rect m_giftizButton;

	public static Rect UpdateDimension(Rect position, float dy = 0f)
	{
		position.y += dy;
		return new Rect(position.x / m_screenRect.width * (float)Screen.width, position.y / m_screenRect.height * (float)Screen.height, position.width / m_screenRect.width * (float)Screen.width, position.height / m_screenRect.height * (float)Screen.height);
	}

	public void OnGUI()
	{
		Rect rect = new Rect(m_screenRect.width / 2f - 250f, 200f, 500f, 100f);
		GUIStyleState normal = m_styleButton.normal;
		Texture2D giftizButtonTexture = GetGiftizButtonTexture();
		m_styleButton.active.background = giftizButtonTexture;
		normal.background = giftizButtonTexture;
		if (GUI.Button(UpdateDimension(m_giftizButton), string.Empty, m_styleButton))
		{
			GiftizBinding.buttonClicked();
		}
	}

	public Texture2D GetGiftizButtonTexture()
	{
		Texture2D result = null;
		switch (GiftizBinding.giftizButtonState)
		{
		case GiftizBinding.GiftizButtonState.Invisible:
			result = m_textureInvisible;
			break;
		case GiftizBinding.GiftizButtonState.Naked:
			result = m_textureNaked;
			break;
		case GiftizBinding.GiftizButtonState.Badge:
			result = m_textureBadge;
			break;
		case GiftizBinding.GiftizButtonState.Warning:
			result = m_textureWarning;
			break;
		}
		return result;
	}
}
