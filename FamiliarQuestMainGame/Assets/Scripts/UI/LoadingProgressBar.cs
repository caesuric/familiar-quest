using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    public Text text;
    public MOBAEnergyBar progressBar;
    public static LoadingProgressBar instance = null;
    public bool isLoading = false;
    public int loadingPhase = 0;
    public int lastPhase = 0;
    public AsyncOperation ao = null;
    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading && !text.gameObject.activeSelf) {
            text.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);
        }
        else if (!isLoading && text.gameObject.activeSelf) {
            text.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
        }
        if (isLoading && loadingPhase == 0 && ao != null) {
            progressBar.Value = ao.progress * 100f;
            text.text = "Loading Scene...";
        }
        else if (isLoading) {
            progressBar.Value = OverworldTerrainGenerator.instance.progress * 100f;
        }
        if (isLoading && loadingPhase == 0 && lastPhase == 0 && ao.progress == 1f) EndLoad();
    }

    public static void StartLoad(AsyncOperation ao, int lastPhase) {
        if (instance == null) return;
        instance.lastPhase = lastPhase;
        instance.isLoading = true;
        instance.loadingPhase = 0;
        instance.ao = ao;
        instance.progressBar.Value = 0;
    }

    public static void StartSecondaryLoadPhase() {
        instance.loadingPhase = 1;
        instance.progressBar.Value = 0;
    }

    public static void EndLoad() {
        instance.isLoading = false;
        instance.loadingPhase = 0;
        instance.ao = null;
        instance.progressBar.Value = 0;
    }

    public static void UpdateProgressText(string text) {
        instance.text.text = text + "...";
    }
}
