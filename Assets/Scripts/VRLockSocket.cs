using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRLockSocket : XRSocketInteractor
{
    [Header("Réglages")]
    [SerializeField] private VRRotatingKnob knob;

    protected override void OnEnable()
    {
        base.OnEnable();

        selectEntered.AddListener(OnPinInserted);
        selectExited.AddListener(OnPinRemoved);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(OnPinInserted);
        selectExited.RemoveListener(OnPinRemoved);

        base.OnDisable();
    }

    private void OnPinInserted(SelectEnterEventArgs args)
    {
        VRLockPin pin = args.interactableObject.transform.GetComponent<VRLockPin>();

        if (pin == null)
        {
            Debug.LogWarning("Objet inséré mais ce n'est pas une VRLockPin.");
            return;
        }

        Debug.Log("Goupille insérée");

        if (knob != null)
        {
            knob.Lock();
        }
    }

    private void OnPinRemoved(SelectExitEventArgs args)
    {
        Debug.Log("Goupille retirée");

        if (knob != null)
        {
            knob.Unlock();
        }
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if (!base.CanHover(interactable))
            return false;

        return interactable.transform.GetComponent<VRLockPin>() != null;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (!base.CanSelect(interactable))
            return false;

        return interactable.transform.GetComponent<VRLockPin>() != null;
    }
}