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

    public Vector2 minScale = new Vector2(0.5f, 1f);
    public Vector2 maxScale = new Vector2(2f, 4f);
    public float waterLevelCutOff = 10;

    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisual();

        CheckForNeighbouringWater();
        
        WaterSpawnCheck();
    }

    void UpdateVisual()
    {
        float scale = waterLevel / waterLevelCutOff;
        if (scale > 1f)
        {
            scale = 1;
        }

        Vector2 scaleScale = ((maxScale - minScale) * scale) * minScale;

        this.transform.localScale = new Vector3(scaleScale.x, scaleScale.y, scaleScale.x);
    }

    void CheckForNeighbouringWater()
    {
        int neighbouringSegmetns = 16;
        var points = GeneratePoints(transform.position, neighbouringSegmetns, false, 1.2f);
        
        foreach (var point in points)
        {
            RaycastHit[] raycasts = WaterManager.CheckForWater(point);

            if (raycasts.Length > 0)
            {
                foreach (var raycast in raycasts)
                {
                    ShareOutWater(raycast.transform.GetComponentInParent<WaterBoi>());
                }
                
                Debug.DrawLine( transform.position, point, Color.blue);
            }
            else
            {
                Debug.DrawLine( transform.position, point, Color.green);
            }
        }
    }
    
    void WaterSpawnCheck()
    {
        if (waterLevel > WaterManager.MAX_WATER_TILL_SPAWNING_BABIES)
        {
            int spawnAmount = 8;
            
            float amountToGiveOut = waterLevel / (2 * spawnAmount);

            var points = GeneratePoints(transform.position, spawnAmount, true, 1.3f);

            foreach (var point in points)
            {
                Debug.DrawLine(transform.position, point, Color.red, 5);
                if (WaterManager.SpawnWaterDelegate != null)
                {
                    WaterManager.SpawnWaterDelegate(point, amountToGiveOut);
                }
            }
            
            //amount to give out is half off the current max we have
            waterLevel *= 0.5f;
        }
    }
    
    private Vector3[] GeneratePoints(Vector3 center, int noOfRays, bool isRandom = false, float factor = 1.0f)
    {
        //get the radius of the puddle
        var puddlesRadius = rend.bounds.extents.x;

        puddlesRadius *= factor;

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

            var worldPos = center + spawnPoint;

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
