using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerCharacter.localPlayer.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
