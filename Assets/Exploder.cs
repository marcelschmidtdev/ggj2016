using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {
    private ArrayList playersInRange;
    private ArrayList minionsInRange;

	// Use this for initialization
	void Start () {
        playersInRange = new ArrayList();
        minionsInRange = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            playersInRange.Add(other.gameObject);
        } else if (other.name == "minion")
        {
            minionsInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "player")
        {
            playersInRange.Remove(other.gameObject);
        }
        else if (other.name == "minion")
        {
            minionsInRange.Remove(other.gameObject);
        }
    }
}
