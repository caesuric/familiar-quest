using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    public void OnBeginDrag(PointerEventData eventData) {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;
        transform.SetParent(canvas.transform);
        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        var group = gameObject.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        GetComponent<InventoryItemUpdater>().details.SetActive(false);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        GetComponent<InventoryItemUpdater>().details.SetActive(false);
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        GetComponent<InventoryItemUpdater>().inventory.sharedInventory.CmdRefresh();
        GetComponent<InventoryItemUpdater>().inventory.RefreshInABit();
        Destroy(gameObject);
    }

    public void SetDraggedPosition(PointerEventData eventData) {
        var rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos)) {
            rt.position = globalMousePos;
        }
    }

    static public T FindInParents<T>(GameObject go) where T : Component {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null) {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
