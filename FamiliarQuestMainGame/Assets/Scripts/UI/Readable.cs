using UnityEngine;

public class Readable : MonoBehaviour {

    public string title;
    [TextArea]
    public string text;
    private ReadingPane readingPane = null;

    void Start() {
        FindReadingPane();
    }

    private void FindReadingPane() {
        readingPane = GameObject.FindGameObjectWithTag("ReadingPane").GetComponent<ReadingPane>();
    }

    public void Use() {
        if (readingPane == null) FindReadingPane();
        readingPane.gameObject.SetActive(true);
        readingPane.SetText(title, text);
    }
}
