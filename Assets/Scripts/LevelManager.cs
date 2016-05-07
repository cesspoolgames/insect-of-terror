using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public Image levelBackground;
    public Sprite[] backgroundSprites;
    public float levelBoundariesFactor;  // Takes the top/bottom/left/right of the GameManager and scales that down a bit
    public GameObject enemyPrefab;
    public float createEnemyEvery;  // Time in seconds between each enemy create calls
    public int maxEnemies;  // Maximum enemies on the level, pooped or alive

    // Level-specific boundaries, a bit differnet than GameManager boundaries
    [HideInInspector]
    public float top, right, bottom, left;

    void Awake() {
        // Make sure there's only one instance of LevelManager
	    if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
            return;
        }
    }

	// Use this for initialization
	void Start () {
        int gameLevel = GameManager.instance.levelNumber;

        // Make sure we won't overflow
        if (gameLevel >= backgroundSprites.Length) {
            gameLevel = backgroundSprites.Length - 1;
        }

        levelBackground.GetComponent<Image>().sprite = backgroundSprites[gameLevel];

        // Set boundaries
        top = GameManager.instance.top * levelBoundariesFactor;
        right = GameManager.instance.right * levelBoundariesFactor;
        bottom = GameManager.instance.bottom * levelBoundariesFactor; 
        left = GameManager.instance.left * levelBoundariesFactor;

        InvokeRepeating("MaybeCreateEnemy", 0, createEnemyEvery);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Creates an enemy at one of the edges of the level.
    /// </summary>
    void CreateEnemy() {
        Vector2 startPosition = new Vector2();  // Starting position for new enemy
        switch (Mathf.FloorToInt(Random.value * 4f)) {
            case 0:  // Top
                startPosition.x = Random.Range(left, right);
                startPosition.y = top;
                break;
            case 1:  // Right
                startPosition.x = right;
                startPosition.y = Random.Range(top, bottom);
                break;
            case 2:  // Bottom
                startPosition.x = Random.Range(left, right);
                startPosition.y = bottom;
                break;
            case 3:  // Left
                startPosition.x = left;
                startPosition.y = Random.Range(top, bottom);
                break;
        }
        Instantiate(enemyPrefab, startPosition, Quaternion.identity);
    }

    /// <summary>
    /// Called every X period of time, will create an enemy only if there are not too many enemies on the level.
    /// </summary>
    void MaybeCreateEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length < maxEnemies) {
            CreateEnemy();
        }
    }
}
