using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBrain : MonoBehaviour
{
	public GameObject prefab;

	private static TerrainBrain m_instance;

	public static float noiseMultiplier = 25f;

	public static int chunkSize = 10;

	private Vector3 playerStart = new Vector3(0.5f, 30.5f, 0.5f);

	private TerrainCache m_tcache = new TerrainCache();

	public Texture2D[] textures;

	private Texture2D groundTexture;

	private Rect[] groundUVs;

	private Vector3 currentPos = Vector3.zero;

	private Vector3 lastPos = Vector3.zero;

	private float viewDistance = 50f;

	private int blockLoadDistance;

	private Queue<Vector3> m_terrainToCreate = new Queue<Vector3>();

	private GameObject[,,] m_meshCache;

	private int m_loaded = 5;

	private int[] A = new int[3];

	private float s;

	private float u;

	private float v;

	private float w;

	private int i;

	private int j;

	private int k;

	private float onethird = 1f / 3f;

	private float onesixth = 1f / 6f;

	private int[] T;

	public static TerrainBrain Instance()
	{
		if (m_instance == null)
		{
			Debug.LogWarning("Lost terrain brain, reaquiring.");
			GameObject gameObject = GameObject.Find("TerrainManager");
			if (gameObject == null)
			{
				Debug.LogError("Could not reaquire terrain brain.");
				return null;
			}
			m_instance = gameObject.GetComponent<TerrainBrain>();
			if (m_instance == null)
			{
				Debug.LogError("Could not reaquire terrain brain component.");
				return null;
			}
		}
		return m_instance;
	}

	public Texture2D getGroundTexture()
	{
		return groundTexture;
	}

	public Rect[] getUVs()
	{
		return groundUVs;
	}

	private void Start()
	{
		m_instance = this;
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
		blockLoadDistance = Mathf.CeilToInt((viewDistance - (float)chunkSize) / (float)chunkSize);
		m_meshCache = new GameObject[blockLoadDistance * 2 + 1, blockLoadDistance * 2 + 1, blockLoadDistance * 2 + 1];
		currentPos = playerStart;
		currentPos.x = Mathf.Floor(currentPos.x / (float)chunkSize);
		currentPos.y = Mathf.Floor(currentPos.y / (float)chunkSize);
		currentPos.z = Mathf.Floor(currentPos.z / (float)chunkSize);
		lastPos = currentPos;
		groundTexture = new Texture2D(2048, 2048);
		groundTexture.anisoLevel = 9;
		groundUVs = groundTexture.PackTextures(textures, 0, 2048);
		Vector3 vector = new Vector3(0f, 100f, 0f);
		for (int num = 0; num == 0; num = getTerrainDensity(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)))
		{
			vector.y -= 1f;
		}
		vector.y += 3f;
		vector.x += 0.5f;
		vector.z += 0.5f;
		playerStart = vector;
		Debug.Log("Start pos = " + vector.ToString());
		Camera.main.transform.position = playerStart;
		GUIText component = GameObject.Find("StatusDisplay").GetComponent<GUIText>();
		component.text = "Loading...";
	}

	private int[] getCachedChunkPos(int x, int y, int z)
	{
		int num = blockLoadDistance * 2 + 1;
		return new int[3]
		{
			(x >= 0) ? (x % num) : ((num - -x % num) % num),
			(y >= 0) ? (y % num) : ((num - -y % num) % num),
			(z >= 0) ? (z % num) : ((num - -z % num) % num)
		};
	}

	private void stepLoad()
	{
		int num = Mathf.RoundToInt(currentPos.x) - blockLoadDistance;
		int num2 = Mathf.RoundToInt(currentPos.x) + blockLoadDistance;
		int num3 = Mathf.RoundToInt(currentPos.y) - blockLoadDistance;
		int num4 = Mathf.RoundToInt(currentPos.y) + blockLoadDistance;
		int num5 = Mathf.RoundToInt(currentPos.z) - blockLoadDistance;
		int num6 = Mathf.RoundToInt(currentPos.z) + blockLoadDistance;
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				for (int k = num5; k <= num6; k++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(prefab, new Vector3(i * chunkSize, j * chunkSize, k * chunkSize), Quaternion.identity) as GameObject;
					gameObject.name = "TerrainChunk (" + i + ", " + j + ", " + k + ")";
					int[] cachedChunkPos = getCachedChunkPos(i, j, k);
					m_meshCache[cachedChunkPos[0], cachedChunkPos[1], cachedChunkPos[2]] = gameObject;
				}
			}
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Input.mousePosition.x >= 0f && Input.mousePosition.x <= (float)Screen.width && Input.mousePosition.y >= 0f && Input.mousePosition.y <= (float)Screen.height)
		{
			Screen.lockCursor = true;
		}
		if (m_loaded > 1)
		{
			m_loaded--;
			return;
		}
		if (m_loaded == 1)
		{
			stepLoad();
			m_loaded = 0;
			GUIText component = GameObject.Find("StatusDisplay").GetComponent<GUIText>();
			component.text = string.Empty;
			Camera.main.transform.position = playerStart;
			Camera.main.SendMessage("startRunningFPS");
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (m_terrainToCreate.Count > 0 && (double)(Time.realtimeSinceStartup - realtimeSinceStartup) < 0.016)
		{
			Vector3 vector = m_terrainToCreate.Dequeue();
			int[] cachedChunkPos = getCachedChunkPos(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
			GameObject gameObject = m_meshCache[cachedChunkPos[0], cachedChunkPos[1], cachedChunkPos[2]];
			gameObject.SetActive(true);
			gameObject.name = "TerrainChunk (" + vector.x + ", " + vector.y + ", " + vector.z + ")";
			gameObject.transform.position = vector * chunkSize;
			gameObject.SendMessage("regenerateMesh");
		}
		currentPos = Camera.main.transform.position;
		currentPos.x = Mathf.Floor(currentPos.x / (float)chunkSize);
		currentPos.y = Mathf.Floor(currentPos.y / (float)chunkSize);
		currentPos.z = Mathf.Floor(currentPos.z / (float)chunkSize);
		if (currentPos != lastPos)
		{
			Vector3 vector2 = currentPos - lastPos;
			int num = Mathf.RoundToInt(vector2.x);
			int num2 = Mathf.RoundToInt(vector2.y);
			int num3 = Mathf.RoundToInt(vector2.z);
			if (num != 0)
			{
				int num4 = Mathf.RoundToInt(lastPos.x) + num * (blockLoadDistance + 1);
				for (int i = Mathf.RoundToInt(lastPos.y) - blockLoadDistance; i <= Mathf.RoundToInt(lastPos.y) + blockLoadDistance; i++)
				{
					for (int j = Mathf.RoundToInt(lastPos.z) - blockLoadDistance; j <= Mathf.RoundToInt(lastPos.z) + blockLoadDistance; j++)
					{
						m_terrainToCreate.Enqueue(new Vector3(num4, i, j));
					}
				}
			}
			if (num2 != 0)
			{
				int num5 = Mathf.RoundToInt(lastPos.y) + num2 * (blockLoadDistance + 1);
				for (int k = Mathf.RoundToInt(lastPos.x) - blockLoadDistance; k <= Mathf.RoundToInt(lastPos.x) + blockLoadDistance; k++)
				{
					for (int l = Mathf.RoundToInt(lastPos.z) - blockLoadDistance; l <= Mathf.RoundToInt(lastPos.z) + blockLoadDistance; l++)
					{
						m_terrainToCreate.Enqueue(new Vector3(k, num5, l));
					}
				}
			}
			if (num3 != 0)
			{
				int num6 = Mathf.RoundToInt(lastPos.z) + num3 * (blockLoadDistance + 1);
				for (int m = Mathf.RoundToInt(lastPos.x) - blockLoadDistance; m <= Mathf.RoundToInt(lastPos.x) + blockLoadDistance; m++)
				{
					for (int n = Mathf.RoundToInt(lastPos.y) - blockLoadDistance; n <= Mathf.RoundToInt(lastPos.y) + blockLoadDistance; n++)
					{
						m_terrainToCreate.Enqueue(new Vector3(m, n, num6));
					}
				}
			}
		}
		lastPos = currentPos;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void generateTerrainChunk(int x, int y, int z)
	{
		m_tcache.getChunk(x, y, z);
	}

	public int getTerrainDensity(int x, int y, int z)
	{
		return m_tcache.getDensity(x, y, z);
	}

	public void setTerrainDensity(int x, int y, int z)
	{
		m_tcache.setDensity(x, y, z);
	}

	public int getDensity(Vector3 loc)
	{
		float num = 500000f;
		Vector3 vector = new Vector3(loc.x + num, loc.y + num, loc.z + num);
		float num2 = noise(loc.x, loc.y, loc.z);
		int num3 = Mathf.Clamp((int)((1f - loc.y + num2) * 10f), 0, 4);
		num3 = ((!(1f + noise(vector.x, vector.y, vector.z) > 1.15f)) ? num3 : 0);
		if (num3 < 0)
		{
			num3 = 0;
		}
		return num3;
	}

	private float noise(float x, float y, float z)
	{
		if (T == null)
		{
			System.Random random = new System.Random();
			T = new int[8];
			for (int i = 0; i < 8; i++)
			{
				T[i] = random.Next();
			}
		}
		s = (x + y + z) * onethird;
		this.i = fastfloor(x + s);
		j = fastfloor(y + s);
		k = fastfloor(z + s);
		s = (float)(this.i + j + k) * onesixth;
		u = x - (float)this.i + s;
		v = y - (float)j + s;
		w = z - (float)k + s;
		A[0] = 0;
		A[1] = 0;
		A[2] = 0;
		int num = ((u >= w) ? ((!(u >= v)) ? 1 : 0) : ((v >= w) ? 1 : 2));
		int num2 = ((u < w) ? ((!(u < v)) ? 1 : 0) : ((v < w) ? 1 : 2));
		return kay(num) + kay(3 - num - num2) + kay(num2) + kay(0);
	}

	private float kay(int a)
	{
		s = (float)(A[0] + A[1] + A[2]) * onesixth;
		float num = u - (float)A[0] + s;
		float num2 = v - (float)A[1] + s;
		float num3 = w - (float)A[2] + s;
		float num4 = 0.6f - num * num - num2 * num2 - num3 * num3;
		int num5 = shuffle(i + A[0], j + A[1], k + A[2]);
		A[a]++;
		if (num4 < 0f)
		{
			return 0f;
		}
		int num6 = (num5 >> 5) & 1;
		int num7 = (num5 >> 4) & 1;
		int num8 = (num5 >> 3) & 1;
		int num9 = (num5 >> 2) & 1;
		int num10 = num5 & 3;
		float num11;
		switch (num10)
		{
		case 1:
			num11 = num;
			break;
		case 2:
			num11 = num2;
			break;
		default:
			num11 = num3;
			break;
		}
		float num12 = num11;
		float num13;
		switch (num10)
		{
		case 1:
			num13 = num2;
			break;
		case 2:
			num13 = num3;
			break;
		default:
			num13 = num;
			break;
		}
		float num14 = num13;
		float num15;
		switch (num10)
		{
		case 1:
			num15 = num3;
			break;
		case 2:
			num15 = num;
			break;
		default:
			num15 = num2;
			break;
		}
		float num16 = num15;
		num12 = ((num6 != num8) ? num12 : (0f - num12));
		num14 = ((num6 != num7) ? num14 : (0f - num14));
		num16 = ((num6 == (num7 ^ num8)) ? num16 : (0f - num16));
		num4 *= num4;
		return 8f * num4 * num4 * (num12 + ((num10 == 0) ? (num14 + num16) : ((num9 != 0) ? num16 : num14)));
	}

	private int shuffle(int i, int j, int k)
	{
		return b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) + b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7);
	}

	private int b(int i, int j, int k, int B)
	{
		return T[(b(i, B) << 2) | (b(j, B) << 1) | b(k, B)];
	}

	private int b(int N, int B)
	{
		return (N >> B) & 1;
	}

	private int fastfloor(float n)
	{
		return (!(n > 0f)) ? ((int)n - 1) : ((int)n);
	}
}
