using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    public static Dictionary<string, DontDestroyOnLoad> ddols = new Dictionary<string, DontDestroyOnLoad>();
    public string type;

    // Start is called before the first frame update
    void Start() {
        if (!ddols.ContainsKey(type) || ddols[type] == null) {
            DontDestroyOnLoad(gameObject);
            ddols[type] = this;
        }
        else Destroy(gameObject);
    }
}
