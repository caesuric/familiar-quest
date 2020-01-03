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
    private AudioSource audioSource;

    void Awake() {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (audioSource.clip == dungeonMusic && audioSource.time >= 262) audioSource.time = 18f;
    }

    public void PlayMusic(AudioClip music) {
        if (audioSource.clip != music) {
            audioSource.clip = music;
            audioSource.Play();
        }
    }
}
