using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] Vector2 sensivity = new Vector2(10, 10);


    public Vector2 MousePosDelta { get; private set; }

    private Vector3 lastMousePos;


    private void OnEnable()
    {
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        if (GetMouseButtonDown())
        {
            lastMousePos = Input.mousePosition;
        }

        CalcMousePositionDelta();
    }

    private void CalcMousePositionDelta()
    {
        //normalized screen position
        Vector2 lastPos = new Vector2(
                   lastMousePos.x / Screen.width,
                   lastMousePos.y / Screen.height);

        Vector2 newPos = new Vector2(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height);


        MousePosDelta = Vector3.Scale(newPos - lastPos, sensivity);
        lastMousePos = Input.mousePosition;
    }

    public bool GetMouseButton()
    {
        return Input.GetMouseButton(0);
    }

    public bool GetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetMouseButtonUp()
    {
        return Input.GetMouseButtonUp(0);
    }
}
