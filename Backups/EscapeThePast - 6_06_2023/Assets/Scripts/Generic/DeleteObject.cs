using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public void Delete(GameObject parent) {
        Destroy(parent);
    }
}
