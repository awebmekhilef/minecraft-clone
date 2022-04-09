using UnityEngine;

public static class Util
{
	public static void DrawBounds(Bounds bounds)
	{
		Vector3
			ruf = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z),
			rub = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z),
			luf = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z),
			lub = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z),
			rdf = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z),
			rdb = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z),
			lfd = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z),
			lbd = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

		Gizmos.DrawLine(ruf, luf);
		Gizmos.DrawLine(ruf, rub);
		Gizmos.DrawLine(luf, lub);
		Gizmos.DrawLine(rub, lub);

		Gizmos.DrawLine(ruf, rdf);
		Gizmos.DrawLine(rub, rdb);
		Gizmos.DrawLine(luf, lfd);
		Gizmos.DrawLine(lub, lbd);

		Gizmos.DrawLine(rdf, lfd);
		Gizmos.DrawLine(rdf, rdb);
		Gizmos.DrawLine(lfd, lbd);
		Gizmos.DrawLine(lbd, rdb);
	}

	public static float MoveWithinBlock(float pos, float normal, bool adjacent)
	{
		if (pos - (int)pos == 0f)
		{
			if (adjacent)
				pos += (normal / 2);
			else
				pos -= (normal / 2);
		}

		return pos;
	}
}
