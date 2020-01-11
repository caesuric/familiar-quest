using UnityEngine;
using UnityEngine.UI;

public class PartyHealthUpdater : MonoBehaviour {

    public float hp;
    public bool targeted = false;
    public Outline outline;
    public string characterName;
    public Text nameText;

    // Update is called once per frame
    void Update() {
        transform.localScale = new Vector3(hp, 1, 1);
        outline.enabled = targeted;
        nameText.text = characterName;
    }
}
