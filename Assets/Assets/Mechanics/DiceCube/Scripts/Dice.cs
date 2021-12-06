using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected DiceSide[] sides;

    private System.Action<Vector3, int> onStopResultCallback;


    private IEnumerator DiceStopCheckerCo()
    {
        while (rb.velocity.magnitude > .01f || rb.angularVelocity.magnitude > .01f)
        {
            //Debug.Log($"velocity: {rb.velocity.magnitude}");
            //Debug.Log($"angularVelocity: {rb.angularVelocity.magnitude}");

            yield return null;
        }

        onStopResultCallback?.Invoke(transform.position, GetCurrentSideNumber());

        Destroy(gameObject);
    }

    private int GetCurrentSideNumber()
    {
        return sides.OrderByDescending(side => side.transform.position.y).First().Number;
    }

    public void SetFreeze(bool isActive)
    {
        rb.isKinematic = isActive;
    }

    public void Throw(Vector3 velocity, System.Action<Vector3, int> onStopResultCallback)
    {
        this.onStopResultCallback = onStopResultCallback;

        SetFreeze(false);
        rb.velocity = velocity;
        StartCoroutine(DiceStopCheckerCo());
    }
}
