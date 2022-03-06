using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

//This things just spans a droplet of water hen you click

[RequireComponent(typeof(Camera))]
public class CameraWaterSpawner : MonoBehaviour
{
    private Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();

        if (camera == null)
        {
            Debug.LogError("CameraWaterSpawner: Camera not found on object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (WaterManager.SpawnWaterDelegate != null)
                {
                    WaterManager.SpawnWaterDelegate(raycastHit.point, WaterManager.STARTING_WATER_AMOUNT);
                }
            }
        }
    }
}
