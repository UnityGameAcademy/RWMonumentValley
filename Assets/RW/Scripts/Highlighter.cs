using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Highlighter : MonoBehaviour
{
    // reference to MeshRenderer component
    [SerializeField] private MeshRenderer[] meshRenderers;

    // Property Reference from Shader Graph
    [SerializeField] private string highlightProperty = "_IsHighlighted";

    private bool isEnabled;
    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }


    private void Start()
    {
        isEnabled = true;
        // use non-highlighted material by default
        EnableHighlight(false);
    }

    // toggle glow on or off using Shader Graph property
    public void EnableHighlight(bool onOff)
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetFloat(highlightProperty, onOff ? 1f : 0f);
        }
    }
    
    private void OnMouseOver()
    {
        if (isEnabled)
            EnableHighlight(true);
        else
            EnableHighlight(false);
    }

    private void OnMouseExit()
    {
            EnableHighlight(false);
    }

    // master toggle (off overrides highlight state)
    public void Activate(bool state)
    {
        isEnabled = state;
    }

}