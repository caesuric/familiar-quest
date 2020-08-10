using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player = null;
    public Vector3 positionOffset;
    //public static GameObject instance = null;

    private void Start() {
        //if (instance == null) {
            DontDestroyOnLoad(gameObject);
        //    instance = gameObject;
        //}
        //else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (player == null && PlayerCharacter.localPlayer != null) player = PlayerCharacter.localPlayer.gameObject;
        else if (player != null) transform.position = player.transform.position + positionOffset;
    }
}
