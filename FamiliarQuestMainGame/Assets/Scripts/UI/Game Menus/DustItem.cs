using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DustItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Dust dust;
    public Text quantityText;

    private readonly Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    public void Initialize(Dust dust) {
        this.dust = dust;
        transform.localScale = new Vector3(1, 1, 1);
        if (((int)dust.quantity) < 1) quantityText.text = "x<1";
        else quantityText.text = "x" + ((int)dust.quantity).ToString();
        quantityText.text = "Dust of " + dust.type + " " + quantityText.text;
    }

    public void OnMouseEnter() {
        var abilityScreen = AbilityMenu.instance;
        abilityScreen.mouseOverTitle.text = "Dust of " + dust.type;
        abilityScreen.mouseOverDescription.text = quantityText.text;
        var mousePos = new Vector3(Input.mousePosition.x + 200, Input.mousePosition.y + 10, 0);
        abilityScreen.mouseOverPanel.SetActive(true);
        abilityScreen.mouseOverPanel.transform.position = mousePos;
    }

    public void OnMouseExit() {
        var abilityScreen = AbilityMenu.instance;
        abilityScreen.mouseOverPanel.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;
        transform.SetParent(canvas.transform);
        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        var group = gameObject.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (AbilityMenu.instance.dustToUse.Contains(dust)) AbilityMenu.instance.dustToUse.Remove(dust);
        AbilityMenu.instance.RefreshDustUsagePanel();
        AbilityMenu.instance.UpdateAbilities();
        Destroy(gameObject);
    }

    public void SetDraggedPosition(PointerEventData eventData) {
        var rt = GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos)) {
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
