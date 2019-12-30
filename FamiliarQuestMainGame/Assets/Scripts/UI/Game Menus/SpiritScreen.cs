using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpiritScreen : MonoBehaviour {

    public GameObject content;
    public GameObject spiritUI;
    public PlayerCharacter player;
    public Text spiritText1;
    public Text spiritText2;
    public Text spiritText3;
    public Text spiritDescription1;
    public Text spiritDescription2;
    public Text spiritDescription3;
    public Text affinitySummary;
    private List<GameObject> spiritObjects = new List<GameObject>();
    public SharedInventory sharedInventory = null;

	// Update is called once per frame
    void Update()
    {
        if (sharedInventory == null)
        {
            var obj = GameObject.FindGameObjectWithTag("ConfigObject");
            if (obj != null) sharedInventory = obj.GetComponent<SharedInventory>();
        }
        if (player == null)
        {
            var players = PlayerCharacter.players;
            //foreach (var item in players) if (item.GetComponent<NetworkIdentity>().isLocalPlayer) player = item.GetComponent<PlayerCharacter>();
            foreach (var item in players) if (item.GetComponent<PlayerCharacter>().isMe) player = item.GetComponent<PlayerCharacter>();
        }
    }

    public void Refresh()
    {
        RefreshSpareSpirits();
        RefreshEquippedSpirits();
        RefreshAffinityText();
    }

    private void RefreshSpareSpirits()
    {
        Update();
        foreach (var obj in spiritObjects) Destroy(obj);
        spiritObjects.RemoveRange(0, spiritObjects.Count);
        for (int i = 0; i < sharedInventory.spareSpiritNames.Count; i++) RefreshSpirit(i);
    }

    private void RefreshSpirit(int i)
    {
        var name = sharedInventory.spareSpiritNames[i];
        var description = sharedInventory.spareSpiritDescriptions[i];
        var obj = Instantiate(spiritUI);
        obj.transform.SetParent(content.transform);
        spiritObjects.Add(obj);
        var spiritItemUpdater = obj.GetComponentInChildren<SpiritItem>();
        spiritItemUpdater.name = name;
        spiritItemUpdater.description = description;
        spiritItemUpdater.spiritScreen = this;
        spiritItemUpdater.number = i;
    }

    private void RefreshEquippedSpirits()
    {
        spiritText1.text = player.GetComponent<HotbarUser>().spiritNames[0];
        spiritDescription1.text = player.GetComponent<MenuUser>().spiritDescriptions[0];
        SetSpirit2(player.GetComponent<HotbarUser>().spiritNames.Count >= 2);
        SetSpirit3(player.GetComponent<HotbarUser>().spiritNames.Count >= 3);
    }

    private void SetSpirit2(bool found)
    {
        if (found)
        {
            spiritText2.text = player.GetComponent<HotbarUser>().spiritNames[1];
            spiritDescription2.text = player.GetComponent<MenuUser>().spiritDescriptions[1];
        }
        else
        {
            spiritText2.text = "";
            spiritDescription2.text = "";
        }
    }

    private void SetSpirit3(bool found)
    {
        if (found)
        {
            spiritText3.text = player.GetComponent<HotbarUser>().spiritNames[2];
            spiritDescription3.text = player.GetComponent<MenuUser>().spiritDescriptions[2];
        }
        else
        {
            spiritText3.text = "";
            spiritDescription3.text = "";
        }
    }

    private void RefreshAffinityText()
    {
        affinitySummary.text = player.GetComponent<MenuUser>().spiritAffinityText;
    }

    public void EquipSpirit(int number, int slotNumber)
    {
        player.EquipSpirit(number, slotNumber);
        StartCoroutine(RefreshInABit());
    }

    public IEnumerator RefreshInABit() {
        yield return new WaitForSeconds(0.1f);
        Refresh();
    }
}
