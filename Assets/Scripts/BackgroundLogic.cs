using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundLogic : MonoBehaviour {

    public Image levelBackground;

	// Use this for initialization
	void Start () {
        int gameLevel = GameManager.instance.levelNumber;
        // TODO: Make sure we don't overflow in levelNumber?
        Sprite backgroundSprite = Resources.Load<Sprite>("level" + gameLevel) as Sprite;
        levelBackground.GetComponent<Image>().sprite = backgroundSprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
