using GlobalEventAggregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerInput : MonoBehaviour, IPlayerInput
{

    [SerializeField] AIPlayerHelper aiHelper;

    private bool isMouseDown;
    private bool isMouseDownOnce;
    private bool isMouseUpOnce;

    private float aimTimeLeft;
    private float aimDistance;
    private Vector3 aimDir;

    private bool gameIsStarted;

    public Vector2 MousePosDelta { get; private set; }


    private void Awake()
    {
        EventAggregator.AddListener<OnDiceGameStarted>(this, e =>
        {
            gameIsStarted = true;
        });
    }

    private void Update()
    {
        if (!gameIsStarted)
            return;

        ResetMouseEvents();

        if (isMouseDown)
        {
            aimTimeLeft -= Time.deltaTime;

            var delta3 = aimDir * aimDistance * Time.deltaTime / aiHelper.AimTime;
            MousePosDelta = new Vector2(delta3.x, delta3.z);

            if (aimTimeLeft <= 0)
            {
                isMouseDown = false;
                isMouseUpOnce = true;
            }
        }
        else if (aiHelper.IsReloaded 
            && isMouseUpOnce == false
            && isMouseDownOnce == false)
        {
            isMouseDown = true;
            isMouseDownOnce = true;
            aimTimeLeft = aiHelper.AimTime;

            var throwTargetPos = aiHelper.GetRandomThrowPos();
            aimDistance = Vector3.Distance(aiHelper.StartThrowTargetPosition, throwTargetPos);
            aimDir = (throwTargetPos - aiHelper.StartThrowTargetPosition).normalized;
        }
        else
        {
            MousePosDelta = Vector2.zero;
        }
    }

    private void ResetMouseEvents()
    {
        if (isMouseDownOnce)
            isMouseDownOnce = false;

        if (isMouseUpOnce)
            isMouseUpOnce = false;
    }


    public bool GetMouseButton()
    {
        return isMouseDown;
    }

    public bool GetMouseButtonDown()
    {
        return isMouseDownOnce;
    }

    public bool GetMouseButtonUp()
    {
        return isMouseUpOnce;
    }
}
