using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;  // Singleton
    public int levelNumber = 0;

    [HideInInspector]
    public float top, right, bottom, left; 

    void Awake () {
        // Make sure there's only one instance of GameManager
	    if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
            return;
        }

        // Set game boundaries based on camera size
        const float sizeRatio = 4f / 3f;
        top = Camera.main.orthographicSize;
        bottom = -top;
        left = bottom * sizeRatio;
        right = top * sizeRatio;

        DontDestroyOnLoad(gameObject);  // Keep GameManager on all scenes all the time ever
	}

    /// <summary>
    /// Return the Enemy's normal and pooped sprites according to the zero-based index of enemies from the nice Enemies spritesheet
    /// </summary>
    /// <param name="index"></param>
    public void ComputeEnemySprites(int index, out Sprite normalSprite, out Sprite poopedSprite) {
        normalSprite = null;
        poopedSprite = null;
        Sprite[] enemySprites = Resources.LoadAll<Sprite>("enemies");
        if (index < 0 || index >= enemySprites.Length / 2) {
            Debug.LogError("Don't choose indices of pooped-upon enemy sprites or negative indices. Index is: " + index + ", Max sprites: " + enemySprites.Length);
            return;
        }
        normalSprite = enemySprites[index];
        poopedSprite = enemySprites[index + enemySprites.Length / 2];
    }
}
