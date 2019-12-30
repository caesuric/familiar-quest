using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class SimulatedNoiseGenerator : MonoBehaviour
{
    //[Command]
    public void CmdMakeNoise(Vector3 position, float volume)
    {
        foreach (var mob in Monster.monsters)
        {
            var distance = Vector3.Distance(mob.transform.position, transform.position);
            //if (distance <= volume && volume - distance - mob.GetComponent<MonsterSenses>().hearing > 0) mob.GetComponent<MonsterSenses>().HearNoise(position);
        }
    }
}
