using System.Collections.Generic;
using UnityEngine;

public class TerrainPrefabBrain : MonoBehaviour
{
	public enum NeighborDir
	{
		Z_PLUS = 0,
		Z_MINUS = 1,
		Y_PLUS = 2,
		Y_MINUS = 3,
		X_PLUS = 4,
		X_MINUS = 5
	}

	private MeshFilter m_meshFilter;

	private bool m_leftButtonDown;

	private bool m_rightButtonDown;

	private Rect[] groundUVs;

	private Texture2D groundTexture;

	public Shader shader;

	private Vector3 offset;

	private int chunkSize;

	private GameObject[] m_neighbors = new GameObject[6];

	private void Start()
	{
		chunkSize = TerrainBrain.chunkSize;
		offset = base.transform.position;
		base.gameObject.AddComponent<MeshFilter>();
		base.gameObject.AddComponent<MeshRenderer>();
		base.gameObject.AddComponent<MeshCollider>();
		m_meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		m_meshFilter.mesh = mesh;
		groundUVs = TerrainBrain.Instance().getUVs();
		regenerateMesh();
		GetComponent<Renderer>().material.shader = shader;
		GetComponent<Renderer>().material.mainTexture = TerrainBrain.Instance().getGroundTexture();
		GetComponent<Renderer>().material.SetFloat("_LightPower", 0.5f);
		base.enabled = true;
	}

	public void setNeighbor(NeighborDir dir, GameObject tpb)
	{
		m_neighbors[(int)dir] = tpb;
	}

	public GameObject getNeighbor(NeighborDir dir)
	{
		return m_neighbors[(int)dir];
	}

	private void Update()
	{
	}

	private void OnMouseOver()
	{
		if (Input.GetMouseButton(0) && !m_leftButtonDown)
		{
			m_leftButtonDown = true;
			doLeftClick();
		}
		else if (!Input.GetMouseButton(0))
		{
			m_leftButtonDown = false;
		}
		if (Input.GetMouseButton(1) && !m_rightButtonDown)
		{
			m_rightButtonDown = true;
		}
		else if (!Input.GetMouseButton(1))
		{
			m_rightButtonDown = false;
		}
	}

	private int getTerrainValue(Vector3 pos)
	{
		pos += base.transform.position;
		return TerrainBrain.Instance().getTerrainDensity((int)pos.x, (int)pos.y, (int)pos.z);
	}

	private void addUV(int density, ref List<Vector2> uvs)
	{
		density = Mathf.Clamp(density - 1, 0, groundUVs.Length);
		uvs.Add(new Vector2(groundUVs[density].x + 0.001f, groundUVs[density].y + 0.001f));
		uvs.Add(new Vector2(groundUVs[density].x + groundUVs[density].width - 0.001f, groundUVs[density].y + groundUVs[density].height - 0.001f));
		uvs.Add(new Vector2(groundUVs[density].x + groundUVs[density].width - 0.001f, groundUVs[density].y + 0.001f));
		uvs.Add(new Vector2(groundUVs[density].x + 0.001f, groundUVs[density].y + groundUVs[density].height - 0.001f));
	}

	private void regenerateMesh()
	{
		offset = base.transform.position;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		MeshCollider component = base.gameObject.GetComponent<MeshCollider>();
		List<int> list = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		List<Vector3> list2 = new List<Vector3>();
		List<Color> list3 = new List<Color>();
		float num = 0f;
		mesh.Clear();
		int num2 = 0;
		Vector3 pos = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < chunkSize; i++)
		{
			for (int j = 0; j < chunkSize; j++)
			{
				for (int k = 0; k < chunkSize; k++)
				{
					num = 0.45f;
					pos.x = i;
					pos.y = j;
					pos.z = k;
					int terrainValue = getTerrainValue(pos);
					if (terrainValue != 0)
					{
						pos.z -= 1f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i, j, k));
							list2.Add(new Vector3(i + 1, j + 1, k));
							list2.Add(new Vector3(i + 1, j, k));
							list2.Add(new Vector3(i, j + 1, k));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 2);
							list.Add(num2);
							list.Add(num2 + 3);
							list.Add(num2 + 1);
							num2 += 4;
						}
						pos.z += 2f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i, j, k + 1));
							list2.Add(new Vector3(i + 1, j + 1, k + 1));
							list2.Add(new Vector3(i + 1, j, k + 1));
							list2.Add(new Vector3(i, j + 1, k + 1));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 2);
							list.Add(num2 + 1);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 3);
							num2 += 4;
						}
						pos.z = k;
						pos.y -= 1f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i, j, k));
							list2.Add(new Vector3(i + 1, j, k + 1));
							list2.Add(new Vector3(i + 1, j, k));
							list2.Add(new Vector3(i, j, k + 1));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 2);
							list.Add(num2 + 1);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 3);
							num2 += 4;
						}
						pos.y += 2f;
						num = 0.5f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i, j + 1, k));
							list2.Add(new Vector3(i + 1, j + 1, k + 1));
							list2.Add(new Vector3(i + 1, j + 1, k));
							list2.Add(new Vector3(i, j + 1, k + 1));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 2);
							list.Add(num2);
							list.Add(num2 + 3);
							list.Add(num2 + 1);
							num2 += 4;
						}
						pos.y = j;
						num = 0.45f;
						pos.x -= 1f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i, j, k));
							list2.Add(new Vector3(i, j + 1, k + 1));
							list2.Add(new Vector3(i, j + 1, k));
							list2.Add(new Vector3(i, j, k + 1));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 3);
							list.Add(num2 + 1);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 2);
							num2 += 4;
						}
						pos.x += 2f;
						if (getTerrainValue(pos) == 0)
						{
							list2.Add(new Vector3(i + 1, j, k));
							list2.Add(new Vector3(i + 1, j + 1, k + 1));
							list2.Add(new Vector3(i + 1, j + 1, k));
							list2.Add(new Vector3(i + 1, j, k + 1));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							list3.Add(new Color(num, num, num));
							addUV(terrainValue, ref uvs);
							list.Add(num2);
							list.Add(num2 + 1);
							list.Add(num2 + 3);
							list.Add(num2);
							list.Add(num2 + 2);
							list.Add(num2 + 1);
							num2 += 4;
						}
					}
				}
			}
		}
		mesh.vertices = list2.ToArray();
		mesh.triangles = list.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors = list3.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();
		component.sharedMesh = new Mesh();
		component.sharedMesh = mesh;
	}

	public static void SolveTangents(Mesh mesh)
	{
		int num = mesh.triangles.Length / 3;
		int num2 = mesh.vertices.Length;
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] array3 = new Vector4[num2];
		for (long num3 = 0L; num3 < num; num3 += 3)
		{
			long num4 = mesh.triangles[num3];
			long num5 = mesh.triangles[num3 + 1];
			long num6 = mesh.triangles[num3 + 2];
			Vector3 vector = mesh.vertices[num4];
			Vector3 vector2 = mesh.vertices[num5];
			Vector3 vector3 = mesh.vertices[num6];
			Vector2 vector4 = mesh.uv[num4];
			Vector2 vector5 = mesh.uv[num5];
			Vector2 vector6 = mesh.uv[num6];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			array[num4] += vector7;
			array[num5] += vector7;
			array[num6] += vector7;
			array2[num4] += vector8;
			array2[num5] += vector8;
			array2[num6] += vector8;
		}
		for (long num18 = 0L; num18 < num2; num18++)
		{
			Vector3 vector9 = mesh.normals[num18];
			Vector3 vector10 = array[num18];
			Vector3 normalized = (vector10 - vector9 * Vector3.Dot(vector9, vector10)).normalized;
			array3[num18] = new Vector4(normalized.x, normalized.y, normalized.z);
			array3[num18].w = ((!(Vector3.Dot(Vector3.Cross(vector9, vector10), array2[num18]) < 0f)) ? 1f : (-1f));
		}
		mesh.tangents = array3;
	}

	private GameObject findTerrainChunk(int x, int y, int z)
	{
		GameObject gameObject = GameObject.Find("TerrainChunk (" + x + ", " + y + ", " + z + ")");
		if (gameObject == null)
		{
			Debug.LogWarning("Trouble finding chunk " + x + ", " + y + ", " + z);
		}
		return gameObject;
	}

	private GameObject findNeighbor(NeighborDir dir, int x, int y, int z)
	{
		switch (dir)
		{
		case NeighborDir.X_MINUS:
			m_neighbors[0] = findTerrainChunk(x - 1, y, z);
			return m_neighbors[0];
		case NeighborDir.X_PLUS:
			m_neighbors[1] = findTerrainChunk(x + 1, y, z);
			return m_neighbors[1];
		case NeighborDir.Y_MINUS:
			m_neighbors[2] = findTerrainChunk(x, y - 1, z);
			return m_neighbors[2];
		case NeighborDir.Y_PLUS:
			m_neighbors[3] = findTerrainChunk(x, y + 1, z);
			return m_neighbors[3];
		case NeighborDir.Z_MINUS:
			m_neighbors[4] = findTerrainChunk(x, y, z - 1);
			return m_neighbors[4];
		case NeighborDir.Z_PLUS:
			m_neighbors[5] = findTerrainChunk(x, y, z + 1);
			return m_neighbors[5];
		default:
			return null;
		}
	}

	private void doLeftClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		RaycastHit hitInfo = default(RaycastHit);
		if (!GetComponent<Collider>().Raycast(ray, out hitInfo, 10f))
		{
			return;
		}
		Vector3 vector = hitInfo.point + 0.0001f * ray.direction;
		int x = Mathf.CeilToInt(vector.x) - 1;
		int y = Mathf.CeilToInt(vector.y) - 1;
		int z = Mathf.CeilToInt(vector.z) - 1;
		TerrainBrain.Instance().setTerrainDensity(x, y, z);
		int x2 = (int)(offset.x / (float)chunkSize);
		int y2 = (int)(offset.y / (float)chunkSize);
		int z2 = (int)(offset.z / (float)chunkSize);
		GameObject gameObject = null;
		if (vector.x - offset.x - 1f < 1f)
		{
			gameObject = findNeighbor(NeighborDir.X_MINUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		else if (vector.x - offset.x - 1f > (float)(chunkSize - 2))
		{
			gameObject = findNeighbor(NeighborDir.X_PLUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		if (vector.y - offset.y - 1f < 1f)
		{
			gameObject = findNeighbor(NeighborDir.Y_MINUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		else if (vector.y - offset.y - 1f > (float)(chunkSize - 2))
		{
			gameObject = findNeighbor(NeighborDir.Y_PLUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		if (vector.z - offset.z - 1f < 1f)
		{
			gameObject = findNeighbor(NeighborDir.Z_MINUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		else if (vector.z - offset.z - 1f > (float)(chunkSize - 2))
		{
			gameObject = findNeighbor(NeighborDir.Z_PLUS, x2, y2, z2);
			if (gameObject != null)
			{
				gameObject.SendMessage("regenerateMesh");
			}
		}
		regenerateMesh();
	}

	private void doRightClick()
	{
		Debug.Log("Benchmarking 100000 direct vs. 100000 indirect calls.");
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		for (int i = 0; i < 100000; i++)
		{
			base.gameObject.SendMessage("doTestCall");
		}
		Debug.Log("SendMessage took " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		realtimeSinceStartup = Time.realtimeSinceStartup;
		for (int j = 0; j < 100000; j++)
		{
			TerrainPrefabBrain component = base.gameObject.GetComponent<TerrainPrefabBrain>();
			component.doTestCall();
		}
		Debug.Log("Realtime took " + (Time.realtimeSinceStartup - realtimeSinceStartup));
	}

	public void doTestCall()
	{
		int num = 5;
		num *= 20;
	}
}
