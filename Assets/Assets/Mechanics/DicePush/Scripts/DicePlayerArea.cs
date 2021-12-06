using GlobalEventAggregator;
using System.Collections.Generic;
using UnityEngine;

public class DicePlayerArea : MonoBehaviour
{
    [SerializeField] PlayerType playerType;

    private Dictionary<PlayerType, List<UnitAI>> unitsDirctionary = new Dictionary<PlayerType, List<UnitAI>>();

    public PlayerType Owner => playerType;


    private void Awake()
    {
        EventAggregator.AddListener<OnDiceGameEnded>(this, e => DisableAllUnits());

        foreach (PlayerType key in System.Enum.GetValues(typeof(PlayerType)))
        {
            unitsDirctionary.Add(key, new List<UnitAI>());
        }
    }

    private void OnDestroy()
    {
        EventAggregator.RemoveListener<OnDiceGameEnded>(this);
    }


    private void DisableAllUnits()
    {
        foreach (var unitsList in unitsDirctionary)
        {
            foreach (var item in unitsList.Value)
            {
                if (item != null)
                    item.Disable();
            }
        }
    }


    public List<UnitAI> GetUnits(PlayerType playerType)
    {
        return unitsDirctionary[playerType];
    }

    public void AddUnit(UnitAI unit)
    {
        if (unitsDirctionary[unit.Owner].Contains(unit) == false)
        {
            unitsDirctionary[unit.Owner].Add(unit);
        }
    }

    public void RemoveUnit(UnitAI unit)
    {
        unitsDirctionary[unit.Owner].Remove(unit);
    }
}