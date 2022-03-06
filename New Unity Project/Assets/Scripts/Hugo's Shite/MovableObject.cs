using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableObject : Interactable
{
    private Rigidbody target;
    protected SpringJoint joint;
    private bool isHeld = false;
    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField]
    private float DampenMovement = 0.15f;
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public override void DoInteraction(PlayerController inController)
    {
        base.DoInteraction(inController);

        if(!isHeld)
        {
            target = inController.PlayerHand;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = target;
            joint.spring = 5.0f;
            joint.damper = 10.0f;
            joint.anchor = Vector3.zero;

            isHeld = true;

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    private void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Mouse0) && isHeld)
        {
            ThrowObject();
        }
    }

    private void FixedUpdate()
    {
        if (isHeld /*&& Vector3.Distance(transform.position, target.transform.position) > 0.1f*/)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref currentVelocity, DampenMovement);
        }
    }

    void ThrowObject()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(target.transform.forward * 100f);
        rb.useGravity = true;
        isHeld = false;
        target = null;
        Destroy(joint);

        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }
}
