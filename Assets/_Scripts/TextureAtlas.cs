using UnityEngine;

public static class TextureAtlas
{
	public const float NormalizedBlockTextureSize = 1f / 16f;

	public static Vector2[] GetUVCoords(Vector2 texCoords)
	{
		float xMin = texCoords.x * NormalizedBlockTextureSize;
		float yMax = 1f - (texCoords.y * NormalizedBlockTextureSize);

		float xMax = xMin + NormalizedBlockTextureSize;
		float yMin = yMax - NormalizedBlockTextureSize;

		return new Vector2[]
		{
			new Vector2(xMin, yMin),
			new Vector2(xMin, yMax),
			new Vector2(xMax, yMin),
			new Vector2(xMax, yMax),
		};
	}
}
