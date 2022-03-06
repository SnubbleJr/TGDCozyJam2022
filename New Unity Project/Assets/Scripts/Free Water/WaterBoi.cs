using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaterBoi : MonoBehaviour
{
    public float waterLevel;

    private WaterBoi[] neibouringWaters;  

    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForNeighbouringWater();
        
        WaterSpawnCheck();
    }

    void CheckForNeighbouringWater()
    {
        int neighbouringSegmetns = 16;
        var points = GeneratePoints(rend.bounds.center, neighbouringSegmetns, false);
        
        foreach (var point in points)
        {
            RaycastHit[] raycasts = WaterManager.CheckForWater(point);

            if (raycasts.Length > 0)
            {
                foreach (var raycast in raycasts)
                {
                    ShareOutWater(raycast.transform.GetComponent<WaterBoi>());
                }
                
                Debug.DrawLine(transform.localToWorldMatrix * rend.bounds.center, point, Color.blue);
            }
            else
            {
                Debug.DrawLine(transform.localToWorldMatrix * rend.bounds.center, point, Color.green);
            }
        }
    }
    
    void WaterSpawnCheck()
    {
        if (waterLevel > WaterManager.MAX_WATER_TILL_SPAWNING_BABIES)
        {
            int spawnAmount = 8;
            
            float amountToGiveOut = waterLevel / (2 * spawnAmount);

            var points = GeneratePoints(rend.bounds.center, spawnAmount, true);

            foreach (var point in points)
            {
                Debug.DrawLine(transform.localToWorldMatrix * rend.bounds.center, point, Color.red, 20);
                if (WaterManager.SpawnWaterDelegate != null)
                {
                    WaterManager.SpawnWaterDelegate(point, amountToGiveOut);
                }
            }
            
            //amount to give out is half off the current max we have
            waterLevel *= 0.5f;
        }
    }
    
    private Vector3[] GeneratePoints(Vector3 center, int noOfRays, bool isRandom = false)
    {
        //get the radius of the puddle
        var puddlesRadius = rend.bounds.extents.x;

        puddlesRadius *= 1.5f;

        float posRandRange = 0.2f;
        puddlesRadius *= (isRandom
            ? Random.Range(puddlesRadius - posRandRange, puddlesRadius + posRandRange)
            : 1f);
        
        float degreesPerSpawn = 360 / noOfRays;

        Vector3 startPoint = Vector3.forward * puddlesRadius;

        float curDegs = 0;

        Vector3[] points = new Vector3[noOfRays];

        for (int i = 0; i < noOfRays; ++i)
        {
            float randRange = 15f;
            float degsToSpawnAt = isRandom ? Random.Range(curDegs - randRange, curDegs + randRange) : curDegs;

            var spawnPoint = Quaternion.Euler(0, degsToSpawnAt, 0) * startPoint;

            var worldPos = transform.localToWorldMatrix * (center + spawnPoint);

            points[i] = worldPos;
            
            curDegs += degreesPerSpawn;
        }

        return points;
    }
    
    void ShareOutWater(WaterBoi waterBoi)
    {
        if (waterBoi && waterBoi != this)
        {
            //share out the water between the puddles
            //whoever is smaller gets 1/2 o the delta between them

            float sharedWaterValue = waterBoi.waterLevel + this.waterLevel;
            float waterToGiveExchange = sharedWaterValue / 4f;

            if (waterBoi.waterLevel > this.waterLevel)
            {
                waterBoi.waterLevel -= waterToGiveExchange;
                this.waterLevel += waterToGiveExchange;
            }

            if (this.waterLevel > waterBoi.waterLevel)
            {
                this.waterLevel -= waterToGiveExchange;
                waterBoi.waterLevel += waterToGiveExchange;
            }
        }
    }
}
