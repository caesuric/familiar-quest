﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour {

    public GameObject audioSource;
    // Use this for initialization
    void Start() {
        if (audioSource!=null) Instantiate(audioSource, gameObject.transform);
    }
}
