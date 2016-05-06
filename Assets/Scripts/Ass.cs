using UnityEngine;
using System.Collections;

public class Ass : MonoBehaviour {

    public AssPoop assPoop;
    public float createPoopTime;

	// Use this for initialization
	void Start () {
        InvokeRepeating("CreatePoop", 0, createPoopTime);
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 mousePos = Input.mousePosition;
        Vector2 assPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = assPos;
	}

    /// <summary>
    /// Create a poop a little down from the Ass with a random z-rotation.
    /// </summary>
    void CreatePoop() {
        Quaternion zRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
        AssPoop newPoop = (AssPoop) Instantiate(assPoop, transform.position, zRotation);
        Vector2 newPoopSize = newPoop.GetComponent<SpriteRenderer>().sprite.bounds.size;
        newPoop.transform.position -= new Vector3(0, newPoopSize.y / 2f, 0);
    }
}
