using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrespawnPoints : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    [SerializeField] List<Transform> positions = new List<Transform>();

    public PlayerType PlayerType => playerType;
    public List<Transform> Positions => positions;


#if UNITY_EDITOR
    internal void CollectPoints_EDITOR()
    {
        Utils.ObjectsUtils.ClearAndCollectObjects(transform, ref positions, includeSelf: false);

        EditorUtility.SetDirty(this);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(PrespawnPoints))]
public class PrespawnPointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        if (GUILayout.Button("Collect positions"))
        {
            (target as PrespawnPoints).CollectPoints_EDITOR();
        }
    }
}
#endif