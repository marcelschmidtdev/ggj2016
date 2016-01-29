using UnityEngine;
using System.Collections;

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
