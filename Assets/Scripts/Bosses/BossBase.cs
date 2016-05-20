using UnityEngine;
using System.Collections;

public class BossBase : MonoBehaviour {

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


}
