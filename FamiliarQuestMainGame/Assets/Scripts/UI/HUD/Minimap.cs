using UnityEngine;
using System.Collections.Generic;

public class Minimap : MonoBehaviour {

    public Mapper mapper = null;
    public GameObject wall;
    public GameObject door;
    public GameObject fountain;
    public GameObject stairsDown;
    public GameObject stairsUp;
    public GameObject navDot;
    public GameObject playerDotPrefab;
    public PartyStatusTracker status = null;
    public Dictionary<uint, GameObject> otherPlayerDots = new Dictionary<uint, GameObject>();
    private RectTransform navRect;
    private float mapSizeX;
    private float mapSizeY;
    private float mapOverallSize;
    private float tileSize;
    private List<GameObject> dots = new List<GameObject>();
    private Dictionary<string, GameObject> prefabs;

    // Use this for initialization
    void Start() {
        var rect = GetComponent<RectTransform>();
        var rect2 = rect.rect;
        mapSizeX = rect2.width;
        mapSizeY = rect2.height;
        mapOverallSize = Mathf.Min(mapSizeX, mapSizeY);
        tileSize = mapOverallSize / 30;
        navRect = navDot.GetComponent<RectTransform>();
        navRect.sizeDelta = new Vector2(tileSize * 5f / 2f, tileSize * 5f / 2f);
        InitializePrefabs();
    }

    private void InitializePrefabs() {
        prefabs = new Dictionary<string, GameObject>() {
            {"W", wall},
            {"F", fountain},
            {">", stairsDown},
            {"<", stairsUp},
            {"D", door}
        };
    }

    // Update is called once per frame
    void Update() {
        if (mapper == null || status == null) Initialize();
        if (mapper == null || status == null) return;
        UpdateNavDots();
        AdjustMapPosition();
        AddNewHits();
    }

    private void Initialize() {
        var players = PlayerCharacter.players;
        foreach (var item in players) if (item.isMe) mapper = item.GetComponent<Mapper>();
        var obj = GameObject.FindGameObjectWithTag("ConfigObject");
        if (obj != null) status = obj.GetComponent<PartyStatusTracker>();
    }

    private void UpdateNavDots() {
        if (status.id.Count - 1 != otherPlayerDots.Count) {
            foreach (var kvp in otherPlayerDots) Destroy(kvp.Value);
            otherPlayerDots.Clear();
        }
        for (int i = 0; i < status.id.Count; i++) {
            //if (status.id[i] != status.localPlayer.netId.Value)
            //{
            if (!otherPlayerDots.ContainsKey(status.id[i])) otherPlayerDots.Add(status.id[i], CreateNewDot());
            UpdateDot(otherPlayerDots[status.id[i]], status.posX[i], status.posY[i]);
            //}
        }
    }

    private void AdjustMapPosition() {
        transform.localPosition = -new Vector3(mapper.transform.position.x * tileSize / 2, mapper.transform.position.z * tileSize / 2, 0);
        navRect.localPosition = new Vector2(mapper.transform.position.x * tileSize / 2, mapper.transform.position.z * tileSize / 2);
    }

    private void AddNewHits() {
        for (int i = 0; i < Mapper.newHits.Count; i++) AddNewHit(i);
        Mapper.newHits.Clear();
        Mapper.newHitsItems.Clear();
    }

    private void AddNewHit(int i) {
        GameObject newItem = null;
        switch (Mapper.newHitsItems[i]) {
            case "{REMOVE}":
                RemoveDot(i);
                return;
            default:
                newItem = Instantiate(prefabs[Mapper.newHitsItems[i]], transform);
                break;
        }
        PositionNewDot(newItem, i);
    }

    private void PositionNewDot(GameObject newItem, int i) {
        if (newItem == null) return;
        var rect = newItem.GetComponent<RectTransform>();
        //rect.localPosition = new Vector2(((Mapper.newHits[i].x * 2) - 120) * tileSize / 2, ((Mapper.newHits[i].y * 2) - 120) * tileSize / 2);
        rect.localPosition = new Vector2(((Mapper.newHits[i].x * 5)) * tileSize / 2, ((Mapper.newHits[i].y * 5)) * tileSize / 2);
        rect.sizeDelta = new Vector2(tileSize * 1.1f * 5f / 2f, tileSize * 1.1f * 5f / 2f);
        var script = newItem.GetComponent<MinimapDot>();
        script.x = Mapper.newHits[i].x;
        script.y = Mapper.newHits[i].y;
        dots.Add(newItem);
    }

    private void RemoveDot(int i) {
        foreach (var dot in dots) {
            var item = dot.GetComponent<MinimapDot>();
            if (item != null && item.x == Mapper.newHits[i].x && item.y == Mapper.newHits[i].y) {
                dots.Remove(dot);
                Destroy(dot);
                break;
            }
        }
    }

    private GameObject CreateNewDot() {
        var obj = Instantiate(playerDotPrefab);
        obj.transform.SetParent(transform);
        return obj;
    }


    private void UpdateDot(GameObject obj, float x, float y) {
        var rect = obj.GetComponent<RectTransform>();
        //rect.localPosition = new Vector2(((x * 1) - 0) * tileSize / 2, ((y * 1) - 0) * tileSize / 2);
        rect.localPosition = new Vector2(x * tileSize / 2, y * tileSize / 2);
        rect.sizeDelta = new Vector2(tileSize * 1.1f * 5f / 2f, tileSize * 1.1f * 5f / 2f);
    }
}
