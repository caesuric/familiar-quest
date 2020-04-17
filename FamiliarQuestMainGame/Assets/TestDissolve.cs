using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDissolve : MonoBehaviour
{
    public float number = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        number += Time.deltaTime / 5f;
        GetComponent<Renderer>().material.SetFloat("_DissolveCutoff", number);
    }
}
