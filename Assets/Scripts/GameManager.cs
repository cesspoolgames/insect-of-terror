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
}
