using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITools : MonoBehaviour
{
    public static UITools _instance;


    public void Awake()
    {
        _instance = this;
    }

    #region Image Fade

    public IEnumerator UnscaledDeltaFadeImage(Image fadeImage, bool fadeAway, float fadeAmount)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += Time.unscaledDeltaTime)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= Time.unscaledDeltaTime)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }

        //Fade an image in unscaled time - so while the game is paused and unpaused
    }

    public IEnumerator DeltaFadeImage(Image fadeImage, bool fadeAway, float fadeAmount)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += Time.deltaTime)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= Time.deltaTime)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }

        //fade an image in scaled time - only when the game is unpaused
    }

    public IEnumerator FastImageFade(Image fadeImage, bool fadeAway, float fadeAmount, float speed)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += speed)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= speed)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
                yield return null;
            }
        }

        //fade an image in float time - faster than unscaled or scaled
    }

    public IEnumerator ImageGlowOnce(Image fadeImage)

    {

        StartCoroutine(UnscaledDeltaFadeImage(fadeImage, true, 1.1f));

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(UnscaledDeltaFadeImage(fadeImage, false, 1.1f));

        yield return null;

        //fade an image quickly then unfade
    }

    #endregion

    #region Text Fade

    public IEnumerator UnscaledDeltaFadeText(TextMeshProUGUI text, bool fadeAway, float fadeAmount)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += Time.unscaledDeltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= Time.unscaledDeltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
    }

    public IEnumerator ScaledDeltaFadeText(TextMeshProUGUI text, bool fadeAway, float fadeAmount)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += Time.deltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= Time.deltaTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
    }

    public IEnumerator FastTextFade(TextMeshProUGUI text, bool fadeAway, float fadeAmount, float speed)
    {

        if (fadeAway)
        {
            for (float i = -1f; i <= fadeAmount; i += speed)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
        else
        {

            for (float i = fadeAmount; i >= -1f; i -= speed)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                yield return null;
            }
        }
    }

    #endregion
}
