using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour
{
    [SerializeField]
    private Transform SnapPoint;
    private bool IsOccupied;
    private float TimeToGrow = 10.0f;
    private float TimeGrowing = 0.0f;
    void Start()
    {
        IsOccupied = false;

        TimeToGrow = Random.Range(7.5f, 20.0f);
        TimeGrowing = 0.0f;
    }

    public bool TryPlantObject(GameObject inObj)
    {
        if(!IsOccupied)
        {
            if(inObj.GetComponent<Veg>())
            {
                Destroy(inObj.GetComponent<Veg>());
                Destroy(inObj.GetComponent<Rigidbody>());
                inObj.transform.SetParent(SnapPoint);
                inObj.transform.localPosition = Vector3.zero;

                inObj.transform.localScale = SnapPoint.localScale;
                IsOccupied = true;

                return true;
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        if(IsOccupied && TimeGrowing <= TimeToGrow)
        {
            TimeGrowing += Time.deltaTime;

            SnapPoint.localScale = Vector3.one * (TimeGrowing / TimeToGrow);
        }
    }


}
