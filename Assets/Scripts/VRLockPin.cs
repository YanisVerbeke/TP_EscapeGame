using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRLockPin : XRGrabInteractable
{
    [Header("Objet à verrouiller")]
    [SerializeField] private VRRotatingKnob knob;

    public VRRotatingKnob Knob => knob;
}