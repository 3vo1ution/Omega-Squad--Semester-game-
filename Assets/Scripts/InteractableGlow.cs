using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableGlow : MonoBehaviour
{
    private Material objectMaterial;
    private Color originalEmissionColor;

    public Color glowColor = Color.cyan;
    public float glowIntensity = 2f;

    private void Start()
    {
        // Access the parent’s Renderer material
        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            objectMaterial = parentRenderer.material;
            originalEmissionColor = objectMaterial.GetColor("_EmissionColor");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetGlow(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetGlow(false);
        }
    }

    private void SetGlow(bool isGlowing)
    {
        if (objectMaterial != null)
        {
            if (isGlowing)
            {
                objectMaterial.EnableKeyword("_EMISSION");
                objectMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);
            }
            else
            {
                objectMaterial.SetColor("_EmissionColor", originalEmissionColor);
            }
        }
    }
}
