using UnityEngine;

[CreateAssetMenu(fileName = "LevelObj", menuName = "Level/Create Level Obj", order = 1)]
public class LevelObj : ILevelObj
{
    [SerializeField] private LevelController levelPrefab;
    [SerializeField] private LevelData levelData;


    public override ILevelData LevelData => levelData;
    public override ILevelController LevelPrefab => levelPrefab;
}
