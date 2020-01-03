using UnityEngine;

public class OverworldInitializer : MonoBehaviour {
    //[Command]
    public void CmdSetCharacterPosition(GameObject go) {
        go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
    }
}
