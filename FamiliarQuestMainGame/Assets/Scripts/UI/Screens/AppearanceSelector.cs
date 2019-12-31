using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppearanceSelector : MonoBehaviour {

    public string characterName;

    public void SelectKitten(int furType) {
        var character = SavedCharacter.BrandNewCharacter(characterName, furType);
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/characters")) Directory.CreateDirectory(Application.persistentDataPath + "/characters");
        FileStream file = File.Create(Application.persistentDataPath + "/characters/" + characterName + ".character");
        bf.Serialize(file, character);
        file.Close();
        SceneManager.LoadScene("Character Selection");
    }
}
