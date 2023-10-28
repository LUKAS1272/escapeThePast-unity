using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardRotator : MonoBehaviour
{
    public float speed = 10f;
    public int orientation;
    int defaultOrientation;

    new bool enabled = true;
    public GameObject uis;

    void Awake() {
        defaultOrientation = orientation;
    }

    void FixedUpdate() {
        enabled = true;
        for (int i = 0; i < uis.transform.childCount; i++) { // Checks, whether there is some toggled UI
            if (uis.transform.GetChild(i).gameObject.activeInHierarchy) {
                enabled = false;
                break;
            }
        }

        if (enabled) {
            float angle = transform.rotation.eulerAngles.z;
            if (angle > 180 && angle != 0 && angle != 360) angle -= 360;

            if (Mathf.Abs(angle) >= 90 || defaultOrientation * angle < 0) {
                orientation *= -1;
            }

            transform.Rotate(new Vector3(0, 0, 1 * speed * orientation * Time.deltaTime));
        }
    }
}
