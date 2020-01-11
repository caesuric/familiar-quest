using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverHotbarButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public GameObject mouseOverCanvas;
    public Text title;
    public Text description;
    public int number;
    private Vector3 mouseOverCanvasOriginalPosition = Vector3.zero;
    public static List<MouseOverHotbarButton> list = new List<MouseOverHotbarButton>();
    public PlayerCharacter pc;
    private MeshRenderer hitbox = null;
    public MeshRenderer rangedHitbox;
    public MeshRenderer aoe;
    private Vector3 aoeScale;
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();
    private GameObject originalParent;
    private Vector3 originalPosition;
    public Image image;
    public GameObject hoverOverlay;

    // Use this for initialization
    void Start() {
        mouseOverCanvasOriginalPosition = mouseOverCanvas.transform.position;
        list.Add(this);
        aoeScale = aoe.transform.localScale;
        originalParent = transform.parent.gameObject;
        originalPosition = transform.position;
        hoverOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (pc == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) pc = item.GetComponent<PlayerCharacter>();
        }
        if (pc != null && hitbox == null) hitbox = pc.gameObject.GetComponentInChildren<HitboxDealDamage>().GetComponent<MeshRenderer>();
    }

    public void OnMouseEnter() {
        //mouseOverCanvas.SetActive(true);
        //mouseOverCanvas.transform.position = mouseOverCanvasOriginalPosition + new Vector3(67f * number, 0, 0);
        //if (mouseOverCanvas.transform.position.x > Screen.width - 350) mouseOverCanvas.transform.position = new Vector3(Screen.width - 250, mouseOverCanvas.transform.position.y, mouseOverCanvas.transform.position.z);
        //var mousePos = new Vector3(Input.mousePosition.x + 200, Input.mousePosition.y + 10, 0);
        //mouseOverCanvas.transform.position = mousePos;
        if (number < pc.GetComponent<HotbarUser>().abilityNames.Count) EnableTooltip();
        else DisableTooltip();
        hoverOverlay.SetActive(true);
    }

    private void EnableTooltip() {
        //if (number==10 || number==11) {
        //    var potionNumber = number - 10;
        //    if (pc.GetComponent<PlayerCharacter>().consumables.Count <= potionNumber) return;
        //    title.text = pc.GetComponent<PlayerCharacter>().consumables[potionNumber].name;
        //    description.text = pc.GetComponent<PlayerCharacter>().consumables[potionNumber].description;
        //}
        //if (pc.GetComponent<HotbarUser>().abilityIsRanged.Count <= number || pc.GetComponent<HotbarUser>().abilityIsAttack.Count <= number || pc.GetComponent<HotbarUser>().abilityNames.Count <= number || pc.GetComponent<HotbarUser>().abilityDescriptions.Count <= number) return;
        //title.text = pc.GetComponent<HotbarUser>().abilityNames[number];
        //description.text = pc.GetComponent<HotbarUser>().abilityDescriptions[number];
        if (!pc.GetComponent<HotbarUser>().abilityIsRanged[number] && pc.GetComponent<HotbarUser>().abilityIsAttack[number]) ShowMeleeAttackTooltip();
        else if (pc.GetComponent<HotbarUser>().abilityIsRanged[number]) ShowRangedAttackTooltip();
        if (pc.GetComponent<HotbarUser>().abilityRadii[number] > 0) ShowAoeRadius();
        else aoe.enabled = false;
    }

    private void ShowAoeRadius() {
        hitbox.enabled = false;
        aoe.enabled = true;
        aoe.transform.localScale = aoeScale * pc.GetComponent<HotbarUser>().abilityRadii[number];
        if (pc.GetComponent<HotbarUser>().abilityIsRanged[number]) {
            Physics.Raycast(pc.transform.position, pc.transform.forward, out RaycastHit hitInfo);
            aoe.transform.position = hitInfo.point;
        }
        else aoe.transform.position = pc.transform.position;
    }

    private void ShowMeleeAttackTooltip() {
        hitbox.enabled = true;
        rangedHitbox.enabled = false;
    }

    private void ShowRangedAttackTooltip() {
        hitbox.enabled = false;
        rangedHitbox.enabled = true;
        rangedHitbox.transform.position = pc.transform.position + (pc.transform.forward * 11.06f);
        rangedHitbox.transform.rotation = pc.transform.rotation;
    }

    private void DisableTooltip() {
        //mouseOverCanvas.SetActive(false);
        hitbox.enabled = false;
        rangedHitbox.enabled = false;
        aoe.enabled = false;
    }

    public void OnMouseExit() {
        //mouseOverCanvas.SetActive(false);
        hitbox.enabled = false;
        rangedHitbox.enabled = false;
        aoe.enabled = false;
        hoverOverlay.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;
        InputMovement.isDragging = true;
        transform.SetParent(canvas.transform);
        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        var group = gameObject.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        mouseOverCanvas.SetActive(false);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        mouseOverCanvas.SetActive(false);
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        InputMovement.isDragging = false;
        transform.SetParent(originalParent.transform);
        transform.position = originalPosition;
        m_DraggingPlanes.Remove(eventData.pointerId);
        Destroy(gameObject.GetComponent<CanvasGroup>());
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
