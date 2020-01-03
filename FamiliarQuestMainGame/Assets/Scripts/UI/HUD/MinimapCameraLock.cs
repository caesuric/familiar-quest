using UnityEngine;

public class MinimapCameraLock : MonoBehaviour {

    public GameObject player = null;
    public Vector3 positionOffset;
    public Vector3 targetPositionOffset;
    public float timer = 0;
    public static float zoomTime = 0.3f;
    public new Camera camera;
    public static GameObject instance = null;

    private void Start() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = gameObject;
            camera.enabled = true;
            var canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            var width = (400f / (float)Screen.width) * canvas.scaleFactor;
            var height = (300f / (float)Screen.height) * canvas.scaleFactor;
            camera.rect = new Rect(1f - width, 1f - (height * 0.29f / 0.25f), width, height);
        }
        else Destroy(gameObject);
        targetPositionOffset = positionOffset;
    }

    // Update is called once per frame
    void Update() {
        if (player == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) player = item.gameObject;
        }
        if (player != null) transform.position = player.transform.position + positionOffset;
        if (positionOffset != targetPositionOffset) {
            timer += Time.deltaTime;
            positionOffset = Vector3.Lerp(positionOffset, targetPositionOffset, timer / zoomTime);
        }
    }

    public void ZoomIn() {
        var newY = targetPositionOffset.y / 1.2f;
        targetPositionOffset = new Vector3(0, newY, 0);
        if (targetPositionOffset.y < 1) targetPositionOffset = new Vector3(0, 1, 0);
        timer = 0f;
    }

    public void ZoomOut() {
        var newY = targetPositionOffset.y * 1.2f;
        targetPositionOffset = new Vector3(0, newY, 0);
        if (targetPositionOffset.y > 500) targetPositionOffset = new Vector3(0, 500, 0);
        timer = 0f;
    }

}
