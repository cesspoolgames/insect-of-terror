using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

public class Enemy : MonoBehaviour {

    public float speed;
    public float maxRotation;
    //public float rotationSpeed;
    //public float changeRotateToRate;

    private SpriteRenderer spriteRenderer;
    private bool passingEdge = false;  // true when passing level edge and transforming to the opposite one
    private bool pooped = false;
    private Sprite normalSprite;  // For easily setting the final death image
    private Sprite poopedSprite;  // For easily setting the final death image

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ComputeEnemySprites(GameManager.instance.levelNumber);
        spriteRenderer.sprite = normalSprite;
        transform.Rotate(new Vector3(0, 0, Random.value * 360.0f));
	}

    /// <summary>
    /// Change the Enemy's appearance according to the zero-based index of enemies from the nice Enemies spritesheet
    /// </summary>
    /// <param name="index"></param>
    public void ComputeEnemySprites(int index) {
        Sprite[] enemySprites = Resources.LoadAll<Sprite>("enemies");
        if (index < 0 || index >= enemySprites.Length / 2) {
            Debug.LogError("Don't choose indices of pooped-upon enemy sprites or negative indices. Index is: " + index + ", Max sprites: " + enemySprites.Length);
            return;
        }
        normalSprite = enemySprites[index];
        poopedSprite = enemySprites[index + enemySprites.Length / 2];
    }

    void FixedUpdate() {
        if (!pooped) {
            // Wiggle wiggle wiggle
            float nowRotationSpeed = Random.Range(-maxRotation, +maxRotation);
            transform.Rotate(new Vector3(0, 0, nowRotationSpeed * Time.deltaTime));

            // Move forward
            transform.position = transform.position += transform.up * speed * Time.deltaTime;

            // Check edge/level boundaries
            if (!passingEdge) {
                Vector3 newPosition = transform.position;

                // top
                if (transform.position.y > LevelManager.instance.top) {
                    passingEdge = true;
                    newPosition.y = LevelManager.instance.bottom;
                }

                // bottom
                if (transform.position.y < LevelManager.instance.bottom) {
                    passingEdge = true;
                    newPosition.y = LevelManager.instance.top;
                }

                // right
                if (transform.position.x > LevelManager.instance.right) {
                    passingEdge = true;
                    newPosition.x = LevelManager.instance.left;
                }

                // left
                if (transform.position.x < LevelManager.instance.left) {
                    passingEdge = true;
                    newPosition.x = LevelManager.instance.right;
                }

                // Start coroutine
                if (passingEdge) {
                    Timing.StartUpdateCoroutine(PassLevelEdge(newPosition));
                }
            }
        }
    }

    IEnumerator<float> PassLevelEdge(Vector3 newPosition) {
        float alpha = 1.0f;
        const float alphaSpeed = 2.8f;
        // Fade out
        while (alpha >= 0.01f) {
            alpha -= alphaSpeed * Time.deltaTime;
            if (alpha < 0) alpha = 0;
            Color newColor = new Color(1, 1, 1, alpha);
            spriteRenderer.material.color = newColor;

            yield return 0f;
        }

        transform.position = newPosition;

        // Fade in
        while (alpha < 1.0f) {
            alpha += alphaSpeed * Time.deltaTime;
            if (alpha > 1.0f) alpha = 1.0f;
            Color newColor = new Color(1, 1, 1, alpha);
            spriteRenderer.material.color = newColor;

            yield return 0f;
        }

        // Done!
        passingEdge = false;
    }

    /// <summary>
    /// Sent by the AssPoop when the Enemy is required to be pooped-upon
    /// </summary>
    public void BecomePooped() {
        pooped = true;
        spriteRenderer.sprite = poopedSprite;
    }

    public bool IsPooped() {
        return pooped;
    }
}
