using UnityEngine;

public class InitializeOverlordMode : MonoBehaviour {
    public GameObject overlordCameras;

    //[Command]
    public void CmdSetupCharacter(GameObject go) {
        go.transform.position = new Vector3(0, 145, 0);
    }

    public void SetupOverlord(GameObject go) {
        go.transform.position = new Vector3(0, 300, 0);
        foreach (var mesh in go.GetComponentsInChildren<MeshRenderer>()) mesh.enabled = false;
        PlayerCharacter.players.Remove(go.GetComponent<PlayerCharacter>());
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null) mainCamera.SetActive(false);
        overlordCameras.SetActive(true);
        // setup alternate control scheme
        go.GetComponent<InputController>().enabled = false;
        // setup alternate HUD
        var playerInterface = GameObject.FindGameObjectWithTag("PlayerInterface");
        if (playerInterface != null) playerInterface.SetActive(false);
        // setup starter room
    }

    public void SetupCharacter() {
        var overlordInterface = GameObject.FindGameObjectWithTag("OverlordInterface");
        if (overlordInterface != null) overlordInterface.SetActive(false);
    }
}