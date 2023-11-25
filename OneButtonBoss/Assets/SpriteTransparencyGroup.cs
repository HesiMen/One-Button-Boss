using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpriteTransparencyGroup : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float alpha;
    [SerializeField] private bool run;

    void Update()
    {
        if(run)
            GrabAllSpriteRenderers();
    }
    private void GrabAllSpriteRenderers()
    {
        var spriteRenderCollection = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in spriteRenderCollection)
        {
            renderer.color = new Vector4 (renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }
    }
}
