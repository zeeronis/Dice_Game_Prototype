
using UnityEngine;

// Yes, this is not an interface. This is due to the fact that the Unity does not support interfaces in the inspector
public abstract class ILevelController: MonoBehaviour
{
    protected abstract void OnLevelEnd(bool isWin);

    public abstract void Init(ILevelData levelData);

    public abstract void StartLevel();
}