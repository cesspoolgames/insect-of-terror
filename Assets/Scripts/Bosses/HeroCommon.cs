using UnityEngine;
using System.Collections;

/// <summary>
/// Stuff shared between the hero scripts and such.
/// This is so you have a HeroCommon GameObject where you control stuff from the Unity interface.
/// The Hero scripts themselves load stuff from this HeroCommon script. No need to change parameters
/// on both heroes' GameObjects.
/// </summary>

public class HeroCommon : MonoBehaviour {
    public float speed;
    public float timeBetweenMoves;
    public float paralyzeTime;
}
