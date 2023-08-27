using UnityEngine;

public class StatusUpdater : MonoBehaviour
{
	public GUIText status;

	public float updateInterval = 0.5f;

	private bool running;

	private int frames;

	private float lastInterval;

	private void Start()
	{
	}

	private void Update()
	{
		frames++;
		if (running)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup > lastInterval + updateInterval)
			{
				status.text = ((float)frames / (realtimeSinceStartup - lastInterval)).ToString("f2");
				frames = 0;
				lastInterval = realtimeSinceStartup;
			}
		}
	}

	private void startRunningFPS()
	{
		running = true;
		lastInterval = Time.realtimeSinceStartup;
	}
}
