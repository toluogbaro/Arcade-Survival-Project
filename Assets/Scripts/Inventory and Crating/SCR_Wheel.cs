using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SCR_Wheel : MonoBehaviour, IPointerEnterHandler,  IPointerClickHandler
{
    public SCR_BaseInvItem item;
    public int itemID;
    public GameObject displayImage;
    public Image hoverImage;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            StartCoroutine(UITools._instance.FastImageFade(hoverImage, true, 1.1f, 0.4f));
            SCR_WeaponWheel._instance.AddToCraftList(item);
            SCR_WeaponWheel._instance.potentialItemsToDiscard.Add(displayImage);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       //do nothing
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
