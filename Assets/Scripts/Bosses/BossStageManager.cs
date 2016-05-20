using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the boss stage. Removing the initial cutscene game objects, spawn bonuses, whatever.
/// </summary>

public class BossStageManager : MonoBehaviour {

    //private bool playable = false;  // We begin with unplayable cutscene

    /// <summary>
    /// The actual boss' game object should call this, or anywhere after the cutscene is done.
    /// </summary>
    public void StartAction() {
        //playable = true;
        // Remove cutscene game object
        Destroy(GameObject.Find("Cutscene"));
    }

}
