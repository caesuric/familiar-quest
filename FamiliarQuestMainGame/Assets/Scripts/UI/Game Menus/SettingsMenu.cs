using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {

    public GameObject minimap;
    public GameObject fps;
    public GameObject controls;
    public GameObject abilityGuide;
    public GameObject changelog;
    public GameObject gameLog;
    public DuloGames.UI.UISliderExtended graphicsSettingsSlider;
    public DuloGames.UI.UISwitchSelect resolutionSlider;
    public List<Resolution> resolutionOptions = new List<Resolution>();
    private bool initialized = false;
    private bool exiting = false;

    // Update is called once per frame
    void Update() {
        if (!initialized) {
            initialized = true;
        }

        if (exiting && !PlayerCharacter.localPlayer.GetComponent<AutoSaver>().currentlySaving) {
            exiting = false;
            //InitializeLevel.ResetGame();
        }
    }

    public void ToggleMinimap() {
        minimap.SetActive(!minimap.activeSelf);
    }

    public void ToggleFps() {
        fps.SetActive(!fps.activeSelf);
    }

    public void ToggleGamepad() {
        var ic = PlayerCharacter.localPlayer.GetComponent<InputController>();
        ic.gamepadMode = !ic.gamepadMode;
    }

    public void ToggleControlsReference() {
        controls.SetActive(!controls.activeSelf);
    }

    public void ToggleAbilityGuide() {
        abilityGuide.SetActive(!abilityGuide.activeSelf);
    }

    public void ToggleChangelog() {
        changelog.SetActive(!changelog.activeSelf);
    }

    public void Unstuck() {
        LevelGen.instance.Unstuck();
    }

    public void ToggleGameLog() {
        gameLog.SetActive(!gameLog.activeSelf);
    }

    public void SaveAndExit() {
        if (!AutoHealer.OutOfCombat()) return;
        PlayerCharacter.localPlayer.GetComponent<AutoSaver>().CmdSave();
        exiting = true;
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void GraphicsSettingsChanged() {
        var value = graphicsSettingsSlider.value;
        if (value == 0) QualitySettings.SetQualityLevel(1);
        else if (value == 1) QualitySettings.SetQualityLevel(4);
        else if (value == 2) QualitySettings.SetQualityLevel(6);
    }

    public void ToggleFullscreen(bool value) {
        Screen.fullScreen = value;
    }

    public void ToggleSettingsMenu() {
        GetComponent<DuloGames.UI.UIWindow>().Toggle();
        RefreshResolutions();
    }

    public void RefreshResolutions() {
        resolutionSlider.options.Clear();
        var resolutions = Screen.resolutions;
        resolutionOptions.Clear();
        foreach (var resolution in resolutions) {
            resolutionSlider.options.Add(resolution.ToString());
            resolutionOptions.Add(resolution);
        }
    }

    public void SelectResolution(int value, string strValue) {
        var actualResolution = resolutionOptions[value];
        Screen.SetResolution(actualResolution.width, actualResolution.height, Screen.fullScreen, actualResolution.refreshRate);
    }
}
