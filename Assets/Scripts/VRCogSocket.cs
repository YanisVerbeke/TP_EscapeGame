using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRCogSocket : XRSocketInteractor
{
    [Header("Rouage")]
    [SerializeField] private GameObject placedCog;      // rouage tournable avec VRRotatingKnob, désactivé au départ
    [SerializeField] private string requiredTag = "";   // tag du rouage portable
    [SerializeField] private UnityEvent onCogPlaced;

    private GameObject insertedCog;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(OnCogInserted);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(OnCogInserted);
        base.OnDisable();
    }

    private void OnCogInserted(SelectEnterEventArgs args)
    {
        insertedCog = args.interactableObject.transform.gameObject;
        
        Invoke(nameof(SwapCog), 0f);
    }

    private void SwapCog()
    {
        if (insertedCog == null)
            return;

        Debug.Log("Rouage posé dans le socket");

        insertedCog.SetActive(false);
        socketActive = false;

        if (placedCog != null)
            placedCog.SetActive(true);

        onCogPlaced?.Invoke();
    }

    private bool IsCog(Transform t)
    {
        return requiredTag == "" || t.CompareTag(requiredTag);
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsCog(interactable.transform);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsCog(interactable.transform);
    }
}