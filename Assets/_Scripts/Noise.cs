using UnityEngine;

public static class Noise
{
	public static float[,] Generate(int width, int height, float scale)
	{
		float[,] noiseMap = new float[width, height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				noiseMap[x, y] = Mathf.PerlinNoise(x / scale, y / scale);
			}
		}

		return noiseMap;
	}
}
