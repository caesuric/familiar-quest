using UnityEngine;

public class Billboard : MonoBehaviour {

    public Camera mainCamera = null;
    private Transform cameraTransform = null;

    // Update is called once per frame
    void Update() {
        if (mainCamera == null || cameraTransform == null) {
            mainCamera = Camera.main;
            cameraTransform = mainCamera.transform;
        }
        if (mainCamera != null && cameraTransform != null) transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}
