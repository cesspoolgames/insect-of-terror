using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class NumberWidget : MonoBehaviour {

    static Sprite[] numberSprites;

    public int number = 0;  // TODO: Handle negative numbers

    protected GameObject[] digits;  // The digits making up the number. Each index is another digit to the left of the number (i.e. 10's 100's 1000's)

    // Use this for initialization
    void Start() {
        if (numberSprites == null) {
            numberSprites = Resources.LoadAll<Sprite>("Widgets/roachnumberspng") as Sprite[];
            Assert.AreEqual(numberSprites.Length, 10);
            Debug.Log(numberSprites.Length);
        }
    }

    public int NumberOfDigits() {
        int numberOfDigits = 1;
        if (number >= 10) {
            numberOfDigits = Mathf.FloorToInt(Mathf.Log10(number)) + 1;
        }
        Debug.Log(numberOfDigits);
        return numberOfDigits;
    }

	// Update is called once per frame
	void Update () {
    }
}
