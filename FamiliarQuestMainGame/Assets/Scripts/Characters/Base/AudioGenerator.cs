using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(CacheGrabber))]
[RequireComponent(typeof(NoiseMaker))]
public class AudioGenerator : MonoBehaviour {

    public void CreateDeathSound()
    {
        if (GetComponent<Monster>() == null) return;
        if (GetComponent<MonsterMortal>().deathSoundCreated) return;
        else GetComponent<MonsterMortal>().deathSoundCreated = true;
        GameObject obj = Instantiate(GetComponent<MonsterMortal>().deathSound, transform.position, transform.rotation);
        if (GetComponent<CacheGrabber>().soundsByName.ContainsKey(GetComponent<MonsterSounds>().onDeath)) {
            obj.GetComponent<AudioSource>().PlayOneShot(GetComponent<CacheGrabber>().soundsByName[GetComponent<MonsterSounds>().onDeath]);
        }
        //NetworkServer.Spawn(obj);
    }

    public void PlaySound(int number) {
        GetComponentInChildren<AudioSource>().PlayOneShot(GetComponent<CacheGrabber>().sounds[number]);
    }

    public void PlaySoundByName(string name) {
        if (GetComponentInChildren<AudioSource>() == null || GetComponent<CacheGrabber>() == null || GetComponent<CacheGrabber>().soundsByName == null) return;
        if (GetComponent<CacheGrabber>().soundsByName.ContainsKey(name)) GetComponentInChildren<AudioSource>().PlayOneShot(GetComponent<CacheGrabber>().soundsByName[name]);
    }
}
