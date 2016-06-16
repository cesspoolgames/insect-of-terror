using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class AssPoop : MonoBehaviour {

    public float height;
    public float gravity;
    public float finalScaleFactor;
    public string secondarySortingLayerName;
    public float waitBeforeFade;
    public float fadeOutSpeed;

    private Vector3 bottomPos;  // The point on the floor where the poop is.
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 finalScale;
    private float fallProgress;  // 0.0->1.0 of the progress of the fall
    private bool active = true;  // Will be false when fading out

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// I keep a list of enemies I am colliding with. Once on the ground I will
    /// send them all "You are pooped" message.
    /// </summary>
    private HashSet<GameObject> enemiesToBePooped = new HashSet<GameObject>();

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        bottomPos = transform.position + new Vector3(0, -height, 0);
        initialScale = transform.localScale;
        finalScale = initialScale * finalScaleFactor;
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    void computeFallProgress() {
        // This is how to defy the KISS principle.

        // Compute the total progress in 0.0->1.0 of the total magnitude and the progressed magnitude
        // Squared values don't interfere with the division
        float sqrMagnitudeTotal = (bottomPos - initialPosition).sqrMagnitude;
        float sqrMagnitudeCurrent = (transform.position - initialPosition).sqrMagnitude;
        fallProgress = sqrMagnitudeCurrent / sqrMagnitudeTotal;
    }

	// Update is called once per frame
	void Update () {
        if (!active) {
            return;
        }

        // Move to lower sorting layer so it won't hide Ass
        if (spriteRenderer.sortingLayerName != secondarySortingLayerName && fallProgress >= 0.25f) {
            spriteRenderer.sortingLayerName = secondarySortingLayerName;
        }

        // Check if we finished being an active poop
        if (fallProgress >= 0.99f) {
            Invoke("StartFading", waitBeforeFade);
            active = false;
            bool didPoopsOccur = SendMessageToPoopedEnemies();
            if (didPoopsOccur) {
                SendMessageToLawnMower();
            }
    }
}

    /// <summary>
    /// Iterate on all enemies that should be pooped and if they are not already pooped, send them the appropriate message.
    /// </summary>
    /// <returns>true is any enemies were pooped</returns>
    bool SendMessageToPoopedEnemies() {
        bool poopingHappened = false;
        foreach (var enemy in enemiesToBePooped) {
            if (enemy == null) {
                continue;
            }
            if (!enemy.GetComponent<Enemy>().IsPooped()) {
                enemy.SendMessage("BecomePooped");
                poopingHappened = true;
            }
        }
        return poopingHappened;
    }

    void SendMessageToLawnMower() {
        // LawnMower needs to be notified of where the pooping has occurred
        GameObject lawnMower = GameObject.Find("/LawnMower");
        lawnMower.SendMessage("AddTarget", transform.position);
    }

    /// <summary>
    /// Start the fading and dying coroutine. This function is called from an Invoke().
    /// </summary>
    void StartFading() {
        Timing.RunCoroutine(FadeOutAndDie(fadeOutSpeed));
    }

    void FixedUpdate () {
        computeFallProgress();
        transform.position = Vector3.MoveTowards(transform.position, bottomPos, gravity * Time.deltaTime);
        transform.localScale = initialScale + ((finalScale - initialScale) * fallProgress);
    }

    /// <summary>
    /// Slowly fade out to nothing and then destory the gameObject and die.
    /// </summary>
    /// <param name="speed">How much units per deltaTime. Note that 100% alpha is 1.0.</param>
    /// <returns></returns>
    IEnumerator<float> FadeOutAndDie(float speed) {
        float alpha = spriteRenderer.material.color.a;
        while (alpha >= 0.01f) {
            alpha -= speed * Time.deltaTime;
            Color newColor = new Color(1, 1, 1, alpha);
            spriteRenderer.material.color = newColor;

            yield return 0f;
        }
        Destroy(gameObject);
    }

    // Enemies to-be-poopped handling
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Enemy") {
            enemiesToBePooped.Add(coll.gameObject);
        }
    }

    // More Enemies to-be-poopped handling
    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Enemy") {
            enemiesToBePooped.Remove(coll.gameObject);
        }
    }
}
