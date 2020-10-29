using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{

    public Transform mainCamera;

    private void Start() {
        mainCamera = Camera.main.transform;
    }

    void Update(){
        transform.LookAt(mainCamera);
    }
}
