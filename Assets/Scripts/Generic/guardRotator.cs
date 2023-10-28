using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardRotator : MonoBehaviour
{
    public float speed = 10f;
    public int orientation;
    int defaultOrientation;

    void Awake() {
        defaultOrientation = orientation;
    }

    void FixedUpdate() {
        if (FirstActiveUI.Instance.Run() == -1) {
            float angle = transform.localRotation.eulerAngles.z;
            if (angle > 180 && angle != 0 && angle != 360) angle -= 360;

            if (Mathf.Abs(angle) >= 90 || defaultOrientation * angle < 0) {
                orientation *= -1;
            }

            transform.Rotate(new Vector3(0, 0, 1 * speed * orientation * Time.deltaTime));
        }
    }
}
