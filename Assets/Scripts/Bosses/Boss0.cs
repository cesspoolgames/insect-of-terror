using UnityEngine;
using System.Collections.Generic;
using MovementEffects;

public class Boss0 : BossBase {

    public Hero leftHero, rightHero;
    public float speed;
    public float rotationSpeed;
    public float switchTargetHeroTime;
    public GameObject minionPrefab;
    public int minionsPerSpawn;
    public float minionDelay;  // The creation rate essentially
    public float swayDistance;  // How much up/down to go
    public bool startGoingUp;

    private string targetHero = "right";  // "left" or "right". Will start with "left" because we invoke SwitchHero immediately
    private Vector3 targetPosition;  // Where to go to

    private IEnumerator<float> spawningCoroutine;

    delegate void Behavior();
    Behavior currentFixedBehavior = delegate () { };

    // Use this for initialization
    void Start() {
        if (startGoingUp) {
            targetPosition = new Vector3(0, swayDistance, 0);
        } else {
            targetPosition = new Vector3(0, -swayDistance, 0);
        }

        // TODO: This is TEMP FOR DEVELOPMENT, remove
        StartAction();
    }

    void FixedUpdate() {
        currentFixedBehavior();
    }

    override public void StartAction() {
        base.StartAction();
        GetComponent<Animator>().enabled = false;
        currentFixedBehavior = FixedNormalBehavior;
        currentFixedBehavior += FixedSwayBehavior;
        InvokeRepeating("SwitchHero", 0, switchTargetHeroTime);
    }

    void FixedNormalBehavior() {
        Quaternion lookAt;
        if (targetHero == "left") {
            lookAt = Quaternion.LookRotation(leftHero.transform.position - transform.position);
        } else {
            lookAt = Quaternion.LookRotation(rightHero.transform.position - transform.position);
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

    void FixedSwayBehavior() {
        bool movingUp = targetPosition.y > 0;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, targetPosition) < Vector3.kEpsilon) {
            if (movingUp) {
                // Move down now
                targetPosition = new Vector3(0, -swayDistance, 0);
            } else {
                // Move up now
                targetPosition = new Vector3(0, swayDistance, 0);
            }
        }
    }

    void SwitchHero() {
        if (targetHero == "left") {
            targetHero = "right";
        } else {
            targetHero = "left";
        }

        // TODO: reset minion creation?
        Timing.RunCoroutine(SpawnMinions());
    }

    IEnumerator<float> SpawnMinions() {
        for (int i = 0; i < minionsPerSpawn; i++) {
            Quaternion rotation = transform.rotation;
            Vector3 position = transform.position;

            rotation *= Quaternion.Euler(0, 0, 180);

            GameObject newMinion = (GameObject) Instantiate(minionPrefab, position, rotation);
            newMinion.transform.Translate(new Vector3(0, bossHeight * 0.75f, 0));

            yield return Timing.WaitForSeconds(minionDelay);
        }
    }

}
