using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Highlighter : MonoBehaviour
{
    // reference to MeshRenderer component
    [SerializeField] private MeshRenderer[] meshRenderers;

    // Property Reference from Shader Graph
    [SerializeField] private string highlightProperty = "_Enabled";

    private bool isEnabled;
    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    void Start()
    {
        Activate(true);
        // use non-highlighted material by default
        EnableHighlight(false);
    }

    // toggle glow off using Shader Graph property
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
    }

    private void OnMouseExit()
    {
        if (isEnabled)
            EnableHighlight(false);
    }

    public void Activate(bool state)
    {
        isEnabled = state;
    }

}