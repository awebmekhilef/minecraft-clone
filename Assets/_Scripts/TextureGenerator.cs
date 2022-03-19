using UnityEngine;

public static class TextureGenerator
{
	public static Texture2D GenerateColoredTexture(float[,] heightMap, Gradient gradient)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);

		Color[] colors = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colors[y * width + x] = gradient.Evaluate(heightMap[x, y]);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.SetPixels(colors);
		texture.Apply();

		return texture;
	}
}
