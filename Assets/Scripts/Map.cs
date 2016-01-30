using UnityEngine;
using System.Collections;

// Drop all map relevant Behaviours in here

public class Map : SingletonMonoBehaviour<Map> {
	public PlayerSpawner[] PlayerSpawners;
	public CreepsSpawner[] CreepsSpawners;
}
