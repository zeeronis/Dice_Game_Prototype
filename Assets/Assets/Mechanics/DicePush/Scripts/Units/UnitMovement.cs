using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody rb;

    private bool isStopped; // = true
    private Vector3 moveDirection;


    public void FixedUpdate()
    {
        if (isStopped)
            return;

        Move();
    }

    private void Move()
    {
        var currVelocity = rb.velocity;
        var dirVelocity = moveDirection * moveSpeed;

        var velocity = new Vector3(0, currVelocity.y);
        velocity.x = Mathf.Abs(currVelocity.x) > Mathf.Abs(dirVelocity.x)
            ? currVelocity.x
            : dirVelocity.x;
        velocity.z = Mathf.Abs(currVelocity.z) > Mathf.Abs(dirVelocity.z)
            ? currVelocity.z
            : dirVelocity.z;


        rb.velocity = velocity;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        isStopped = false;
        moveDirection = direction.normalized;
    }

    public void Stop()
    {
        isStopped = true;
        moveDirection = Vector3.zero;
    }
}