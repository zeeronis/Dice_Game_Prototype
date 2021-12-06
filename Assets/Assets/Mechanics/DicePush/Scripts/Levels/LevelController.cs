using UnityEngine;
using GlobalEventAggregator;
using System.Collections.Generic;

public class LevelController : ILevelController
{
    [SerializeField] UnitAI unitPrefab;
    [Space]
    [SerializeField] Collider gameField;
    [SerializeField] SpawnPointsBase spawnPoints;
    [SerializeField] PrespawnPoints[] prespawnPoints;


    private bool isRun;
    private LevelData levelData;
    private List<UnitAI> disabledUnits = new List<UnitAI>();



    private void OnDestroy()
    {
        UnsubscriveFromEvents();
    }

    private void SubscribeToEvents()
    {
        EventAggregator.AddListener<OnDiceSpawnUnits>(this, SpawnUnits);
        EventAggregator.AddListener<OnDiceGameEnded>(this, e => OnLevelEnd(e.isWin));
    }

    private void UnsubscriveFromEvents()
    {
        EventAggregator.RemoveListener<OnDiceSpawnUnits>(this);
        EventAggregator.RemoveListener<OnDiceGameEnded>(this);
    }

    private void SpawnStartUnits()
    {
        foreach (var prespawn in prespawnPoints)
        {
            var unitsMat = GetUnitMat(prespawn.PlayerType);

            foreach (var item in prespawn.Positions)
            {
                var unit = SpawnUnit(item.position, prespawn.PlayerType, unitsMat);
                unit.Disable();

                disabledUnits.Add(unit);
            }
        }
    }

    private void EnableDisabledUnits()
    {
        foreach (var item in disabledUnits)
        {
            if (item != null)
            {
                item.Enable();
            }
        }

        disabledUnits.Clear();
    }

    private void SpawnUnits(OnDiceSpawnUnits data)
    {
        if (!isRun)
            return;

        var unitsMat = GetUnitMat(data.playerType);
        var points = spawnPoints.GetSpawnPoints(data.pos, data.count, unitPrefab.BodyRadius);

        for (int i = 0; i < data.count; i++)
        {
            SpawnUnit(points[i], data.playerType, unitsMat);
        }
    }

    private Material GetUnitMat(PlayerType owner)
    {
        if (levelData.Players.Length > (int)owner)
        {
            return levelData.Players[(int)owner].unitMat;
        }

        return null;
    }

    private UnitAI SpawnUnit(Vector3 pos, PlayerType owner, Material unitsMat)
    {
        var unit = Instantiate(unitPrefab, pos, Quaternion.identity, transform);

        unit.Init(owner, unitsMat);

        return unit;
    }

    protected override void OnLevelEnd(bool isWin)
    {
        UIManager.Instance.ActivScreen(isWin ? Screens.Win : Screens.Loose);

        // todo
        isRun = false;
        EventAggregator.Invoke(new OnLevelEnd(isWin));
    }

    public override void Init(ILevelData data)
    {
        UIManager.Instance.ActivScreen(Screens.Start);

        // Init startup data
        levelData = (data as LevelData);
        if (levelData == null)
        {
            Debug.LogError($"LevelData for Dice game is null; {DataManager.Instance.playerData.levelIndex}");
        }

        SubscribeToEvents();
        SpawnStartUnits();
    }

    public override void StartLevel()
    {
        UIManager.Instance.ActivScreen(Screens.Game);
        EventAggregator.Invoke(new OnDiceGameStarted()
        {
            gameFieldY = gameField.bounds.max.y
        });

        // Run level
        isRun = true;
        EnableDisabledUnits();
    }
}
