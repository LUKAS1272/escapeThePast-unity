using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public CircleCollider2D player;
    public GameObject oppositePortal;
    public Vector3 newCameraPosition;
    public Vector3 originalCameraPosition;
    public new GameObject camera;

    void Start() {
        Physics2D.IgnoreCollision(player, GetComponent<BoxCollider2D>());
    }

    bool isColliding = false;
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            isColliding = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && oppositePortal.GetComponent<CameraChange>().isColliding) {
            isColliding = false;
            camera.GetComponent<Transform>().position = originalCameraPosition;
        }
    }
}
