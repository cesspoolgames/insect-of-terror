using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the boss stage. Removing the initial cutscene game objects, spawn bonuses, whatever.
/// </summary>

public class BossStageManager : MonoBehaviour {

    public GameObject bonusPrefab;
    public float bonusCreateMinDelay, bonusCreateMaxDelay;
    public Text messageText;

    private EventManager eventManager;

    void Start() {
        Invoke("SpawnBonus", Random.Range(bonusCreateMinDelay, bonusCreateMaxDelay));

        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.Subscribe("RifleBonus", ApplyRifleBonus);
        eventManager.Subscribe("TimeBonus", ApplyTimeBonus);
        eventManager.Subscribe("PoopBonus", ApplyPoopBonus);
    }

    /// <summary>
    /// The actual boss' game object should call this, or anywhere after the cutscene is done.
    /// </summary>
    public void StartAction() {
        //playable = true;
        // Remove cutscene game object
        Destroy(GameObject.Find("Cutscene"));
    }

    void SpawnBonus() {
        Vector3 newPosition = new Vector3();
        newPosition.x = Random.Range(GameManager.instance.left, GameManager.instance.right);
        newPosition.y = Random.Range(GameManager.instance.bottom, GameManager.instance.top);

        Instantiate(bonusPrefab, newPosition, Quaternion.identity);
        Invoke("SpawnBonus", Random.Range(bonusCreateMinDelay, bonusCreateMaxDelay));
    }

    void ApplyRifleBonus() {
        messageText.text = "Oh my god... Machine-Gun!";
    }

    void ApplyTimeBonus() {
        messageText.text = "Time Bonus acquired!";
    }

    void ApplyPoopBonus() {
        messageText.text = "Collected Poop Bonus!";
    }
}
