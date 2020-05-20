using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Valve.VR;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color enterColor = Color.white;
    [SerializeField] private Color downColor = Color.white;
    [SerializeField] private UnityEvent OnClick = new UnityEvent();

    private MeshRenderer meshRenderer = null;

    /// <summary>
    /// Источник событий - геймпад.
    /// </summary>
    public SteamVR_Input_Sources TargetSource = SteamVR_Input_Sources.RightHand;

    /// <summary>
    /// Действие на геймпаде, которое будем отслеживать.
    /// </summary>
    public SteamVR_Action_Boolean ClickAction = null;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }


    void Start()
    {
        ClickAction.AddOnStateDownListener(TriggerDown, TargetSource);
        //ClickAction.AddOnStateUpListener(TriggerUp, TargetSource);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        meshRenderer.material.color = enterColor;
        print("Enter");

        if (ClickAction.state)
            print("QWE");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        meshRenderer.material.color = normalColor;
        print("Exit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        meshRenderer.material.color = downColor;
        print("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        meshRenderer.material.color = enterColor;
        print("Up");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
        print("Click");
    }

    public void Qwe()
    {

    }
}
