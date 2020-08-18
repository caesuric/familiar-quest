using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingEffect : MonoBehaviour {

    private bool started = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float time;
    private float currentTime = 0;
    // Use this for initialization
    void Start() {
        startPos = transform.position;
        targetPos = transform.position + transform.forward * 20;
        var rayDirection = targetPos - transform.position;
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, rayDirection, out hitInfo);
        if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Wall") || hitInfo.distance > Vector3.Distance(targetPos, startPos)) Destroy(this);
        else {
            time = 0.25f;
            currentTime = 0;
            started = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (started) {
            currentTime += Time.deltaTime;
            if (currentTime > time) currentTime = time;
            var newPosition = Vector3.Lerp(startPos, targetPos, currentTime / time);
            var rayDirection = newPosition - transform.position;
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, rayDirection, out hitInfo);
            if (hitInfo.collider != null && !hitInfo.collider.gameObject.CompareTag("Wall") || hitInfo.distance > Vector3.Distance(newPosition, transform.position)) {
                transform.position = newPosition;
                if (currentTime == time) Destroy(this);
            }
        }
    }
}
