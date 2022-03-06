using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string output;
    public virtual void DoInteraction(PlayerController inController)
    {
        Debug.Log(output);
    }

    protected void Update()
    {
        
    }
}
