using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public Image levelBackground;
    public Sprite[] backgroundSprites;
    public float levelBoundariesFactor;  // Takes the top/bottom/left/right of the GameManager and scales that down a bit

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
