using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    public float timeToDisappear;
    public float minRotationSpeed, maxRotationSpeed;
    public float minSpeed, maxSpeed;
    public Vector3 direction;  // Should be normalized

    private float rotationSpeed;
    private float speed;

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.value * 360f));  // Start at random rotation
        speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        Invoke("Disappear", timeToDisappear);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));
        transform.position = transform.position + Time.deltaTime * direction * speed;
	}

    // Destroy myself
    void Disappear() {
        Destroy(gameObject);
    }
}
