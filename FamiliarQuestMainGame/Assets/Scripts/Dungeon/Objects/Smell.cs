using UnityEngine;

public class Smell : MonoBehaviour {
    public float intensity;
    private float radius = 0;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        radius += Time.deltaTime * 2;
        intensity *= Mathf.Pow(0.8f, Time.deltaTime);
        if (intensity < 1) Cull();
        //else foreach (var mob in Monster.monsters) if (Vector3.Distance(mob.transform.position, transform.position) <= radius && intensity >= mob.GetComponent<MonsterSenses>().senseOfSmell) mob.GetComponent<MonsterSenses>().SmellSomething(transform.position);
    }
    private void Cull() {
        Destroy(gameObject);
        Destroy(this);
    }
}
