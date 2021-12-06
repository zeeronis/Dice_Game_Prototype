using System.Collections;
using UnityEngine;

public class UnitSkin : MonoBehaviour
{
    [SerializeField] Renderer modelRenderer;

    public void SetMaterial(Material material)
    {
        modelRenderer.sharedMaterial = material;
    }
}