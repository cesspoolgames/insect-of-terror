using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    public bool startMoveLate;  // Set to true on one of the Heroes so it starts moving in alternation to the other hero

    private HeroCommon masterScript;
    private Vector2 targetPosition;
    private bool waitForMove = false;
    delegate void Behavior();
    Behavior currentBehavior = delegate () { };

	// Use this for initialization
	void Start () {
        masterScript = GetComponentInParent<HeroCommon>();
        targetPosition = transform.position;
        Invoke("StartMoving", startMoveLate ? masterScript.timeBetweenMoves : 0);
	}

    void StartMoving() {
        currentBehavior += NormalBehavior;
        waitForMove = false;
        InvokeRepeating("AlternateWaitForMove", 0, masterScript.timeBetweenMoves);
    }

	// Update is called once per frame
	void Update () {
        currentBehavior();
	}

    /// <summary>
    /// The normal behvaior of the hero. Moving up/down, shooting
    /// </summary>
    void NormalBehavior() {
        // Calculate where do we want the hero to go (mouse's Y coordinate)
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.y = mousePosition.y;
    }

    void FixedUpdate() {
        if (!waitForMove) {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * masterScript.speed);
            transform.position = newPosition;
        }
    }

    void AlternateWaitForMove() {
        waitForMove = !waitForMove;
    }

}
