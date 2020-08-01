using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    private MaskableGraphic image;

    private void Awake()
    {
        image = GetComponent<MaskableGraphic>();
    }

    public void FadeOff(float fadeOffTime = 0.5f)
    {
        image?.CrossFadeAlpha(0f, fadeOffTime, true);

    }

    public void FadeOn(float fadeOnTime = 0.5f)
    {
        image?.CrossFadeAlpha(1f, fadeOnTime, true);
    }

}
