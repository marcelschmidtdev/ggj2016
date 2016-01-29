﻿using UnityEngine;
using System.Collections;

// This is going to be on every player instance and controls stuff like input, camera etc.

public class Player : MonoBehaviour {

	public Camera Camera;
	public int Index
	{
		get; private set;
	}

	public void InitPlayerIndex(int index) {
		this.Index = index;
		switch (index) {
			//case 
		}
	}
}
