using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoi : MonoBehaviour
{
    public float waterLevel;
    
    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        WaterSpawnCheck();
    }

    void WaterSpawnCheck()
    {
        if (waterLevel > WaterManager.MAX_WATER_TILL_SPAWNING_BABIES)
        {
            //get the radius of the puddle
            var puddlesRadius = rend.bounds.extents.x;

            puddlesRadius *= 1.5f;

            int spawnAmount = 8;

            Vector3 center = rend.bounds.center;

            float degreesPerSpawn = 360 / spawnAmount;

            Vector3 startPoint = Vector3.forward * puddlesRadius;

            float curDegs = 0;

            float amountToGiveOut = waterLevel / (2 * spawnAmount);


            for (int i = 0; i < spawnAmount; ++i)
            {
                float randRange = 15f;
                float degsToSpawnAt = Random.Range(curDegs - randRange, curDegs + randRange);

                var spawnPoint = Quaternion.Euler(0, degsToSpawnAt, 0) * startPoint;

                var worldPos = transform.localToWorldMatrix * (center + spawnPoint);

                Debug.DrawLine(transform.localToWorldMatrix * center, worldPos, Color.red, 20);

                //amount to give out is half off the current max we have

                if (WaterManager.SpawnWaterDelegate != null)
                {
                    WaterManager.SpawnWaterDelegate(worldPos, amountToGiveOut);
                }

                curDegs += degreesPerSpawn;
            }

            waterLevel *= 0.5f;
        }
    }
}
