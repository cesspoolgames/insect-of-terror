using UnityEngine;
using System;
using System.Collections.Generic;

public class LawnMower : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    private List<Vector3> targets = new List<Vector3>();
    private Rigidbody2D rigidbody2d;

	// Use this for initialization
	void Start () {
        rigidbody2d = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate() {

        // Get all enemies and sort them by the time they were pooped.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<KeyValuePair<DateTime, Vector3>> whenWhere = new List<KeyValuePair<DateTime, Vector3>>();  // <Poop time, location>. We'll sort this soon.

        foreach (var enemy in enemies) {
            var enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript && enemyScript.IsPooped()) {
                // This enemy is pooped, add the poop time and location to the list 
                whenWhere.Add(new KeyValuePair<DateTime, Vector3>(enemyScript.PoopedTime(), enemy.transform.position));
            }
        }
        whenWhere.Sort((a, b) => a.Key.CompareTo(b.Key));  // Sort by poop time

        if (whenWhere.Count > 0)  {
            Vector3 currentTarget = whenWhere[0].Value;  // The position of the enemy
            Vector3 deltaVector = currentTarget - transform.position;

            float angle = transform.rotation.eulerAngles.z;
            float towardsAngle = Vector3.Angle(deltaVector, new Vector3(1, 0, 0));  // The angle of deltaVector in relation to x-axis
            if (deltaVector.y < 0) towardsAngle *= -1;  // Negate if stuff are negatable
            towardsAngle += 90;  // LawnMower graphic is 90 degrees to the computed degree

            float calculatedAngle = Mathf.MoveTowardsAngle(angle, towardsAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, calculatedAngle));

            if (Mathf.DeltaAngle(towardsAngle, calculatedAngle) < 5f) { // FIXME: doesn't work well, think how to go forward stuff. Update: I think it works actually...
                // If we look straight at target, start moving towards it
                rigidbody2d.AddForce(-transform.up.normalized * speed);
            }
        }
    }

    public void AddTarget(Vector3 newTarget) {
        targets.Add(newTarget);
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == "Enemy") {
            Enemy enemyScript = coll.gameObject.GetComponent<Enemy>();
            if (enemyScript && enemyScript.IsPooped()) {
                // TODO: Add score and yays
                Destroy(coll.gameObject);
            }
        }
    }
}
