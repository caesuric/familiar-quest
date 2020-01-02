using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float duration;
    // Use this for initialization
    void Start() {
        Destroy(gameObject, duration);
    }
}
