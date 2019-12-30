using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler {
    public delegate void plainDelegate();
    public plainDelegate onClick = null;
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right && onClick != null) onClick();
    }
}
