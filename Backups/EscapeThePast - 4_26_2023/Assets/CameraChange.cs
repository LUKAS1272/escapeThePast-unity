using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject oppositePortal;
    public Vector3 newCameraPosition;
    public Vector3 baseCameraPosition;
    public GameObject cam;

    void Awake() {
        // cam.GetComponent<Transform>().position = newCameraPosition;
    }

    bool isColliding = false;
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            isColliding = true;
            cam.GetComponent<Transform>().position = newCameraPosition; // Not working
        }
    }
    
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && oppositePortal.GetComponent<CameraChange>().isColliding) {
            isColliding = false;
            cam.GetComponent<Transform>().position = baseCameraPosition;
        }
    }
}
