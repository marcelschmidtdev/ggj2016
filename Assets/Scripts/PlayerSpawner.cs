using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public Vector3 Position
	{
		get
		{
			return this.transform.position;
		}
	}
}
