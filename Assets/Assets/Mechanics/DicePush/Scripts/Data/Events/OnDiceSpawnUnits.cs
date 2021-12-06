
using UnityEngine;

public struct OnDiceSpawnUnits
{
    public int count;
    public Vector3 pos;
    public PlayerType playerType;

    public OnDiceSpawnUnits(int count, Vector3 pos, PlayerType playerType)
    {
        this.count = count;
        this.pos = pos;
        this.playerType = playerType;
    }
}
