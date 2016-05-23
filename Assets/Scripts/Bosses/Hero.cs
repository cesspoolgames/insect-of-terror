using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class Hero : MonoBehaviour {

    public bool startMoveLate;  // Set to true on one of the Heroes so it starts moving in alternation to the other hero
    public GameObject poop;  // The poop the hero shoots

    private HeroCommon masterScript;
    private Vector2 targetPosition;
    private bool waitForMove = true;  // Start immobile
    delegate void Behavior();
    Behavior currentBehavior = delegate () { };
    Behavior currentFixedBehavior = delegate () { };
    private bool flipped = false;
    private float spriteWidth;
    private float paralyzeStart;  // In game time

    void Awake() {
        // Subscribe to events
        EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.Subscribe("StartAction", StartAction);

        // Get reference to Hero master script
        masterScript = GameObject.Find("HeroesCommon").GetComponent<HeroCommon>();
    }

	// Use this for initialization
	void Start () {
        targetPosition = transform.position;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.flipX) {
            flipped = true;  // Mark that we are flipped so we know when to shoot things reversed etc.
        }
        spriteWidth = spriteRenderer.sprite.bounds.max.x - spriteRenderer.sprite.bounds.min.x;
	}

    void StartAction() {
        if (startMoveLate) {
            waitForMove = true;
            Invoke("StartMoving", masterScript.timeBetweenMoves);
        } else {
            waitForMove = false;
            Invoke("StartMoving", 0);
        }
        InvokeRepeating("AlternateWaitForMove", masterScript.timeBetweenMoves, masterScript.timeBetweenMoves);
        StartFiring();  // Does not shoot on waitOnMove
    }

    void StartMoving() {
        currentBehavior += NormalBehavior;
        currentFixedBehavior += FixedMoveTowardsTargetPosition;
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
            if (waitForMove || IsParalyzed()) {
                yield return 0f;
            } else {
                // Create poop
                Quaternion rotation = Quaternion.Euler(0, 0, Random.value * 360f);
                Vector3 position = transform.position;

                // Move poop's starting point to approximately where the poop gun is.
                float horizontalDisplacement = spriteWidth;
                if (flipped) {
                    horizontalDisplacement *= -1;
                }
                position.x += horizontalDisplacement;

                GameObject newPoop = (GameObject)Instantiate(poop, position, rotation);
                if (flipped) {
                    newPoop.GetComponent<BossPoop>().horizontalFactor = -1;
                }

                yield return Timing.WaitForSeconds(.3f);
            }
        }
    }

    void StartFiring() {
        Timing.RunCoroutine(_FirePoop());
    }

    //void StopFiring() {  // FIXME: Might not be used? Do we have constant fire? If so no need for the coroutine handle variable
    //    Timing.KillCoroutine(_FirePoop().GetType());
    //}

    void FixedUpdate() {
        currentFixedBehavior();
    }

    void FixedMoveTowardsTargetPosition() {
        if (!waitForMove) {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * masterScript.speed);
            transform.position = newPosition;
        }
    }

    void AlternateWaitForMove() {
        waitForMove = !waitForMove;
    }

    /// <summary>
    /// When hit by a minion, fall down and do nothing for a few seconds
    /// </summary>
    void FallDown() {
        if (IsParalyzed()) {
            paralyzeStart = Time.realtimeSinceStartup;
            return;
        }

        currentFixedBehavior -= FixedMoveTowardsTargetPosition;
        paralyzeStart = Time.realtimeSinceStartup;

        Timing.RunCoroutine(_GetUpSomeday());

        if (flipped) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f));
        } else {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
    }

    /// <summary>
    /// Should be invoked automatically from FallDown
    /// </summary>
    void GetUp() {
        StartMoving();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    /// <summary>
    /// Will not GetUp() until paralyzeStart is not too recent. It may be reset each time minion touches hero.
    /// </summary>
    /// <returns></returns>
    IEnumerator<float> _GetUpSomeday() {
        //yield return Timing.WaitForSeconds(masterScript.paralyzeTime);
        while (IsParalyzed()) {
            yield return Timing.WaitForSeconds(0.02f);
        }
        GetUp();
    }

    public bool IsParalyzed() {
        return Time.realtimeSinceStartup - paralyzeStart < masterScript.paralyzeTime;
    }
}
