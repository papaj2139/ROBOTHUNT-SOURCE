using System;
using System.Collections.Generic;
using UnityEngine;

internal class TerrainCache
{
	private Dictionary<IntCoords, int[,,]> m_data = new Dictionary<IntCoords, int[,,]>();

	private IntCoords m_lastCoords;

	private int[,,] m_lastData;

	public int[,,] getChunk(int x, int y, int z)
	{
		IntCoords intCoords = new IntCoords(x, y, z);
		if (intCoords.Equals(m_lastCoords))
		{
			return m_lastData;
		}
		m_lastCoords = intCoords;
		if (m_data.ContainsKey(intCoords))
		{
			m_lastData = m_data[intCoords];
			return m_lastData;
		}
		int chunkSize = TerrainBrain.chunkSize;
		m_lastData = new int[chunkSize, chunkSize, chunkSize];
		TerrainBrain terrainBrain = TerrainBrain.Instance();
		for (int i = 0; i < chunkSize; i++)
		{
			for (int j = 0; j < chunkSize; j++)
			{
				for (int k = 0; k < chunkSize; k++)
				{
					Vector3 loc = new Vector3(x + i, y + j, z + k) / TerrainBrain.noiseMultiplier;
					m_lastData[i, j, k] = terrainBrain.getDensity(loc);
				}
			}
		}
		m_data[intCoords] = m_lastData;
		return m_lastData;
	}

	public int getDensity(int x, int y, int z)
	{
		int chunkSize = TerrainBrain.chunkSize;
		int num = Math.Abs(x);
		int num2 = Math.Abs(y);
		int num3 = Math.Abs(z);
		int num4 = ((x >= 0) ? (x % chunkSize) : ((chunkSize - num % chunkSize) % chunkSize));
		int num5 = ((y >= 0) ? (y % chunkSize) : ((chunkSize - num2 % chunkSize) % chunkSize));
		int num6 = ((z >= 0) ? (z % chunkSize) : ((chunkSize - num3 % chunkSize) % chunkSize));
		int x2 = x - num4;
		int y2 = y - num5;
		int z2 = z - num6;
		int[,,] chunk = getChunk(x2, y2, z2);
		return chunk[num4, num5, num6];
	}

	public void setDensity(int x, int y, int z)
	{
		int chunkSize = TerrainBrain.chunkSize;
		int num = Math.Abs(x);
		int num2 = Math.Abs(y);
		int num3 = Math.Abs(z);
		int num4 = ((x >= 0) ? (x % chunkSize) : ((chunkSize - num % chunkSize) % chunkSize));
		int num5 = ((y >= 0) ? (y % chunkSize) : ((chunkSize - num2 % chunkSize) % chunkSize));
		int num6 = ((z >= 0) ? (z % chunkSize) : ((chunkSize - num3 % chunkSize) % chunkSize));
		int x2 = x - num4;
		int y2 = y - num5;
		int z2 = z - num6;
		IntCoords key = new IntCoords(x2, y2, z2);
		if (!m_data.ContainsKey(key))
		{
			Debug.Log("Couldn't find coords: " + key.ToString());
		}
		else
		{
			m_data[key][num4, num5, num6] = 0;
		}
	}
}
