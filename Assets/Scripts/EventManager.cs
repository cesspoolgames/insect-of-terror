using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage a list of System.Action to be called when an 'event' happens. 'event' is just a string name of something.
/// </summary>

public class EventManager : MonoBehaviour {

    // 'event name' => dictionary of 'subscription id' => 'actual delegate'
    // The subscription id is optional for the subscriber so it can remove its subscription anytime.
    // Subscription id is per-event-name basis.
    private Dictionary<string, Dictionary<float, System.Action>> eventTable = new Dictionary<string, Dictionary<float, System.Action>>();

    public float Subscribe(string name, System.Action callMe) {
        if (!eventTable.ContainsKey(name)) {
            eventTable.Add(name, new Dictionary<float, System.Action>());
        }

        float subscriptionId;
        do {
            subscriptionId = Random.value;  // Create unique random value for subscription id
        } while (eventTable[name].ContainsKey(subscriptionId));

        eventTable[name].Add(subscriptionId, callMe);
        return subscriptionId;
    }

    public void Unsubscribe(string name, float subscriptionId) {
        eventTable[name].Remove(subscriptionId);
    }

    // Trigger the event, call the delegate of all subscribers
    public void Trigger(string name) {
        if (eventTable.ContainsKey(name)) {
            foreach (var callThis in eventTable[name]) {
                callThis.Value();  // Call the action
            }
        }
    }

}
