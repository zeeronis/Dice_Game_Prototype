using GlobalEventAggregator;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] List<ILevelObj> levels = default;

    private int levelIndex = 0;
    private ILevelController currLevel;
    private DataManager dataManager;

    private ILevelObj CurLevelData => levels[levelIndex];


    private void Awake()
    {
        EventAggregator.AddListener<OnGameDataLoaded>(this, e =>
        {
            dataManager = DataManager.Instance;

            Init(e.playerData);
            RespawnCurrentLevel();
        });

        EventAggregator.AddListener<OnLevelEnd>(this, e =>
        {
            if (e.isWin)
            {
                dataManager.playerData.isCompleteCurrLevel = true;
                dataManager.Save();
            }
        });

        EventAggregator.AddListener<OnRestartLevel>(this, e =>
        {
            RespawnCurrentLevel();
        });

        EventAggregator.AddListener<OnNextLevel>(this, e =>
        {
            SpawnNextLevel();
        });

        EventAggregator.AddListener<OnLevelStart>(this, e =>
        {
            if (currLevel != null)
            {
                currLevel.StartLevel();
            }
        });
    }


    private void Init(PlayerData playerData)
    {
        if (playerData.isCompleteCurrLevel)
        {
            SetNextLevelIndex();
        }
        else
        {
            UpdateLevelIndex();
        }
    }

    private void SetNextLevelIndex()
    {
        dataManager.playerData.levelIndex++;
        dataManager.playerData.isCompleteCurrLevel = false;

        UpdateLevelIndex();
    }

    private void UpdateLevelIndex()
    {
        levelIndex = dataManager.playerData.levelIndex % levels.Count;
    }


    private void TryDestroyCurrLevelObject()
    {
        if (currLevel != null)
            Destroy(currLevel.gameObject);
    }

    private void SetStartupLevelData()
    {
        currLevel.Init(CurLevelData.LevelData);
    }

    private void SpawnLevelObject()
    {
        TryDestroyCurrLevelObject();

        if (levels.Count > 0)
        {
            if (levels[levelIndex] != null)
            {
                currLevel = Instantiate(levels[levelIndex].LevelPrefab);
                SetStartupLevelData();
            }
            else
            {
                Debug.LogError($"LevelsManager: levels[{levelIndex}] is null");
            }
        }
        else
        {
            Debug.LogError("LevelsManager: levels.Count is 0");
        }
    }

    private void RespawnCurrentLevel()
    {
        SpawnLevelObject();
    }

    private void SpawnNextLevel()
    {
        SetNextLevelIndex();
        SpawnLevelObject();
    }

    private void SpawnLevelByIndex(int index)
    {
        levelIndex = index % levels.Count;
        SpawnLevelObject();
    }
}
