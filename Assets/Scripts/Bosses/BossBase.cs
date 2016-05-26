using UnityEngine;
using System.Collections;

public class BossBase : MonoBehaviour {

    public GameObject explosionPrefab;
    protected float bossHeight;

    void Awake() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        bossHeight = spriteRenderer.sprite.bounds.max.y - spriteRenderer.sprite.bounds.min.y;
    }

    /// <summary>
    /// You should call this function from within your boss code, it does importnat stuff!
    /// </summary>
    virtual public void StartAction() {
        // Call the StartAction function for this stage
        //GameObject.Find("BossStageManager").GetComponent<BossStageManager>().StartAction();
        EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.Trigger("StartAction");
    }

    virtual public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Poop") {
            // Create a new explosion with movement opposite to poop hit position
            GameObject newExplosion = (GameObject) Instantiate(explosionPrefab, coll.transform.position, Quaternion.identity);

            Explosion explosionScript = newExplosion.GetComponent<Explosion>();
            Vector3 newDirection = (coll.transform.position - transform.position).normalized;
            explosionScript.direction = newDirection;

            Destroy(coll.gameObject);
        }
    }
}
