using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FinConvoyeur : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        var grab = rb.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected) return;

        Destroy(rb.gameObject);
    }
}