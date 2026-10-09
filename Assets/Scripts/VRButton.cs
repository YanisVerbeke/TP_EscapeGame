using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRButton : XRBaseInteractable
{
    [Header("Action")]
    [SerializeField] private CurtainController curtain;
    [SerializeField] private bool toggle = true;

    [Header("Animation du bouton")]
    [SerializeField] private Transform buttonVisual;
    [SerializeField] private float pressDistance = 0.02f;
    [SerializeField] private float pressSpeed = 10f;

    [Header("Objet lancé")]
    [SerializeField] private bool activateOnThrownObject = true;
    [SerializeField] private string requiredTag = "";
    [SerializeField] private float minImpactSpeed = 1f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float thrownPressDuration = 0.2f;

    private Vector3 startPosition;
    private Vector3 pressedPosition;
    private bool isPressed;
    private bool curtainOpen;
    private float lastTriggerTime = -999f;
    private float visualPressedUntil;

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
        TriggerAction();
    }

    private void OnButtonReleased(SelectExitEventArgs args)
    {
        isPressed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < minImpactSpeed)
            return;

        OnThrownObjectHit(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnThrownObjectHit(other);
    }

    private void OnThrownObjectHit(Collider other)
    {
        if (!activateOnThrownObject)
            return;

        if (Time.time - lastTriggerTime < cooldown)
            return;

        if (requiredTag != "" && !other.CompareTag(requiredTag))
            return;

        XRGrabInteractable grab = other.GetComponentInParent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
            return;

        lastTriggerTime = Time.time;
        visualPressedUntil = Time.time + thrownPressDuration;

        Debug.Log("Bouton activé par un objet lancé");
        TriggerAction();
    }

    private void TriggerAction()
    {
        if (curtain == null)
        {
            Debug.LogWarning("CurtainController non assigné au bouton");
            return;
        }

        if (toggle && curtainOpen)
        {
            curtain.LowerCurtain();
            curtainOpen = false;
        }
        else
        {
            curtain.RaiseCurtain();
            curtainOpen = true;
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (buttonVisual == null)
            return;

        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        bool visuallyPressed = isSelected || Time.time < visualPressedUntil;
        Vector3 targetPosition = visuallyPressed ? pressedPosition : startPosition;

        buttonVisual.localPosition = Vector3.Lerp(buttonVisual.localPosition, targetPosition, Time.deltaTime * pressSpeed);
    }
}