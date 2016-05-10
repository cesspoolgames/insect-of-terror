using UnityEngine;
using System.Collections;

public class BossTest : MonoBehaviour {

    public BossStageTest bossStageTest;

    private delegate void Behavior();
    Behavior currentBehavior = delegate () { };  // called in Update()
    Behavior currentFixedBehavior = delegate () { };  // called in FixedUpdate()
    private Rigidbody2D rigidbody2d;

	// Use this for initialization
	void Start () {
        rigidbody2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        currentBehavior();
	}

	// Update is called once per frame
	void FixedUpdate () {
        currentFixedBehavior();
	}

    void StartAction() {
        currentBehavior = ActionBehavior;
        currentFixedBehavior = ActionFixedBehavior;
        bossStageTest.SendMessage("StartAction");
    }

    void ActionBehavior() {
        transform.Rotate(new Vector3(0, 0, 1) * 20 * Time.deltaTime);
    }

    void ActionFixedBehavior() {
        var mag = 10f;
        rigidbody2d.AddForce(new Vector2(Random.Range(-mag, +mag), Random.Range(-mag, +mag)));
    }
}
