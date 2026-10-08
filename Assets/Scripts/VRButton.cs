using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRButton : XRBaseInteractable
{
    [Header("Action")]
    [SerializeField] private CurtainController curtain;

    [Header("Animation du bouton")]
    [SerializeField] private Transform buttonVisual;

    [SerializeField] private float pressDistance = 0.02f;

    [SerializeField] private float pressSpeed = 10f;

    private Vector3 startPosition;
    private Vector3 pressedPosition;

    private bool isPressed;

    protected override void Awake()
    {
        base.Awake();

        if (buttonVisual != null)
        {
            startPosition = buttonVisual.localPosition;

            pressedPosition = startPosition;
            pressedPosition.y -= pressDistance;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        selectEntered.AddListener(OnButtonPressed);
        selectExited.AddListener(OnButtonReleased);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(OnButtonPressed);
        selectExited.RemoveListener(OnButtonReleased);

        base.OnDisable();
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (isPressed)
            return;

        isPressed = true;

        Debug.Log("Bouton pressé");
        
        if (curtain != null)
        {
            curtain.RaiseCurtain();
        }
        else
        {
            Debug.LogWarning("CurtainController non assigné au bouton");
        }
    }

    private void OnButtonReleased(SelectExitEventArgs args)
    {
        isPressed = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (buttonVisual == null)
            return;

        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        Vector3 targetPosition = isSelected
            ? pressedPosition
            : startPosition;

        buttonVisual.localPosition = Vector3.Lerp(buttonVisual.localPosition, targetPosition, Time.deltaTime * pressSpeed);
    }
}