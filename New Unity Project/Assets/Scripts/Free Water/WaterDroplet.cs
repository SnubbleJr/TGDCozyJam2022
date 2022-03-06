using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter()
    {
        this.gameObject.SetActive(false);
        
        if (WaterManager.SpawnWaterDelegate != null)
        {
            Vector3 pos = transform.position;
            WaterManager.SpawnWaterDelegate(pos, WaterManager.STARTING_WATER_AMOUNT);
        }


        Destroy(this.gameObject);
    }
}
