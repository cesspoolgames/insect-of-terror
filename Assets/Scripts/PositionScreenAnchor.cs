using UnityEngine;
using System.Collections;

/// <summary>
/// Position the game object in one of the corners of the screen, hopefully.
/// </summary>

public class PositionScreenAnchor : MonoBehaviour {

    public Vector3 offset;

	// Use this for initialization
	void Start () {
        // TODO: Add other positions, this is very early work

        // Stick object to the edge       
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        bottomRight.z = 0;
        transform.position = bottomRight + offset;
	}
	
}
