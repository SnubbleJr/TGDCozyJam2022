using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veg : MovableObject
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<FarmLand>())
        {
            if(collision.gameObject.GetComponent<FarmLand>().TryPlantObject(gameObject))
            {
                if(joint != null)
                {
                    Destroy(joint);
                }
            }
        }
    }
}
