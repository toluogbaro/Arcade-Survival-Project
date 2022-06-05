using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_ObjectShake : MonoBehaviour
{
    [SerializeField] Vector3 shakePos;
    [SerializeField] private bool isText = false;
    public Color changeColour, originalColour;

    private void Start()
    {

    }

    public void ShakeObject()
    {
        HorizontalShakeOne();
    }

    public void HorizontalShakeOne()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0, 0, shakePos.z), 0.01f).setOnComplete(HorizontalShakeTwo).setIgnoreTimeScale(true);
        if (isText)
            gameObject.GetComponent<TextMeshProUGUI>().color = changeColour;
    }

    public void HorizontalShakeTwo()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0, 0, -shakePos.z), 0.05f).setOnComplete(OriginalPosition).setIgnoreTimeScale(true);
        if (isText)
            gameObject.GetComponent<TextMeshProUGUI>().color = changeColour;
    }

    public void OriginalPosition()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0, 0, 0), 0.1f).setIgnoreTimeScale(true);
        if (isText)
            gameObject.GetComponent<TextMeshProUGUI>().color = originalColour;
    }

}
