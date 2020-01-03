using UnityEngine;

public class Readable : MonoBehaviour {

    public string title;
    [TextArea]
    public string text;
    private ReadingPane readingPane = null;

    void Start() {
        readingPane = GameObject.FindGameObjectWithTag("ReadingPane").GetComponent<ReadingPane>();
    }

    public void Use() {
        readingPane.gameObject.SetActive(true);
        readingPane.SetText(title, text);
    }
}
