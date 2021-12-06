using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class SpawnPoints : SpawnPointsBase
{
    [System.Serializable]
    public struct SpawnPoint
    {
        public Vector3 position;
        public float distanceToCenter;
    }

    [SerializeField] List<SpawnPoint> points = new List<SpawnPoint>();


    // Possible to add spawn on obstacles if using raycast
    // An upgrade is required for different units radiuses. tip: Calculating Relative Offsets
    public override Vector3[] GetSpawnPoints(Vector3 spawnCenter, int count, float unitRadius = 0.5f)
    {
        int unitMask = LayerMask.GetMask("Unit", "Obstacle");
        Vector3[] resultPoints = new Vector3[count];

        int currIndex = 0;
        foreach (var item in points)
        {
            Vector3 worldPos = spawnCenter + item.position;
            if (Physics.OverlapSphere(worldPos, unitRadius, unitMask).Count() == 0)
            {
                resultPoints[currIndex] = worldPos;
                currIndex++;

                if (currIndex >= count)
                    break;
            }
        }

        return resultPoints;
    }

#if UNITY_EDITOR
    internal void CollectPoints_EDITOR()
    {
        points.Clear();

        List<Transform> list = new List<Transform>();
        Utils.ObjectsUtils.ClearAndCollectObjects(transform, ref list, includeSelf: false);

        var sortedList = list.OrderBy(e => Vector3.Distance(e.position, Vector3.zero));
        foreach (var item in sortedList)
        {
            points.Add(new SpawnPoint()
            {
                position = item.position,
                distanceToCenter = Vector3.Distance(item.position, Vector3.zero)
            });
        }

        EditorUtility.SetDirty(this);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnPoints))]
public class SpawnPointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        if (GUILayout.Button("Recalculate positions"))
        {
            (target as SpawnPoints).CollectPoints_EDITOR();
        }
    }
}
#endif
