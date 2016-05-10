using UnityEngine;
using System.Collections;

public class BossStageTest : MonoBehaviour {

    private bool playable = false;  // we begin with unplayable cutscene

	// Use this for initialization
	void Start () {

	}

    void StartAction() {
        playable = true;
        // Remove cutscene
        Destroy(GameObject.Find("CutsceneTest"));
    }
	
}
