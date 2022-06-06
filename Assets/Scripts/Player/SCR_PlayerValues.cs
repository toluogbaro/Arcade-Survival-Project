using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SCR_PlayerValues : MonoBehaviour
{
    public static SCR_PlayerValues _instance;
    private Movement.SCR_PlayerController playerController;

    [Header("Sanity")]

    public Volume globalVolume;
    public float currentSanity = 300f;
    public float maxSanity = 300f;
    public float trackedSanity;
    private float sanityReduction;
    private bool damageCalculation;
    public TextMeshProUGUI sanityText;
    public Image sanityFill;
    public GameObject cameraToShake;
    public GameObject player;
    public AnimationCurve animationCurve;
    [HideInInspector] public bool isInvincible;

    //postprocessing effects
    private FilmGrain filmGrain;
    private ChromaticAberration aberration;
    private MotionBlur motionBlur;

    [Header("Stamina")]

    public float currentStamina = 100f;
    public float maxStamina = 100f;
    public Image staminaFill;
    public GameObject staminaBar;
    public float staminaRegenMultiplier;
    private bool staminaCalculation;
    private bool fadeStaminaIn;
    private bool fadeStaminaOut;

    [Header("Karma")]

    public float currentKarma;
    public float maxKarma;
    public float karmaThreshold;

    private void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        playerController = FindObjectOfType<Movement.SCR_PlayerController>();
        globalVolume = GameObject.FindGameObjectWithTag("Global Volume").GetComponent<Volume>();
        trackedSanity = currentSanity;
        FilmGrain currentFilmGrain;
        ChromaticAberration currentAberration;
        MotionBlur currentMotionBlur;

        if (globalVolume.profile.TryGet<FilmGrain>(out currentFilmGrain))
        {
            filmGrain = currentFilmGrain;
        }

        if (globalVolume.profile.TryGet<ChromaticAberration>(out currentAberration))
        {
            aberration = currentAberration;
        }

        if(globalVolume.profile.TryGet<MotionBlur>(out currentMotionBlur))
        {
            motionBlur = currentMotionBlur;
        }
    }


    private void Update()
    {
        ConstantSanityReduction();
        ConstantStaminaRegen();
        DamageCalculation();

        if (Input.GetKeyDown(KeyCode.T))
            TakeSanity(50f);
    }

    public void TakeSanity(float sanityReduction)
    {
        if(!isInvincible)
        {
            trackedSanity -= sanityReduction;
            StartCoroutine(DamageEffect());
            damageCalculation = true;
        }
      
    }

    public void ConstantSanityReduction()
    {
        if (!damageCalculation)
        {
            currentSanity -= Time.deltaTime / 10f;
            sanityFill.fillAmount = currentSanity / maxSanity;
        }
        sanityText.text = Mathf.RoundToInt(currentSanity).ToString();
    }

    public void ConstantStaminaRegen()
    {
        if (!staminaCalculation && currentStamina < maxStamina) currentStamina += (Time.deltaTime * staminaRegenMultiplier);
        staminaFill.fillAmount = currentStamina / maxStamina;

        switch (playerController.currentMovementState)
        {
            case Movement.MovementStates.Walk:
                staminaCalculation = false;
                StopTakeStamina();
                motionBlur.intensity.Override(0f);
                break;

            case Movement.MovementStates.Sprint:
                TakeStamina(10f);
                motionBlur.intensity.Override(5f);
                break;

            case Movement.MovementStates.Dodge:
                TakeStamina(75f);
                motionBlur.intensity.Override(2f);
                break;


        }

    }

    public void TakeStamina(float _staminaMultipler)
    {
        if (!fadeStaminaIn) StartCoroutine(StartStaminaCalulation());
        staminaCalculation = true;

        currentStamina -= (Time.deltaTime * _staminaMultipler);
        
    }

    public void StopTakeStamina()
    {

        if(!fadeStaminaOut && currentStamina >= maxStamina) StartCoroutine(StopStaminaCalculation());
    }

    public IEnumerator StartStaminaCalulation()
    {
        fadeStaminaIn = true;
        fadeStaminaOut = false;

        staminaBar.SetActive(true);

        Image[] imagesToFade = staminaBar.GetComponentsInChildren<Image>();

        for (int i = 0; i < imagesToFade.Length; i++)
            StartCoroutine(UITools._instance.DeltaFadeImage(imagesToFade[i], true, 1.1f));

        yield return null;

    }

    public IEnumerator StopStaminaCalculation()
    {
        fadeStaminaIn = false;
        fadeStaminaOut = true;

        Image[] imagesToFade = staminaBar.GetComponentsInChildren<Image>();

        for (int i = 0; i < imagesToFade.Length; i++)
            StartCoroutine(UITools._instance.DeltaFadeImage(imagesToFade[i], false, 1.1f));

        yield return new WaitForSecondsRealtime(2f);

        staminaBar.SetActive(false);
    }

    public IEnumerator DamageEffect()
    {

        //FilmGrain grain = default;
        //ChromaticAberration aberration = default;

        
       
        //for(int i = 0; i > globalVolume.profile.components.Count; i++)
        //{
        //    if (globalVolume.profile.components[i].name == "FilmGrain")
        //    {
        //        grain = (FilmGrain)globalVolume.profile.components[i];
        //        Debug.Log("found grain");
        //    }

        //    if (globalVolume.profile.components[i].name == "ChromaticAberration")
        //    {
        //        aberration = (ChromaticAberration)globalVolume.profile.components[i];
        //        Debug.Log("found abberation");
        //    }
        //}
        StartCoroutine(CameraShake(0.5f));

        for (float i = 0; i < 1; i += 0.1f)

        {
            //volumeFilmGrain.intensity.Override(i);
            filmGrain.intensity.Override(i);
            aberration.intensity.Override(i);
            yield return null;
        }


        for (float i = 1; i > 0; i -= 0.1f)

        {
            //volumeFilmGrain.intensity.Override(i);
            filmGrain.intensity.Override(i);
            aberration.intensity.Override(i);
            yield return null;
        }



    }

    public void DamageCalculation()
    {
        if (damageCalculation) StartCoroutine(DamageReduction());
    }

    IEnumerator DamageReduction()

    {
        isInvincible = true;

        while (currentSanity > trackedSanity)
        {
            
            currentSanity = Mathf.MoveTowards(currentSanity, trackedSanity, Time.deltaTime * 2f);
            sanityFill.fillAmount = currentSanity / maxSanity;
            yield return null;
        }

        isInvincible = false;
        damageCalculation = false;
    }

    IEnumerator CameraShake(float duration)
    {

        SCR_CameraMovement.cantMoveCamera = true;
        Movement.SCR_PlayerController.cantMoveCharacter = true;

        Vector3 cameraStartPos = cameraToShake.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / duration);
            cameraToShake.transform.position = cameraStartPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        cameraToShake.transform.position = cameraStartPos;
        SCR_CameraMovement.cantMoveCamera = false;
        Movement.SCR_PlayerController.cantMoveCharacter = false;
    }
}
