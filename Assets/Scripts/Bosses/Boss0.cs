using UnityEngine;
using System.Collections;

public class Boss0 : BossBase {

    delegate void Behavior();
    Behavior currentBehavior = delegate () { };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        currentBehavior();
	}

    override public void StartAction() {
        base.StartAction();
        GetComponent<Animator>().enabled = false;
        currentBehavior = NormalBehavior;
    }

    void NormalBehavior() {
        transform.Rotate(new Vector3(0, 0, 120 * Time.deltaTime));
    }
}
