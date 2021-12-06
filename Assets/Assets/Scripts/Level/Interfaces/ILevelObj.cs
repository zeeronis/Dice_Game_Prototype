using UnityEngine;

// Yes, this is not an interface. This is due to the fact that the Unity does not support interfaces in the inspector
public abstract class ILevelObj: ScriptableObject
{
    public abstract ILevelData LevelData { get; }
    public abstract ILevelController LevelPrefab { get; }
}
