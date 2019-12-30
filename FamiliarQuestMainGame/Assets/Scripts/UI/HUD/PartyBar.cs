using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBar : MonoBehaviour {

    public PartyStatusTracker status = null;
    private Dictionary<uint, GameObject> bars = new Dictionary<uint, GameObject>();
    public GameObject partyBarPrefab;
	
	// Update is called once per frame
	void Update () {
        if (status == null) Initialize();
        else
        {
            if (status.id.Count - 1 != bars.Count) ClearBars();
            for (int i = 0; i < status.id.Count; i++)
            {
                if (status.localPlayer==null) return;
                //if (status.id[i]!=status.localPlayer.netId.Value)
                //{
                    if (!bars.ContainsKey(status.id[i])) bars.Add(status.id[i], CreateNewBar());
                    if (status.id.Count>i && status.hp.Count>i && status.isTargeted.Count>i) UpdateBar(bars[status.id[i]], status.hp[i], status.isTargeted[i], status.names[i]);
                //}
            }
        }
	}

    private void Initialize()
    {
        var obj = GameObject.FindGameObjectWithTag("ConfigObject");
        if (obj != null) status = obj.GetComponent<PartyStatusTracker>();
    }

    private GameObject CreateNewBar()
    {
        var obj = Instantiate(partyBarPrefab);
        obj.transform.SetParent(transform);
        return obj;            
    }

    private void ClearBars()
    {
        foreach (var kvp in bars) Destroy(kvp.Value);
        bars.Clear();
    }

    private void UpdateBar(GameObject obj, float hp, bool isTargeted, string name)
    {
        obj.GetComponentInChildren<PartyHealthUpdater>().hp = hp;
        obj.GetComponentInChildren<PartyHealthUpdater>().targeted = isTargeted;
        obj.GetComponentInChildren<PartyHealthUpdater>().characterName = name;
    }
}
