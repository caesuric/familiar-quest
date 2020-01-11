using UnityEngine;
using UnityEngine.UI;

public class GameLog : MonoBehaviour {

    public static GameLog instance = null;
    public GameObject textParent;
    public GameObject textPrefab;
    public Scrollbar scrollbar;
    public ScrollRect scrollRect;

    // Use this for initialization
    void Start() {
        if (instance != null) Destroy(this);
        else {
            instance = this;
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update () {
    //var value = scrollbar.value;
    //if (value == 0) scrollRect.verticalNormalizedPosition = 0;
    //}

    public static void AddText(string textToAdd) {
        textToAdd = textToAdd.Replace("(Clone)", "");
        var newText = Instantiate(instance.textPrefab, instance.textParent.transform);
        var textComponent = newText.GetComponent<Text>();
        textComponent.text = textToAdd;
        instance.scrollRect.verticalNormalizedPosition = 0;
    }
}