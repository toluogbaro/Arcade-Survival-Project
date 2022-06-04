using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_DayProgression : MonoBehaviour
{

    public static int mode;
    private float dayTimer = 150f;
    private float nightTimer = 150f;
    private bool hasNightTimeStarted;
    public TextMeshProUGUI stopWatchText, dayNightText;
    public Image dayNightIcon;
    public Sprite sunSprite, moonSprite;
    public Light directionalLight;
    public Image playerMarker;
    public Camera miniMapCam;
    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<Movement.SCR_PlayerController>().gameObject;
    }
    public void Update()
    {
        CountDown();

    }

    public void CountDown()
    {
        if(dayTimer > 0f && !hasNightTimeStarted)
        {
            mode = 0;
            dayTimer -= Time.deltaTime;
            double dayTimerDouble = System.Math.Round(dayTimer, 2);
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(dayTimerDouble);
            stopWatchText.text = new System.DateTime(timeSpan.Ticks).ToString("mm:ss");
            directionalLight.colorTemperature = dayTimer * 46f;
        }

        if (Input.GetKey(KeyCode.G))
            dayTimer -= 1;

        if (Input.GetKey(KeyCode.N))
            nightTimer -= 1;

        if (dayTimer <= 0f && !hasNightTimeStarted) StartCoroutine(TransitionToNighttime());

        if (hasNightTimeStarted && nightTimer > 0)
        {
            mode = 1;
            nightTimer -= Time.deltaTime;
            double nightTimerDouble = System.Math.Round(nightTimer, 2);
            System.TimeSpan nightTimeSpan = System.TimeSpan.FromSeconds(nightTimerDouble);
            stopWatchText.text = new System.DateTime(nightTimeSpan.Ticks).ToString("mm:ss");
            //if (dayTimer < 150f) dayTimer += Time.deltaTime;
            //directionalLight.colorTemperature = dayTimer * 46f;
        }

        if (nightTimer <= 0f && hasNightTimeStarted) StartCoroutine(TransitionToDayTime());

    }

    public IEnumerator TransitionToNighttime()

    {
        yield return new WaitForSecondsRealtime(0.001f);

        hasNightTimeStarted = true;

        dayTimer = 150f;

        for (float i = directionalLight.intensity; i > 0f; i -= 1000f)

        {
            directionalLight.intensity = i;

            yield return null;
        }

        StartCoroutine(UITools._instance.DeltaFadeImage(dayNightIcon, false, 1.1f));

        StartCoroutine(UITools._instance.ScaledDeltaFadeText(dayNightText, false, 1.1f));

        yield return new WaitForSeconds(1.5f);

        dayNightIcon.sprite = moonSprite;

        dayNightText.text = "Night";

        yield return new WaitForSeconds(1f);

        StartCoroutine(UITools._instance.DeltaFadeImage(dayNightIcon, true, 1.1f));

        StartCoroutine(UITools._instance.ScaledDeltaFadeText(dayNightText, true, 1.1f));


        yield return null;
    }

    public IEnumerator TransitionToDayTime()
    {
        yield return new WaitForSecondsRealtime(0.001f);

        hasNightTimeStarted = false;

        nightTimer = 150f;

        for (float i = directionalLight.intensity; i < 0f; i += 500f)

        {
            directionalLight.intensity = i;

            yield return null;
        }

        StartCoroutine(UITools._instance.DeltaFadeImage(dayNightIcon, false, 1.1f));

        StartCoroutine(UITools._instance.ScaledDeltaFadeText(dayNightText, false, 1.1f));

        yield return new WaitForSeconds(1.5f);

        dayNightIcon.sprite = sunSprite;

        dayNightText.text = "Day";

        yield return new WaitForSeconds(1f);

        StartCoroutine(UITools._instance.DeltaFadeImage(dayNightIcon, true, 1.1f));

        StartCoroutine(UITools._instance.ScaledDeltaFadeText(dayNightText, true, 1.1f));


        yield return null;
    }




}
