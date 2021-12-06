using GlobalEventAggregator;
using UnityEngine;

public class PushLine : MonoBehaviour
{
    [SerializeField] float yMaxTheshold = 0.05f;

    private float yStart;
    private Transform myTransform;


    private void Awake()
    {
        myTransform = transform;
        yStart = myTransform.position.y;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(myTransform.position.y - yStart) > yMaxTheshold)
        {
            this.enabled = false;
            EventAggregator.Invoke(new OnDiceGameEnded(isWin: myTransform.position.z > 0));
        }
    }
}