using UnityEngine;

/*
 * The reason why the previous octave noise algorithm wasn't working is due to
 * normalizing the noise values locally. So each chunk has it's own max and min noise heights
 * which causes them to not align. The noise value needs to be normalized globally.
 * See: https://www.youtube.com/watch?v=4olmeStiBsE&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=10
 */

public static class Noise
{
	public static float[,] Generate(int width, int height, float scale, int octaves, float persistence, float lacunarity, Vector2 offset, int seed = -1)
	{
		FastNoiseLite noise = new FastNoiseLite(seed == -1 ? System.DateTime.Now.Second : seed);

		noise.SetFractalOctaves(octaves);
		noise.SetFractalGain(persistence);
		noise.SetFractalLacunarity(lacunarity);

		float[,] noiseMap = new float[width, height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float noiseValue = noise.GetNoise(
					(x + offset.x) / scale,
					(y + offset.y) / scale
				);

				// Convert from -1 to 1 to 0 to 1
				noiseMap[x, y] = (noiseValue + 1f) * 0.5f;
			}
		}

		return noiseMap;
	}
}
