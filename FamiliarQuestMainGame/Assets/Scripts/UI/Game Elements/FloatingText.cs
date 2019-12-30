using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FloatingText : MonoBehaviour {

    public string text;
    public Color color;
    public int size;
    private Color baseColor;
    private bool ready = false;
    private float timeLeft = 6.0f;
    private float fadeTime = 4f;
    private float hangTimeMax = 4f;
    private float hangTime = 0f;
    private TextMesh textMesh;
    public static List<FloatingText> objects = new List<FloatingText>();
    private float speed = 0.04f;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        objects.Add(this);
	}

    // Update is called once per frame
    void Update() {
        if (!ready) Initialize();
        if (timeLeft < 0) {
            objects.Remove(this);
            Destroy(gameObject);
        }
        if (!Collisions()) {
            timeLeft -= Time.deltaTime;
            MoveAndDim();
        }
        else {
            hangTime += Time.deltaTime;
            if (hangTime>=hangTimeMax) {
                hangTime = 0;
                speed *= 2;
            }
        }
    }

    private bool Collisions() {
        foreach (var obj in objects) {
            if (obj == this) return false;
            if (obj == null) {
                objects.Remove(obj);
                return false;
            }
            if (Vector3.Distance(obj.transform.position, this.transform.position) < 0.5) return true;
        }
        return false;
    }

    private void MoveAndDim() {
        gameObject.transform.Translate(0, speed, 0);
        if (timeLeft < fadeTime)
        {
            float fade = timeLeft / fadeTime;
            var newColor = baseColor;
            newColor.a = fade;
            textMesh.color = newColor;
        }
	}

    private void Initialize()
    {
        ready = true;
        textMesh.text = text;
        textMesh.color = color;
        baseColor = color;
        textMesh.fontSize = size;
        gameObject.transform.Translate(0, 2f, 0);
    }
}
