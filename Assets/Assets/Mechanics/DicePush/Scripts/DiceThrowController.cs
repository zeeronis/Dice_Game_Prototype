using GlobalEventAggregator;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceThrowController : MonoBehaviour
{
    [SerializeField] ThrowDice dicePrefab;
    [SerializeField] PlayerType playerType;
    [Space]
    [SerializeField] float shootDelay;
    [SerializeField] float angleInDegrees;
    [SerializeField] Transform spawnTransform;
    [SerializeField] Transform shootDirTransform;
    [SerializeField] Transform targetTransform;
    [Space]
    [SerializeField] Image reloadProgressImage;
    [SerializeField] TrajectoryRenderer trajectoryRenderer;

    private bool isRun;
    private bool isMouseDown;
    private bool isReloaded;
    private float gameFieldY;
    private ThrowDice diceObj = null;
    private Coroutine reloadProgress;

    private IPlayerInput playerInput;

    public bool IsReloaded => isReloaded;
    public Vector3 ThrowTargetPos => targetTransform.position;
    public Vector3 StartThrowTargetPos => spawnTransform.position + transform.forward;


    private void Awake()
    {
        playerInput = GetComponent<IPlayerInput>();

        EventAggregator.AddListener<OnDiceGameStarted>(this, e =>
        {
            isRun = true;
            gameFieldY = e.gameFieldY;

            var startTargetPos = spawnTransform.position;
            startTargetPos.y = gameFieldY;
            spawnTransform.position = startTargetPos;

            RunReloadProgress();
        });
        EventAggregator.AddListener<OnDiceGameEnded>(this, e =>
        {
            isRun = false;
            SetTrajectoryActive(false);

            StopAllCoroutines();
        });

        SetReloadProgress(0);
        SetTrajectoryActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventAggregator.RemoveListener<OnDiceGameEnded>(this);
        EventAggregator.RemoveListener<OnDiceGameStarted>(this);
    }

    private void LateUpdate()
    {
        if (!isRun)
            return;

        if (playerInput.GetMouseButtonDown())
        {
            if (isReloaded)
            {
                ResetTargetPos();
                SetTrajectoryActive(true);

                trajectoryRenderer.gameObject.SetActive(true);
            }

            isMouseDown = isReloaded;
        }

        if (playerInput.GetMouseButton() && isMouseDown == true)
        {
            if (playerInput.MousePosDelta != Vector2.zero)
            {
                MoveTargetPos(new Vector3(
                                playerInput.MousePosDelta.x,
                                0,
                                playerInput.MousePosDelta.y));

                UpdateTrajectory();
            }
        }

        if (playerInput.GetMouseButtonUp() && isMouseDown == true)
        {
            isMouseDown = false;

            TryShoot();
            //SetTrajectoryActive(false);
        }
    }

    private void UpdateTrajectory()
    {
        trajectoryRenderer.SetCirclePos(targetTransform.position);
        trajectoryRenderer.ShowTrajectory(spawnTransform.position, GetVelocityToTarget(), gameFieldY);
    }

    private void SetTrajectoryActive(bool isActive)
    {
        trajectoryRenderer.gameObject.SetActive(isActive);

        if (isActive)
        {
            UpdateTrajectory();
        }
    }

    private void SetReloadProgress(float amount)
    {
        reloadProgressImage.fillAmount = amount;
    }

    private IEnumerator DisableTrajectory(float time)
    {
        yield return new WaitForSeconds(time);

        if (isMouseDown == false)
        {
            SetTrajectoryActive(false);
        }
    }

    private IEnumerator ReloadProgressCo()
    {
        isReloaded = false;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / shootDelay;

            SetReloadProgress(t);

            yield return null;
        }

        isReloaded = true;
        diceObj = Instantiate(dicePrefab, spawnTransform.position, Quaternion.identity, transform.parent);
        diceObj.SetFreeze(true);

        reloadProgress = null;
    }

    private void RunReloadProgress()
    {
        if (reloadProgress != null)
            StopCoroutine(reloadProgress);

        reloadProgress = StartCoroutine(ReloadProgressCo());
    }

    private void TryShoot()
    {
        if (!isReloaded || diceObj == null)
            return;


        diceObj.AddRadomTorque();
        diceObj.SetOwner(playerType);
        diceObj.Throw(GetVelocityToTarget(), (pos, num) =>
        {
            if (pos.y > gameFieldY)
            {
                EventAggregator.Invoke(new OnDiceSpawnUnits(num, pos, playerType));
            }
        });

        RunReloadProgress();
        StartCoroutine(DisableTrajectory(trajectoryRenderer.TrajectoryFlyTime));
    }

    private Vector3 GetVelocityToTarget()
    {
        shootDirTransform.localEulerAngles = new Vector3(-angleInDegrees, 0f, 0f);

        Vector3 fromTo = targetTransform.position - spawnTransform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        spawnTransform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
        
        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float AngleInRadians = angleInDegrees * Mathf.PI / 180;

        float v2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        return shootDirTransform.forward * v;
    }

    private void ResetTargetPos()
    {
        targetTransform.position = StartThrowTargetPos;
    }

    private void MoveTargetPos(Vector3 delta)
    {
        var pos = targetTransform.position += delta;
        pos.y = gameFieldY;

        targetTransform.position = pos;
    }
}