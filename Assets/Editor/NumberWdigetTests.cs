using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NumberWdigetTests {

    [Test]
    public void EditorTest()
    {
        //Arrange
        var widget1 = new NumberWidget();

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
