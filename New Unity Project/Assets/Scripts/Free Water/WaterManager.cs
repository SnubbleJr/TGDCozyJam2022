using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public static float STARTING_WATER_AMOUNT = 1f;

    public static float MAX_WATER_TILL_SPAWNING_BABIES = 3f;
    
    [SerializeField]
    private GameObject waterDroplet;

    public delegate void SpawnWaterDel(Vector3 position, float startingWaterLevel);
    public static SpawnWaterDel SpawnWaterDelegate;

    // Start is called before the first frame update
    void Start()
    {
        if (waterDroplet == null)
        {
            Debug.LogError("WaterManager: waterrDroplet preab not given");
        }
        
        SpawnWaterDelegate += SpawnWater;                
    }
    private void OnDestroy()
    {
        if(SpawnWaterDelegate != null)
        {
            SpawnWaterDelegate -= SpawnWater;
        }        
    }

    private void SpawnWater(Vector3 position, float startingWaterLevel)
    {
        //Check to see if we've hit other water puddles
        //if we have, instead of spawning neww puddle with split the amount of a new puddle between them

        RaycastHit[] raycasts = CheckForWater(position);

        if (raycasts.Length > 0)
        {
            float amountToDishOutBetweenPuddles = startingWaterLevel / raycasts.Length;

            foreach (var raycast in raycasts)
            {            
                WaterBoi waterBoi = raycast.transform.GetComponent<WaterBoi>();
                if (waterBoi)
                {
                    waterBoi.waterLevel += amountToDishOutBetweenPuddles;
                } 
            }
        }
        else
        {
            var newWaterPuddle = Instantiate(waterDroplet, position, Quaternion.identity);

            WaterBoi waterBoi = newWaterPuddle.GetComponent<WaterBoi>();
            if (waterBoi)
            {
                waterBoi.waterLevel = startingWaterLevel;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static RaycastHit[] CheckForWater(Vector3 position)
    {
        RaycastHit[] raycasts = Physics.SphereCastAll(position, 0.1f, Vector3.forward, LayerMask.GetMask("DiferentWater"));

        return raycasts;
    }
}
