using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

    public float timeToDisappear;

    private EventManager eventManager;  // What message to trigger when bonus is given

    private string actionToTrigger;

	// Use this for initialization
	void Start () {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        RandomizeBonus();
        Invoke("Disappear", timeToDisappear);
	}
	
    /// <summary>
    /// Randomly select the bonus I'm going to give and set stuff.
    /// </summary>
    void RandomizeBonus() {

        int bonusType = Mathf.FloorToInt(Random.value * 3);

        // Just in case the Random.value was 1.0 exactly:
        if (bonusType > 2) {
            bonusType = 2;
        }

        // Handle bonus-specifics
        Sprite[] items = Resources.LoadAll<Sprite>("items");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (bonusType) {
            case 0:
                actionToTrigger = "RifleBonus";
                spriteRenderer.sprite = items[1];
                break;
            case 1:
                actionToTrigger = "TimeBonus";
                spriteRenderer.sprite = items[2];
                break;
            case 2:
                actionToTrigger = "PoopBonus";
                spriteRenderer.sprite = items[3];
                break;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void Disappear() {
        GetComponent<Animator>().SetTrigger("Shrink");
    }

    public void ShrinkEnd() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Handle bonus collecting with poops
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Poop") {
            // Let others know that a bonus was collected and die
            eventManager.Trigger(actionToTrigger);
            Destroy(gameObject);
        }
    }
}
