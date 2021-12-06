using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UnitSkin))]
[RequireComponent(typeof(UnitMovement))]
public class UnitAI : MonoBehaviour
{
    public enum UnitState
    {
        WaitTasks,
        Move,
        MoveToEnemy,
        Stopped,
    }

    [SerializeField] PlayerType owner;
    [SerializeField] UnitSkin skin;
    [SerializeField] UnitMovement movement;
    [SerializeField] SphereCollider modelCollider;


    private bool isDisabled;
    private UnitState state;
    private Transform target;
    private DicePlayerArea currentArea;
    private Dictionary<UnitState, System.Action> statesDictionary;

    public PlayerType Owner => owner;
    public float BodyRadius => modelCollider.radius;


    private void Awake()
    {
        SetBehaviorState(UnitState.Stopped);

        statesDictionary = new Dictionary<UnitState, System.Action>()
        {
            [UnitState.Move] = Move,
            [UnitState.WaitTasks] = WaitTasks,
            [UnitState.MoveToEnemy] = MoveToEnemy,
        };
    }

    private void Update()
    {
        if (statesDictionary.TryGetValue(state, out System.Action stateAction))
        {
            stateAction?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentArea == null && other.TryGetComponent(out DicePlayerArea playerZone))
        {
            currentArea = playerZone;
            currentArea.AddUnit(this);

            SetBehaviorState(UnitState.WaitTasks);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out UnitAI unit))
        {
            if (unit.Owner == Owner)
                return;

            Destroy();
            unit.Destroy();
        }

        if (collision.gameObject.TryGetComponent(out ThrowDice dice))
        {
            if (dice.Owner != Owner)
            {
                Destroy();
            }
        }
    }

    private void WaitTasks()
    {
        bool isEnemyArea = currentArea.Owner != Owner;

        if (!isEnemyArea)
        {
            SetBehaviorState(UnitState.Move);
        }
        else
        {
            SetBehaviorState(UnitState.MoveToEnemy);
        }
    }

    private void Move()
    {
        movement.SetMoveDirection(owner == PlayerType.Player1 
            ? Vector3.forward 
            : Vector3.back);
    }

    private void MoveToEnemy()
    {
        if (target == null)
        {
            movement.Stop();
            target = GetClosestEnemy();
            
            if (target == null)
            {
                SetBehaviorState(UnitState.WaitTasks);
                return;
            }
        }

        movement.SetMoveDirection(target.position - transform.position);
    }

    private Transform GetClosestEnemy()
    {
        PlayerType enemyPlayer = Owner == PlayerType.Player1 
            ? PlayerType.Player2 
            : PlayerType.Player1;

        var enemiesList = currentArea.GetUnits(enemyPlayer);
        if (enemiesList.Count > 0)
        {
            var myPos2 = new Vector2(transform.position.x, transform.position.z);
            return enemiesList.OrderBy(e =>
            {
                return Vector2.Distance(myPos2, new Vector2(e.transform.position.x, e.transform.position.z));
            }).First().transform;
        }

        return null;
    }

    private void SetBehaviorState(UnitState state)
    {
        if (isDisabled)
            return;

        if (state == UnitState.WaitTasks || state == UnitState.Stopped)
        {
            movement.Stop();
        }

        this.state = state;
    }


    public void Disable()
    {
        SetBehaviorState(UnitState.Stopped);
        isDisabled = true;
    }

    public void Enable()
    {
        isDisabled = false;
        SetBehaviorState(UnitState.WaitTasks);
    }

    public void Init(PlayerType playerType, Material skinMat)
    {
        this.owner = playerType;
        skin.SetMaterial(skinMat);
    }

    public void Destroy()
    {
        if (gameObject != null)
        {
            currentArea.RemoveUnit(this);
            Destroy(gameObject);
        }
    }
}