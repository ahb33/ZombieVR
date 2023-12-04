using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class TwoHandGrab : XRGrabInteractable
{
    

    
    public XRSimpleInteractable SecondHandSocket;
    XRBaseInteractor secondHand;



    // Start is called before the first frame update
    void Start()
    {
        SecondHandSocket.selectEntered.AddListener(OnSecondHandGrab);
        SecondHandSocket.selectExited.AddListener(OnSecondHandRelease);

    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = isSelected && !interactor.Equals(isSelected);

        return base.IsSelectableBy(interactor);    
    }

    public void OnSecondHandGrab(SelectEnterEventArgs interactor)
    { 
        secondHand = interactor.interactorObject.transform.gameObject.GetComponent<XRBaseInteractor>();
        //secondHand.attachTransform.GetChild(0).transform.parent = SecondHandSocket.transform;
        Debug.Log(secondHand.attachTransform.childCount);

    }

    public void OnSecondHandRelease(SelectExitEventArgs interactor)
    {
        //secondHand.attachTransform.GetChild(0).transform.parent = SecondHandSocket.transform;
        secondHand = null;
        Debug.Log("OnSecondHandRelease");
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondHand && isSelected)                                    
        {
            selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondHand.attachTransform.position - selectingInteractor.attachTransform.position);
        }

        base.ProcessInteractable(updatePhase);
    }

}
