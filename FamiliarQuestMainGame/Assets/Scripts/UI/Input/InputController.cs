using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public float speed;
    public GameObject hips;
    private GameObject inventory = null;
    private GameObject spiritScreen = null;
    private GameObject characterSheet = null;
    public GameObject minimap = null;
    private GameObject fpsBar;
    public GameObject inventoryDetails = null;
    private GameObject abilityScreen = null;
    private GameObject settingsMenu = null;
    private GameObject controlsReference = null;
    private GameObject abilityGuide = null;
    private GameObject changelog = null;
    private GameObject readingPane = null;
    private GameObject mouseOverPanel = null;
    public new Rigidbody rigidbody = null;
    public Character character = null;
    private bool inventoryActive = false;
    private bool characterSheetActive = false;
    private bool spiritScreenActive = false;
    public bool minimapActive = true;
    private bool fpsActive = true;
    public bool dPadActive = false;
    private Inventory inventoryController;
    private SpiritScreen spiritScreenController;
    //[SyncVar]
    public bool moving = false;
    //[SyncVar]
    public bool dead = false;
    //[SyncVar]
    public bool stealthy = false;
    //[SyncVar]
    public float posX;
    //[SyncVar]
    public float posY;
    //[SyncVar]
    public float rotation;
    //[SyncVar]
    public int currentAbility = 0;
    //[SyncVar]
    public int currentAltAbility = 1;
    public bool fusionMode = false;
    public int fusionTarget1 = -1;
    public int fusionTarget2 = -1;
    public bool gamepadMode = false;
    public UnityEngine.PostProcessing.PostProcessingProfile stealthEffect;
    public UnityEngine.PostProcessing.PostProcessingProfile normalEffect;
    private Dictionary<string, Action> inputs = new Dictionary<string, Action>();
    private PlayerAnimation animationController;
    GameObject[] hotbarButtons;

    // Use this for initialization
    void Start() {
        animationController = new PlayerAnimation(gameObject);
        //if (!isLocalPlayer) return;
        hotbarButtons = GameObject.FindGameObjectsWithTag("HotbarButton");
        foreach (var button in hotbarButtons) {
            var component = button.GetComponent<Button>();
            var mohb = button.GetComponent<MouseOverHotbarButton>();
            if (mohb == null) continue;
            if (mohb.number == 12) continue;
            component.onClick.AddListener(() => AssignMouseToHotbarButton(mohb.number));
            if (button.GetComponent<RightClickHandler>() != null) button.GetComponent<RightClickHandler>().onClick = () => AssignAltMouseToHotbarButton(mohb.number);
        }
        //inputs = new Dictionary<string, Action>() { { "SwitchTarget", CmdSwitchTarget }, { "Cancel", ToggleInventory }, { "SpiritScreen", ToggleSpiritScreen }, { "Minimap", ToggleMinimap }, { "Fuse", ToggleFusion }, { "Toggle Gamepad", ToggleGamepadMode }, { "Toggle FPS", ToggleFPS }, { "Character Sheet", ToggleCharacterSheet } };
        inputs = new Dictionary<string, Action>() { { "SwitchTarget", SwitchTarget }, { "Cancel", ClearWindows }, { "Use Item", UseWorldObject }, { "Character Sheet", ToggleCharacterSheet }, { "Inventory", ToggleInventory } };
    }

    public void Restart() {
        //if (!isLocalPlayer) return;
        hotbarButtons = GameObject.FindGameObjectsWithTag("HotbarButton");
        foreach (var button in hotbarButtons) {
            var component = button.GetComponent<Button>();
            if (component == null) return;
            var mohb = button.GetComponent<MouseOverHotbarButton>();
            if (mohb == null) continue;
            component.onClick.AddListener(() => AssignMouseToHotbarButton(mohb.number));
            if (button.GetComponent<RightClickHandler>() != null) button.GetComponent<RightClickHandler>().onClick = () => AssignAltMouseToHotbarButton(mohb.number);
        }
    }

    private void Initialize() {
        //if (!isLocalPlayer) return;
        if (ProtoClient.localPlayer != GetComponent<NetworkCharacterSyncer>()) return;
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        inventoryDetails = GameObject.FindGameObjectWithTag("InventoryDetails");
        //spiritScreen = GameObject.FindGameObjectWithTag("SpiritScreen");
        //minimap = GameObject.FindGameObjectWithTag("Minimap");
        //fpsBar = GameObject.FindGameObjectWithTag("FPSBar");
        characterSheet = GameObject.FindGameObjectWithTag("CharacterSheet");
        abilityScreen = GameObject.FindGameObjectWithTag("AbilityScreen");
        //settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
        //controlsReference = GameObject.FindGameObjectWithTag("ControlsReference");
        //abilityGuide = GameObject.FindGameObjectWithTag("AbilityGuide");
        changelog = GameObject.FindGameObjectWithTag("Changelog");
        readingPane = GameObject.FindGameObjectWithTag("ReadingPane");
        //mouseOverPanel = GameObject.FindGameObjectWithTag("MouseOverPanel");
        //if (inventory == null || spiritScreen == null || minimap == null || fpsBar == null || characterSheet == null) return;
        //inventoryController = inventory.GetComponent<Inventory>();
        //GetComponent<PlayerCharacter>().inventory = inventoryController;
        //spiritScreenController = spiritScreen.GetComponent<SpiritScreen>();
        //inventory.SetActive(false);
        //spiritScreen.SetActive(false);
        //characterSheet.SetActive(false);
        //abilityScreen.SetActive(false);
        //settingsMenu.SetActive(false);
        //controlsReference.SetActive(false);
        //abilityGuide.SetActive(false);
        changelog.SetActive(false);
        readingPane.SetActive(false);
        //mouseOverPanel.SetActive(false);
        inventoryDetails.SetActive(false);
        inventory.GetComponent<DuloGames.UI.UIWindow>().Toggle();
        characterSheet.GetComponent<DuloGames.UI.UIWindow>().Toggle();
        abilityScreen.GetComponent<DuloGames.UI.UIWindow>().Toggle();
        rigidbody = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        GetComponent<InputAbilities>().Initialize(this);
        InputMovement.Initialize(this);
    }

    // Update is called once per frame
    private void Update() {
        //if (!isLocalPlayer) return;
        if (!moving) {
            var euler = hips.transform.eulerAngles;
            hips.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
        }
        if (hotbarButtons == null || hotbarButtons.Length == 0 || hotbarButtons[0] == null) Restart();
        if (inventory == null || characterSheet == null || rigidbody == null || character == null) Initialize();
        if (inventory == null || characterSheet == null || rigidbody == null || character == null) return;
        if (character.GetComponent<Health>().hp <= 0 || character == null) return;
        GetComponent<InputAbilities>().KeyboardCheck();
        GetComponent<InputAbilities>().RightStickCheck();
        GetComponent<InputAbilities>().DPadCheck();
        MinimapScrollCheck();
        foreach (string input in inputs.Keys) if (Input.GetButtonDown(input)) inputs[input]();
    }

    private void MinimapScrollCheck() {
        if (Input.mouseScrollDelta.y > 0) MinimapCameraLock.instance.GetComponent<MinimapCameraLock>().ZoomIn();
        else if (Input.mouseScrollDelta.y < 0) MinimapCameraLock.instance.GetComponent<MinimapCameraLock>().ZoomOut();
    }

    private void ToggleCharacterSheet() {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        SharedInventory.instance.CmdRefresh();
        canvas.GetComponent<Inventory>().Refresh();
        characterSheet.GetComponent<DuloGames.UI.UIWindow>().Toggle();
    }

    private void ToggleInventory() {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        SharedInventory.instance.CmdRefresh();
        canvas.GetComponent<Inventory>().Refresh();
        //inventory.SetActive(!inventory.activeSelf);
        inventory.GetComponent<DuloGames.UI.UIWindow>().Toggle();
    }

    //private void ToggleInventory() {
    private void ClearWindows() {
        //inventoryActive = !inventoryActive;
        //inventory.SetActive(inventoryActive);
        //inventory.GetComponent<Inventory>().inventoryDetails.SetActive(false);
        //spiritScreenActive = false;
        //spiritScreen.SetActive(false);
        //characterSheetActive = false;
        //characterSheet.SetActive(false);
        //inventoryController.Refresh();
        //inventory.SetActive(false);
        //spiritScreen.SetActive(false);
        //characterSheet.SetActive(false);
        //abilityScreen.SetActive(false);
        //settingsMenu.SetActive(false);
        //controlsReference.SetActive(false);
        //abilityGuide.SetActive(false);
        //changelog.SetActive(false);
        readingPane.SetActive(false);
        //mouseOverPanel.SetActive(false);
        inventoryDetails.SetActive(false);
        InputMovement.isDragging = false;
    }

    private void ToggleSpiritScreen() {
        spiritScreenActive = !spiritScreenActive;
        spiritScreen.SetActive(spiritScreenActive);
        inventoryActive = false;
        inventory.SetActive(false);
        characterSheetActive = false;
        characterSheet.SetActive(false);
        spiritScreenController.Refresh();
    }

    private void ToggleMinimap() {
        minimapActive = !minimapActive;
        minimap.SetActive(minimapActive);
    }

    private void ToggleGamepadMode() {
        gamepadMode = !gamepadMode;
    }

    private void ToggleFusion() {
        fusionMode = !fusionMode;
        if (fusionMode) {
            fusionTarget1 = -1;
            fusionTarget2 = -1;
        }
    }

    private void ToggleFPS() {
        fpsActive = !fpsActive;
        fpsBar.SetActive(fpsActive);
    }

    void LateUpdate() {
        animationController.Animate();
        if (!GetComponent<PlayerCharacter>().isMe || GetComponent<Health>().hp <= 0) return;
        InputMovement.KeyboardCheck();
        InputMovement.MouseCheck();
    }

    //[Command]
    public void CmdSetMoving(bool state) {
        moving = state;
    }

    //[Command]
    public void CmdUseHotbarAbility(int i, int j) {
        if (fusionMode) {
            //if (fusionTarget1 == -1) fusionTarget1 = j;
            //else {
            //    fusionTarget2 = j;
            //    FuseIfPossible();
            //}
        }
        else {
            if (!gamepadMode) {
                FaceMouse();
                RpcFaceMouse();
            }
            GetComponent<HotbarUser>().UseHotbarAbility(i, j);
        }
    }

    //[Command]
    public void CmdUsePrimaryAbility(Vector3 angle) {
        //if (!gamepadMode) {
        //    FaceMouse();
        //    RpcFaceMouse();
        //}
        transform.eulerAngles = angle;
        GetComponent<HotbarUser>().UseHotbarAbility(0, currentAbility);
    }

    //[Command]
    public void CmdUseSecondaryAbility(Vector3 angle) {
        //if (!gamepadMode) {
        //    FaceMouse();
        //    RpcFaceMouse();
        //}
        transform.eulerAngles = angle;
        GetComponent<HotbarUser>().UseHotbarAbility(0, currentAltAbility);
    }

    //[Command]
    public void CmdAttack() {
        if (character.GetComponent<AbilityUser>().GCDTime > 0 || character.GetComponent<Health>().hp <= 0) return;
        FaceMouse();
        RpcFaceMouse();
        if (GetComponent<PlayerCharacter>().weapon is RangedWeapon) GetComponent<Attacker>().RangedWeaponAttack();
        else StartCoroutine(GetComponent<Attacker>().ActivateMeleeHitbox());
    }

    //[ClientRpc]
    private void RpcFaceMouse() {
        //if (isLocalPlayer) FaceMouse();
    }

    private void FaceMouse() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) transform.LookAt(hit.point, transform.up);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void AssignMouseToHotbarButton(int num) {
        currentAbility = num;
    }

    public void AssignAltMouseToHotbarButton(int num) {
        currentAltAbility = num;
    }

    //[Command]
    public void CmdUseItem(int num) {
        GetComponent<HotbarUser>().UseItem(num);
    }

    public void SwitchTarget() {
        CmdSwitchTarget();
    }

    //[Command]
    public void CmdSwitchTarget() {
        var pst = GameObject.FindGameObjectWithTag("ConfigObject").GetComponent<PartyStatusTracker>();
        var player = GetComponent<PlayerCharacter>();
        for (int i = 0; i < pst.id.Count; i++) {
            if (player.target == pst.id[i]) {
                if (i < pst.id.Count - 1) player.target = pst.id[i + 1];
                else player.target = pst.id[0];
                return;
            }
        }
    }

    //[Command]
    public void CmdSetPosition(float x, float y, float rot) {
        posX = x;
        posY = y;
        rotation = rot;
    }

    public void UseWorldObject() {
        CmdUseWorldObject();
    }

    //[Command]
    public void CmdUseWorldObject() {
        if (abilityScreen.GetComponent<DuloGames.UI.UIWindow>().IsOpen) return;
        var worldObj = GetNearestUsableObject();
        if (worldObj == null) return;
        worldObj.Use(gameObject);
    }

    public UsableObject GetNearestUsableObject() {
        var hits = Physics.OverlapSphere(transform.position, 2f);
        UsableObject nearestObj = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hit in hits) {
            var usable = hit.gameObject.GetComponent<UsableObject>();
            if (usable == null) continue;
            var distance = Vector3.Distance(usable.transform.position, transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                nearestObj = usable;
            }
        }
        return nearestObj;
    }
}