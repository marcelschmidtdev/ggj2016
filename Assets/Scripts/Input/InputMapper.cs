using UnityEngine;
using System.Collections;

public abstract class InputMapper {

    protected bool wasCharging = false;
    protected bool charging = false;
    protected Vector2 movement = Vector2.zero;

    public abstract Vector2 getMovement();

	public abstract bool GetStart ();
	public abstract bool GetConfirm();
	public abstract bool GetCancel();
	public abstract bool IsConnected ();
}
