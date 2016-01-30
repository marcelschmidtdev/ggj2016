using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	static InputMapper[] InputMappers;

	static PlayerInput () {
		InputMappers = new InputMapper[4];
		for(int i = 0; i < 4; i++) {
#if UNITY_STANDALONE_WIN
			InputMappers[i] = new InputMapperWindows( i );
#else
        InputMappers[i] = new InputMapperDefault(i);
#endif
		}
	}

	public static InputMapper GetInput(int playerIndex) {
		return InputMappers[playerIndex];
	}
}
