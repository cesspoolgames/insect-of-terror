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
        top = Camera.main.orthographicSize;
        bottom = -top;
        left = bottom;
        right = top;

        DontDestroyOnLoad(gameObject);  // Keep GameManager on all scenes all the time ever
	}
}
