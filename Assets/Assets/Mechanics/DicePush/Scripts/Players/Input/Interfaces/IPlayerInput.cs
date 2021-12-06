using UnityEngine;

public interface IPlayerInput
{
    public Vector2 MousePosDelta { get; }

    public bool GetMouseButtonDown();
    public bool GetMouseButtonUp();
    public bool GetMouseButton();
}
