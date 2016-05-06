using UnityEngine;
using System.Collections.Generic;

public class LawnMower : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    private List<Vector3> targets = new List<Vector3>();

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
        if (true || targets.Count > 0)  {
            Vector3 currentTarget = targets[0];
            Vector3 deltaVector = currentTarget - transform.position;

            float angle = transform.rotation.eulerAngles.z;
            float towardsAngle = Vector3.Angle(deltaVector, new Vector3(1, 0, 0));  // The angle of deltaVector in relation to x-axis
            if (deltaVector.y < 0) towardsAngle *= -1;  // Negate if stuff are negatable
            towardsAngle += 90;  // LawnMower graphic is 90 degrees to the computed degree

            float calculatedAngle = Mathf.MoveTowardsAngle(angle, towardsAngle, 110 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, calculatedAngle));

            if (calculatedAngle - angle < 5f) {
                // If we look straight at target, start moving towards it
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * .5f);
            }

            if ((transform.position - currentTarget).sqrMagnitude < 1e-6) {
                targets.RemoveAt(0);
            }
        }
	}

    public void AddTarget(Vector3 newTarget) {
        targets.Add(newTarget);
    }
}
