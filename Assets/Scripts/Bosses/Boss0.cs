using UnityEngine;
using System.Collections.Generic;

public class Boss0 : BossBase {

    public Hero leftHero, rightHero;
    public float rotationSpeed;
    public float switchTargetHeroTime;
    public float timeBetweenMinions;  // The creation rate essentially

    private string targetHero = "right";  // "left" or "right". Will start with "left" because we invoke SwitchHero immediately

    delegate void Behavior();
    Behavior currentFixedBehavior = delegate () { };

	// Use this for initialization
	void Start () {
        // TODO: This is TEMP FOR DEVELOPMENT
        StartAction();
	}
	
	void FixedUpdate () {
        currentFixedBehavior();
	}

    override public void StartAction() {
        base.StartAction();
        GetComponent<Animator>().enabled = false;
        currentFixedBehavior = FixedNormalBehavior;
        InvokeRepeating("SwitchHero", 0, switchTargetHeroTime);
    }

    void FixedNormalBehavior() {
        Quaternion lookAt;
        if (targetHero == "left") {
            lookAt = Quaternion.LookRotation(leftHero.transform.position);
        } else {
            lookAt = Quaternion.LookRotation(rightHero.transform.position);
        }
        lookAt.x = 0; lookAt.y = 0;  // So we rotate only on z-axis, though this is a quaternion so it's weird. huh?

        // Now turn your minion creation hole towards the hero
        if (targetHero == "left") {
            lookAt *= Quaternion.Euler(new Vector3(0, 0, -90));
        } else {
            lookAt *= Quaternion.Euler(new Vector3(0, 0, +90));
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, Time.deltaTime * rotationSpeed);
    }

    void SwitchHero() {
        if (targetHero == "left") {
            targetHero = "right";
        } else {
            targetHero = "left";
        }

        // TODO: reset minion creation?
    }
}
