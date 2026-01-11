#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SpriteToQuadConverter
{
    [MenuItem("Tools/Convert Selected Sprites To URP Lit Quads")]
    static void ConvertSelected()
    {
        var selected = Selection.gameObjects;
        int converted = 0;

        // Find URP Lit shader
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit == null)
        {
            Debug.LogError("URP/Lit shader not found. Are you sure URP is installed and active?");
            return;
        }

        foreach (var go in selected)
        {
            var sr = go.GetComponent<SpriteRenderer>();
            if (!sr || !sr.sprite) continue;

            // Create quad
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = go.name + "_Quad";
            Undo.RegisterCreatedObjectUndo(quad, "Create Quad");

            // Match transform (world)
            quad.transform.position = go.transform.position;
            quad.transform.rotation = go.transform.rotation;
            quad.transform.localScale = go.transform.lossyScale;

            // Match sprite size in world units
            var sprite = sr.sprite;
            float ppu = sprite.pixelsPerUnit;
            var rect = sprite.rect;
            float widthUnits = rect.width / ppu;
            float heightUnits = rect.height / ppu;

            quad.transform.localScale = new Vector3(
                quad.transform.localScale.x * widthUnits,
                quad.transform.localScale.y * heightUnits,
                quad.transform.localScale.z
            );

            // Create URP Lit material
            var mat = new Material(urpLit);
            mat.SetTexture("_BaseMap", sprite.texture);
            mat.SetColor("_BaseColor", Color.white);

            // Make it visible + safe defaults
            mat.SetFloat("_Surface", 0); // 0 = Opaque, 1 = Transparent
            mat.SetFloat("_Cull", 0);    // 0 = Both, 1 = Front, 2 = Back

            var mr = quad.GetComponent<MeshRenderer>();
            mr.sharedMaterial = mat;

            // Parent to original
            quad.transform.SetParent(go.transform.parent, true);

            // Disable original sprite renderer (reversible)
            Undo.RecordObject(sr, "Disable SpriteRenderer");
            sr.enabled = false;

            converted++;
        }

        Debug.Log($"Converted {converted} sprites to URP Lit quads.");
    }
}
#endif
