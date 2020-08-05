using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RW.MonumentValley
{
    /**/
    // utiltiy for fading UI Image or Text on/off
    [RequireComponent(typeof(MaskableGraphic))]
    public class ScreenFader : MonoBehaviour
    {
        private MaskableGraphic[] images;

        private void Awake()
        {
            images = GetComponentsInChildren<MaskableGraphic>();
        }

        public void FadeOff(float fadeOffTime = 0.5f)
        {
            foreach (MaskableGraphic image in images)
            {
                image?.CrossFadeAlpha(0f, fadeOffTime, true);
                StartCoroutine(DisableRoutine(image, fadeOffTime));
            }
        }

        IEnumerator DisableRoutine(MaskableGraphic graphic, float disableTime)
        {
            while (graphic.color.a > 0)
            {
                yield return null;
            }
            graphic.gameObject.SetActive(false);

        }

        public void FadeOn(float fadeOnTime = 0.5f)
        {
            foreach (MaskableGraphic image in images)
            {
                image.gameObject.SetActive(true);
                image?.CrossFadeAlpha(0f, .01f, true);
                image?.CrossFadeAlpha(1f, fadeOnTime, true);
            }
        }

    }

}