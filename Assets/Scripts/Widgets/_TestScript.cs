using UnityEngine;
using System.Collections;

public class _TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Sprite[] numbers = Resources.LoadAll<Sprite>("Widgets/roachnumberspng") as Sprite[];
        Debug.Log(numbers.Length);
        for (int i = 0; i <= 9; i++) {
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>();
            SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = numbers[i];
            newObject.transform.position = new Vector3(i - 5, 0, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
