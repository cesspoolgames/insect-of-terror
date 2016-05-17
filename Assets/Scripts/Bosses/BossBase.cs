using UnityEngine;
using System.Collections;

public class BossBase : MonoBehaviour {

    /// <summary>
    /// You should call this function from within your boss code, it does importnat stuff!
    /// </summary>
    virtual public void StartAction() {
        // Call the StartAction function for this stage
        //GameObject.Find("BossStageManager").GetComponent<BossStageManager>().StartAction();
        EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.Trigger("StartAction");
    }

}
