using UnityEngine;

public class LifeleechScript : MonoBehaviour {

    public bool started = false;
    public Transform victim = null;
    public Vector3 victimOriginalPosition;
    public Vector3 randPos1;
    public Vector3 randPos2;
    public Transform vampire = null;
    public float runTime = 0;
    private readonly float effectRange = 3;
    private readonly float effectTime = 0.5f;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (runTime >= effectTime) {
            Destroy(gameObject);
            Destroy(this);
        }
        if (!started && victim != null && vampire != null) Initialize();
        if (started) {
            runTime += Time.deltaTime;
            transform.position = CubicBezier(victimOriginalPosition, randPos1, randPos2, vampire.position, (runTime / effectTime));
        }
    }

    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
    }

    private void Initialize() {
        started = true;
        victimOriginalPosition = new Vector3(victim.position.x, victim.position.y, victim.position.z);
        randPos1 = victimOriginalPosition + new Vector3(Random.Range(-effectRange, effectRange), Random.Range(0, effectRange * 2), Random.Range(-effectRange, effectRange));
        randPos2 = vampire.position + new Vector3(Random.Range(-effectRange, effectRange), Random.Range(0, effectRange * 2), Random.Range(-effectRange, effectRange));
    }
}
