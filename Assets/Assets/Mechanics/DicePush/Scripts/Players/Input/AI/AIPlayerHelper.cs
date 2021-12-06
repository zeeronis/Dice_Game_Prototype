using UnityEngine;

[System.Serializable]
public class AIPlayerHelper
{
    [SerializeField] float aimTime;
    [SerializeField] float addtionalBoundsOffset;
    [SerializeField] Collider boundsCollider;
    [SerializeField] DiceThrowController throwController;


    public float AimTime => aimTime;
    public bool IsReloaded => throwController.IsReloaded;
    public Vector3 ThrowTargetPosition => throwController.ThrowTargetPos;
    public Vector3 StartThrowTargetPosition => throwController.StartThrowTargetPos;


    public Vector3 GetRandomThrowPos()
    {
        return new Vector3(
            Random.Range(boundsCollider.bounds.min.x - addtionalBoundsOffset, boundsCollider.bounds.max.x + addtionalBoundsOffset),
            boundsCollider.bounds.max.y,
            Random.Range(boundsCollider.bounds.min.z - addtionalBoundsOffset, boundsCollider.bounds.max.z + addtionalBoundsOffset));
    }

}
