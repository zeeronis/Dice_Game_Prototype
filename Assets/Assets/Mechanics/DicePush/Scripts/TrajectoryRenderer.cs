using System.Collections;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform circleSpriteTransform;
    [Space]
    [SerializeField] float timeStep = 0.1f;

    public float TrajectoryFlyTime { get; private set; }


    /// <summary>
    /// Show trajectory line and return fly time
    /// </summary>
    public float ShowTrajectory(Vector3 origin, Vector3 velocity, float minY)
    {
        float flyTime = 0;
        var points = new Vector3[50];

        lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * timeStep;

            points[i] = origin + velocity * time + Physics.gravity * time * time / 2;
            if (points[i].y < minY)
            {
                flyTime = time - timeStep;
                lineRenderer.positionCount = i;

                break;
            }
        }

        TrajectoryFlyTime = flyTime;
        lineRenderer.SetPositions(points);

        return flyTime;
    }

    public void SetCirclePos(Vector3 pos)
    {
        const float spriteOfsetY = 0.05f;

        circleSpriteTransform.position = pos + Vector3.up * spriteOfsetY;
    }

}