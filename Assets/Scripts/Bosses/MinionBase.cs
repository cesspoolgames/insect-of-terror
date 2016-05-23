using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for Minion (Boss stage). Basic behavior such collision handling, points, stuff.
/// </summary>

public class MinionBase : MonoBehaviour {

    protected bool pooped = false;
    protected SpriteRenderer spriteRenderer;

    protected Sprite normalSprite;
    protected Sprite poopedSprite;

    virtual public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.instance.ComputeEnemySprites(GameManager.instance.levelNumber, out normalSprite, out poopedSprite);
    }

    virtual public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Poop") {
            BecomePooped();
            Destroy(coll.gameObject);
        } else if (coll.gameObject.tag == "Player") {
            coll.gameObject.SendMessage("FallDown");
        }
    }

    public void BecomePooped() {
        pooped = true;
        GetComponent<Rigidbody2D>().gravityScale = 1f;
        spriteRenderer.sprite = poopedSprite;
    }
}
