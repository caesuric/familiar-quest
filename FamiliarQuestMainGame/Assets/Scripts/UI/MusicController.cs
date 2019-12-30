using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public static MusicController instance = null;
    public AudioClip menuMusic;
    public AudioClip dungeonMusic;
    public AudioClip townMusic;
    public AudioClip fortressMusic;
    public AudioClip cavesAndMinesMusic;
    public AudioClip bossMusic;
    public AudioClip overworldMusic;
    public AudioClip sewersMusic;
    public AudioClip templeMusic;
    public AudioClip tombMusic;
    public AudioClip treasureMusic;
    private AudioSource audio;

    void Awake() {
        if (instance == null) {
            instance = this;
            audio = GetComponent<AudioSource>();
        }
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start() {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (audio.clip == dungeonMusic && audio.time >= 262) audio.time = 18f;
	}

    public void PlayMusic(AudioClip music) {
        if (audio.clip != music) {
            audio.clip = music;
            audio.Play();
        }
    }
}
