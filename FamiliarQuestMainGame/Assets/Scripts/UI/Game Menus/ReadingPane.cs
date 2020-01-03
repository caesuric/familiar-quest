using UnityEngine;
using UnityEngine.UI;

public class ReadingPane : MonoBehaviour {

    public Text titleObj;
    public Text textObj;

    public void SetText(string title, string text) {
        titleObj.text = title;
        textObj.text = text;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
