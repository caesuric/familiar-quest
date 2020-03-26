using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigGrabber : MonoBehaviour {

    //public InitializeLevel il;
    //public SceneInitializer si;
    public OverworldInitializer overworldInitializer;
    public InitializeOverlordMode initializeOverlordMode;
    public QuestLog questLog = null;
    private bool initialized = false;

    // Update is called once per frame

    void Start() {
        var config = GameObject.FindGameObjectWithTag("ConfigObject");
        if (config == null) return;
        //il = InitializeLevel.instance;
        //si = SceneInitializer.instance;
        overworldInitializer = config.GetComponent<OverworldInitializer>();
        initializeOverlordMode = config.GetComponent<InitializeOverlordMode>();
        questLog = new QuestLog();
        initialized = true;
    }
}
