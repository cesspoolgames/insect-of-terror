using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public Sprite[] backgroundSprites;
    public float levelBoundariesFactor;  // Takes the top/bottom/left/right of the GameManager and scales that down a bit
    public GameObject enemyPrefab;
    public float createEnemyEvery;  // Time in seconds between each enemy create calls
    public int maxEnemies;  // Maximum enemies on the level, pooped or alive

    public int timeLeftStart;
    private int timeLeft;

    private Text countdownValueText;

    private EventManager eventManager;

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
        // Set boundaries
        top = GameManager.instance.top * levelBoundariesFactor;
        right = GameManager.instance.right * levelBoundariesFactor;
        bottom = GameManager.instance.bottom * levelBoundariesFactor; 
        left = GameManager.instance.left * levelBoundariesFactor;

        InvokeRepeating("MaybeCreateEnemy", 0, createEnemyEvery);

        // Colorize text gameObjects
        GameObject.Find("CountdownLabel").GetComponent<Text>().color = GameManager.instance.basicColor;
        countdownValueText = GameObject.Find("CountdownValue").GetComponent<Text>();
        countdownValueText.color = GameManager.instance.actionColor;
        GameObject.Find("Score").GetComponent<Text>().color = GameManager.instance.basicColor;

        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.Subscribe("EnemyCollected", EnemyCollected);

        timeLeft = timeLeftStart;
        UpdateCountdownText();
        InvokeRepeating("CountdownTick", 1f, 1f);
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

    void UpdateCountdownText() {
        // Update text
        countdownValueText.text = timeLeft.ToString();
    }

    void CountdownTick() {
        timeLeft--;
        UpdateCountdownText();
    }

    void EnemyCollected() {
        Debug.Log("Update score");
    }
}
