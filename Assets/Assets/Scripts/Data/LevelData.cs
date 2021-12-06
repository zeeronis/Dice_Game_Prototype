
using UnityEngine;

[System.Serializable]
public class LevelData: ILevelData
{
    [System.Serializable]
    public struct DicePlayerData
    {
        public Material unitMat;
    }


    [SerializeField] DicePlayerData[] players;

    public DicePlayerData[] Players => players;
}
