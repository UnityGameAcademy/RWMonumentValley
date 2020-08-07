/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RW.MonumentValley
{
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