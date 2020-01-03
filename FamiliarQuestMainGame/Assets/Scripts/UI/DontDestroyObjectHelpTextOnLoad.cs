using UnityEngine;

public class DontDestroyObjectHelpTextOnLoad : MonoBehaviour {

    public static GameObject instance = null;

    // Use this for initialization
    void Start() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = gameObject;
        }
        else Destroy(gameObject);
    }
    // Update is called once per frame
    void Update() {

    }
}
