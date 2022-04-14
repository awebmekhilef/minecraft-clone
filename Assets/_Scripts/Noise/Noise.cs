using UnityEngine;

/*
 * The reason why the previous octave noise algorithm wasn't working is due to
 * normalizing the noise values locally. So each chunk has it's own max and min noise heights
 * which causes them to not align. The noise value needs to be normalized globally.
 * See: https://www.youtube.com/watch?v=4olmeStiBsE&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=10
 */

public static class Noise
{
	static FastNoiseLite _noise;

	public static void Init(int octaves, float persistence, float lacunarity)
	{
		_noise = new FastNoiseLite();

		_noise.SetFractalOctaves(octaves);
		_noise.SetFractalGain(persistence);
		_noise.SetFractalLacunarity(lacunarity);
	}

	public static float[,] Generate(int width, int height, float scale, Vector2 offset)
	{
		float[,] noiseMap = new float[width, height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float noiseValue = _noise.GetNoise(
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
