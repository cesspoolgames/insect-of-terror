using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class NumberWidget : MonoBehaviour {

    static Sprite[] numberSprites;

    public float digitWidth = 1f;
    public int number = 0;  // TODO: Handle negative numbers
    private int previousNumber = 0;

    protected List<GameObject> digits = new List<GameObject>();  // The digits making up the number. Each index is another digit to the left of the number (i.e. 10's 100's 1000's)

    // Use this for initialization
    void Start() {
        if (numberSprites == null) {
            numberSprites = Resources.LoadAll<Sprite>("Widgets/roachnumberspng") as Sprite[];
            Assert.AreEqual(numberSprites.Length, 10);
        }
        digits.Add(transform.GetChild(0).gameObject);  // Add the initial GameObject that the object already has, "0"
        SyncGameObjectsInstances();
    }

    public int NumberOfDigits() {
        int numberOfDigits = 1;
        if (number >= 10) {
            numberOfDigits = Mathf.FloorToInt(Mathf.Log10(number)) + 1;
        }
        return numberOfDigits;
    }

    // Update is called once per frame
    void Update() {
        // Check if we need to refresh the digits
        if (number != previousNumber) {
            previousNumber = number;

            // Make sure we have enough digit game objects
            SyncGameObjectsInstances();

            // Change each digit GameObject to the correct digit sprite
            int runningNumber = number;  // Helper to calculate the least-significant digit each iteration
            for (int i = 0; i < digits.Count; i++) {
                int digitValue = runningNumber - ((int)(runningNumber / 10) * 10);
                SpriteRenderer renderer = digits[i].GetComponent<SpriteRenderer>();
                renderer.sprite = numberSprites[digitValue];
                runningNumber /= 10;  // "shift-right"
            }
        }
    }

    /// <summary>
    /// Move the digits GameObjects to the correct position. Helper function.
    /// </summary>
    private void RepositionDigits() {
        Vector3 translation = new Vector3(0, 0, 0);
        for (var i = 1; i < digits.Count; i++) {
            translation.x -= digitWidth;
            digits[i].transform.rotation = digits[0].transform.rotation;
            digits[i].transform.position = digits[0].transform.position;
            digits[i].transform.localScale = digits[0].transform.localScale;
            digits[i].transform.Translate(translation);
        }
    }

    void SyncGameObjectsInstances() {
        if (NumberOfDigits() > digits.Count) {
            // We need to add digits GameObjects
            for (var i = 0; i < NumberOfDigits() - digits.Count; i++) {
                //// Create and position new GameObject
                GameObject newDigit = Instantiate(digits[0]);
                newDigit.transform.parent = transform;
                digits.Add(newDigit);
            }
            RepositionDigits();
        } else {
            // Too many GameObject digits. Remove the most-significant ones
            for (var index = digits.Count - 1; index > NumberOfDigits() - 1; index--) {
                Destroy(digits[index]);
                digits.RemoveAt(index);
            }
        }
    }

}
