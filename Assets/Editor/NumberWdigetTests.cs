using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NumberWdigetTests {

    GameObject gameObject;
    NumberWidget widget1;

    [SetUp]
    public void SetUp() {
        gameObject = new GameObject();
        gameObject.AddComponent<NumberWidget>();
        widget1 = gameObject.GetComponent<NumberWidget>();
    }

    [Test]
    public void TestNumerOfDigits()
    {
        //Arrange
        widget1.number = 0;
        Assert.AreEqual(widget1.NumberOfDigits(), 1);

        widget1.number = 1;
        Assert.AreEqual(widget1.NumberOfDigits(), 1);

        widget1.number = 10;
        Assert.AreEqual(widget1.NumberOfDigits(), 2);

        widget1.number = 12;
        Assert.AreEqual(widget1.NumberOfDigits(), 2);

        widget1.number = 30;
        Assert.AreEqual(widget1.NumberOfDigits(), 2);

        widget1.number = 100;
        Assert.AreEqual(widget1.NumberOfDigits(), 3);

        widget1.number = 101231;
        Assert.AreEqual(widget1.NumberOfDigits(), 6);
    }

}
