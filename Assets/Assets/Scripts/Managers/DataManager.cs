using GlobalEventAggregator;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : SingletonMonoBehavior<DataManager>
{
    [SerializeField] PlayerData startupPlayerData;

    private BinaryFormatter m_formatter = new BinaryFormatter();

    [HideInInspector] public PlayerData playerData;


    protected override void Awake()
    {
        base.Awake();

        StartupLoadData();
    }

    private void Start()
    {
        EventAggregator.Invoke(new OnGameDataLoaded(playerData));
    }


    private void StartupLoadData()
    {
        const string IS_FIRST_GAME = "IsFirstGame";

        if (PlayerPrefs.GetInt(IS_FIRST_GAME, 1) == 1)
        {
            playerData = startupPlayerData;
            Save();

            PlayerPrefs.SetInt(IS_FIRST_GAME, 0);
        }
        else
        {
            Load();
        }
    }


    public void Save()
    {
        using (FileStream stream = new FileStream(Application.persistentDataPath + "/state.s", FileMode.Create))
        {
            m_formatter.Serialize(stream, playerData);
        }
    }

    public void Load()
    {
        PlayerData? data = null;

        try
        {
            using (FileStream stream = new FileStream(Application.persistentDataPath + "/state.s", FileMode.Open))
            {
                if (stream.Length != 0)
                    data = (PlayerData)m_formatter.Deserialize(stream);

                if (data == null)
                    data = startupPlayerData;
            }
        }
        catch (System.Exception)
        {
            data = startupPlayerData;
        }

        playerData = (PlayerData)data;
    }
}