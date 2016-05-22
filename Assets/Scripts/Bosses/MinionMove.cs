using UnityEngine;
using System.Collections;

/// <summary>
/// Minion movement, forward and optionally(?) wrap around stage
/// </summary>

public class MinionMove : MinionBase {

    public float speed;
    public float maxRotation;
    public float rotationDelay;
    public bool wrapAroundWorld;

    override public void Start() {
        base.Start();
        InvokeRepeating("RotateABit", rotationDelay, rotationDelay);
    }

    void Update() {
        if (!wrapAroundWorld) {
            // Die when outside world
            if (transform.position.x < GameManager.instance.left * 1.5f || transform.position.x > GameManager.instance.right * 1.5f) {
                Destroy(gameObject);
            }

            if (transform.position.y < GameManager.instance.bottom * 1.5f || transform.position.y > GameManager.instance.top * 1.5f) {
                Destroy(gameObject);
            }
        } else {
            // TODO
        }
    }

    void FixedUpdate() {
        if (!pooped) {
            transform.Translate(Time.deltaTime * Vector3.up * speed);
        }
    }

    /// <summary>
    /// Called periodically to 'wiggle' minion
    /// </summary>
    void RotateABit() {
        transform.Rotate(new Vector3(0, 0, (Random.value - 0.5f) * 2 * maxRotation));
    }
}
