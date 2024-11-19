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

    public Volume globalVolume; // post processing effects
    public float currentSanity = 300f;
    public float maxSanity = 300f;
    public float trackedSanity; 
    private bool damageCalculation; //is damage being calculated?
    public TextMeshProUGUI sanityText;
    public Image sanityFill; //sanity bar
    public GameObject cameraToShake;
    public GameObject player;
    public AnimationCurve animationCurve;
    [HideInInspector] public bool isInvincible; //is the player invincible?

    //postprocessing effects
    private FilmGrain filmGrain;
    private ChromaticAberration aberration;
    private MotionBlur motionBlur;

    [Header("Stamina")]

    public float currentStamina = 100f;
    public float maxStamina = 100f;
    public Image staminaFill;
    public GameObject staminaBar;
    public float staminaRegenMultiplier; // multiplied by time to increase stamina regen speed
    private bool staminaCalculation; //is stamina being calculated?
    private bool fadeStaminaIn;
    private bool fadeStaminaOut;

    [Header("Karma")]

    public float currentKarma;
    public float maxKarma;
    public float karmaThreshold; //the threshold for changing the player over to "the dark side"
    public static bool isDarkSide;

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
            //if the player isnt invincible, like during a dodge or after taking damage, then reduce sanity based on the
            //passed in number
        }
      
    }

    public void ConstantSanityReduction()
    {
        if (!damageCalculation)
        {
            currentSanity -= Time.deltaTime / 10f;
            sanityFill.fillAmount = currentSanity / maxSanity;
            //if damage isnt being calculated, reduce sanity steadily;
        }
        sanityText.text = Mathf.RoundToInt(currentSanity).ToString();
        //sanity text = current sanity number rounded to the nearest whole number
    }

    public void ConstantStaminaRegen()
    {
        if (!staminaCalculation && currentStamina < maxStamina) currentStamina += (Time.deltaTime * staminaRegenMultiplier);
        //regen stamina constantly if its less than max

        staminaFill.fillAmount = currentStamina / maxStamina;
        //diving the current stamina by max stamina gives the necessary step to convert whatever number current
        //stamina is to in between 0 and 1 which is how image fill amounts work

        switch (playerController.currentMovementState)
        {
            //controls stamina based on current movement state. This is efficient because only one state
            //can exist at once

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

        //takes stamina and fades in the bar if it isnt faded in
        
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

        //fades the stamina bar in
        //the use of booleans is just so it doesnt constantly fade in and out in Update()

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

        //fades the stamina bar out
    }

    public IEnumerator DamageEffect()
    {


        StartCoroutine(CameraShake(0.5f)); //shake camera

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


        //post processing effects
    }

    public void DamageCalculation()
    {
        if (damageCalculation) StartCoroutine(DamageReduction());
    }

    IEnumerator DamageReduction()

    {
        isInvincible = true; //player is invincible after being hit

        while (currentSanity > trackedSanity)
        {
            //Apply mesh render fade in and out coroutine
            currentSanity = Mathf.MoveTowards(currentSanity, trackedSanity, Time.deltaTime * 2f);
            sanityFill.fillAmount = currentSanity / maxSanity; // bar fill moves down
            yield return null;

            //tracked sanity is a float that keeps track of current sanity and damage is taken away from the
            //current sanity variable when it changes
        }

        isInvincible = false;
        damageCalculation = false;
    }

    IEnumerator CameraShake(float duration)
    {

        SCR_CameraMovement.cantMoveCamera = true;
        Movement.SCR_PlayerController.cantMoveCharacter = true;

        //stops the player and camera from moving while its shaking to avoid the player/camera relationship from
        //being changed

        Vector3 cameraStartPos = cameraToShake.transform.position; //stores original cam position
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / duration);
            cameraToShake.transform.position = cameraStartPos + Random.insideUnitSphere * strength;

            //camera shake strength is calculated by an animation curve, reduces over time
            yield return null;
        }

        cameraToShake.transform.position = cameraStartPos;
        SCR_CameraMovement.cantMoveCamera = false;
        Movement.SCR_PlayerController.cantMoveCharacter = false;

        //camera and player can move again
    }
}
