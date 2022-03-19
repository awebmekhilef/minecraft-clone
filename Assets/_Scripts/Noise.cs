using UnityEngine;

public static class Noise
{
	public static float[,] Generate(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity)
	{
		float[,] noiseMap = new float[width, height];

		if (seed == 0)
			seed = Random.Range(0, 10000);

		float minValue = float.MaxValue;
		float maxValue = float.MinValue;

		if (scale <= 0f)
			scale = 1f;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				float amplitude = 1f;
				float frequency = 1f;
				float noiseValue = 0f;

				for (int i = 0; i < octaves; i++)
				{
					noiseValue += amplitude * Mathf.PerlinNoise(
						((seed + x) / scale) * frequency,
						((seed + y) / scale) * frequency);

					amplitude *= persistence;
					frequency *= lacunarity;
				}

				noiseMap[x, y] = noiseValue;

				if (noiseValue > maxValue)
					maxValue = noiseValue;
				if (noiseValue < minValue)
					minValue = noiseValue;
			}
		}

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minValue, maxValue, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}
}
