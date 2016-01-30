using UnityEngine;
using System.Collections;

public abstract class InputMapper {

    protected bool wasCharging = false;
    protected bool charging = false;
    protected Vector2 movement = Vector2.zero;

    public abstract Vector2 getMovement();

    public abstract void Update();

	public abstract bool GetStart ();
	public abstract bool GetConfirm();
	public abstract bool GetCancel();
	public abstract bool IsConnected ();

	public bool startedCharging()
    {
        return !wasCharging && charging;
    }

    public bool isCharging()
    {
        return charging && wasCharging;
    }

    public bool finishedCharging()
    {
        return wasCharging && !charging;
    }
}
