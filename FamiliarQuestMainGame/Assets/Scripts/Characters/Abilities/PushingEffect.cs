using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushingEffect : MonoBehaviour {

    private bool started = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 movementAmount;
    private float time;
    private float currentTime = 0;

    // Update is called once per frame
	void FixedUpdate () {
        //if (!NetworkServer.active) return;
        if (started) {
            currentTime += Time.deltaTime;
            var hits = Physics.OverlapSphere(transform.position, 0.25f);
            foreach (var hit in hits) {
                if (hit.gameObject.CompareTag("Wall")) {
                    Destroy(this);
                    return;
                }
            }
            var newPosition = transform.position + (Time.deltaTime / time * movementAmount);
            transform.position = new Vector3(newPosition.x, 0, newPosition.z);
            if (currentTime>=time) Destroy(this);
        }
	}

    public void Initialize(Vector3 targetPos, float time) {
        startPos = transform.position;
        this.targetPos = targetPos;
        movementAmount = targetPos - startPos;
        this.time = time;
        currentTime = 0;
        started = true;
    }
}
