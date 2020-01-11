using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectCamera : MonoBehaviour {

    public GameObject characterModel;
    public float dragSpeedMultiplier = 0.01f;
    public float cameraHeight = 2.5f;
    public float minY = -60;
    public float maxY = -10;
    public Texture2D cursorTexture;
    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private float offsetX = 1.57f;
    private float offsetY = -17;
    private float cameraRadius = 8;
    private GameObject canvas;

    // Use this for initialization
    void Start() {
        gameObject.transform.LookAt(characterModel.transform);
    }

    // Update is called once per frame
    void Update() {
        var wheel = Input.GetAxis("Mouse ScrollWheel");
        cameraRadius = Mathf.Clamp(cameraRadius - wheel * 20, 3, 10);
        if (ClickIsOnUi()) {
            isDragging = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            if (isDragging) DragCamera();
            isDragging = true;
            lastMousePosition = Input.mousePosition;
            Cursor.SetCursor(cursorTexture, new Vector2(256, 256), CursorMode.Auto);
        }
        else if (wheel != 0) {
            lastMousePosition = Input.mousePosition;
            DragCamera();
        }
        else {
            isDragging = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void DragCamera() {
        var draggedDiff = Input.mousePosition - lastMousePosition;
        offsetX += draggedDiff.x * dragSpeedMultiplier;
        offsetY += draggedDiff.y * dragSpeedMultiplier / 2f;
        offsetX = LimitAngle(offsetX);
        offsetY = LimitAngle(offsetY, true);
        var rotation = Quaternion.Euler(offsetY, offsetX, 0);
        var position = rotation * new Vector3(0f, 0f, cameraRadius) + characterModel.transform.position;
        Camera.main.transform.position = position;
        gameObject.transform.LookAt(characterModel.transform);
    }

    private float LimitAngle(float input, bool vertical = false) {
        if (input <= -360) input += 360;
        if (input >= 360) input -= 360;
        if (vertical && input < minY) input = minY;
        if (vertical && input > maxY) input = maxY;
        return input;
    }

    private bool ClickIsOnUi() {
        if (canvas == null) canvas = GameObject.FindGameObjectWithTag("Canvas");
        var caster = canvas.GetComponent<GraphicRaycaster>();
        var pointerEventData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(pointerEventData, results);
        if (results.Count > 0 && results[0].gameObject.name != "Large Status Text" && results[0].gameObject.name != "Minimap" && results[0].gameObject.name != "Party Health Pane" && !results[0].gameObject.name.Contains("Minimap") && results[0].gameObject.name != "Canvas") return true;
        return false;
    }
}
