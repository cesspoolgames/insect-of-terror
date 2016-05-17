using UnityEngine;
using System.Collections;

public class BossPoop : MonoBehaviour {

    public float horizontalFactor = 1;  // Use this to reverse the direction of the poop
    public float speed;  // Horizontal speed

    private Rigidbody2D rigidbody2d;

	// Use this for initialization
	void Start () {
        rigidbody2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // Destroy myself when out of game area
        if (transform.position.y < GameManager.instance.bottom * 2) {
            Destroy(this.gameObject);
        }
	}

    void FixedUpdate() {
        Vector2 constantVelocity = rigidbody2d.velocity;
        constantVelocity.x = speed * horizontalFactor;
        rigidbody2d.velocity = constantVelocity;
    }
}
