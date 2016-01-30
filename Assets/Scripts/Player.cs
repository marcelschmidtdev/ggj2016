using UnityEngine;
using System.Collections;

// This is going to be on every player instance and controls stuff like input, camera etc.

public class Player : MonoBehaviour {

	public Camera Camera;
	public PlayerControls PlayerControls;
	public PlayerView PlayerView;
	public bool PlayerReady = false;
	public int Score;
	public int OwnMinionsKilled;
	public int EnemyMinionsKilled;

	public int Index
	{
		get; private set;
	}

	public void InitPlayerIndex(int index) {
        this.Index = index;
		this.PlayerControls.playerNumber = (PlayerControls.PlayerNumber)index;
	}

	public Vector3 Position
	{
		get
		{
			return this.PlayerView.transform.position;
		}
		set
		{
			this.PlayerView.transform.position = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return this.PlayerView.transform.rotation;
		}
		set
		{
			this.PlayerView.transform.rotation = value;
		}
	}
}
