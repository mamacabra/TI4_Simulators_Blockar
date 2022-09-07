using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShader : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineSacaleFactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlineRenderer;
    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineSacaleFactor, outlineColor);
        outlineRenderer.enabled = true;
        
    }
    Renderer CreateOutline(Material outlineMat,float scaleFactor,Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.GetComponent<OutlineShader>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;
        rend.enabled = false;
        return rend;
    }
}