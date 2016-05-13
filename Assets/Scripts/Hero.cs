using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class Hero : MonoBehaviour {

    public bool startMoveLate;  // Set to true on one of the Heroes so it starts moving in alternation to the other hero
    public GameObject poop;  // The poop the hero shoots

    private HeroCommon masterScript;
    private Vector2 targetPosition;
    private bool waitForMove = false;
    delegate void Behavior();
    Behavior currentBehavior = delegate () { };
    private bool flipped = false;

    private IEnumerator<float> fireCoroutineHandle;

	// Use this for initialization
	void Start () {
        masterScript = GameObject.Find("HeroesCommon").GetComponent<HeroCommon>();
        targetPosition = transform.position;
        Invoke("StartMoving", startMoveLate ? masterScript.timeBetweenMoves : 0);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(spriteRenderer.flipX);
        if (spriteRenderer.flipX) {
            flipped = true;  // Mark that we are flipped so we know when to shoot things reversed etc.
        }
        StartFiring();
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

    IEnumerator<float> _FirePoop() {
        while (true) {
            if (waitForMove) {
                yield return 0f;
            } else {
                // Create poop
                Quaternion rotation = Quaternion.identity;
                Vector3 position = transform.position;

                GameObject newPoop = (GameObject)Instantiate(poop, position, rotation);
                if (flipped) {
                    newPoop.GetComponent<BossPoop>().horizontalFactor = -1;
                }

                yield return Timing.WaitForSeconds(.3f);
            }
        }
    }

    void StartFiring() {
        fireCoroutineHandle = Timing.RunCoroutine(_FirePoop());
    }

    void StopFiring() {  // FIXME: Might not be used? Do we have constant fire? If so no need for the coroutine handle variable
        Timing.KillCoroutine(fireCoroutineHandle);
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
