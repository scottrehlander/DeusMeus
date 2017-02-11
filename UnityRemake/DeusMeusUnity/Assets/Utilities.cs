using UnityEngine;
using System.Collections;

public static class Utilities {

	public static bool ShouldColliderBlock(Collider2D collider)
	{
		if (collider.tag.Equals ("Do not block")) {
			return false;
		}
		return true;
	}

}
