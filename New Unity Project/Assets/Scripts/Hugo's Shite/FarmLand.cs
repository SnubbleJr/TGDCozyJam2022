using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour
{
    [SerializeField]
    private Transform SnapPoint;
    private bool IsOccupied;
    void Start()
    {
        IsOccupied = false;
    }

    public void TryPlantObject(GameObject inObj)
    {
        if(!IsOccupied)
        {
            inObj.transform.position = SnapPoint.position;
            inObj.transform.localScale = SnapPoint.localScale;

            if(inObj.GetComponent<MovableObject>())
            {
                Destroy(inObj.GetComponent<MovableObject>());
            }
        }
    }
}
