using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRRotatingKnob : XRBaseInteractable
{
    [Header("Rotation")]
    [SerializeField] private float targetAngle = 1080f;
    [SerializeField] private bool reverseDirection = false;
    [SerializeField] private bool useControllerTwist = false;
    [SerializeField] private float minHandRadius = 0.02f;

    [Header("Retour automatique (si non verrouillé)")]
    [SerializeField] private bool returnWhenReleased = true;
    [SerializeField] private float returnSpeed = 180f;

    [Header("Indicateur")]
    [SerializeField] private Image progressFill;

    [Header("Événements")]
    [SerializeField] private UnityEvent onLock;
    [SerializeField] private UnityEvent onUnlock;
    [SerializeField] private UnityEvent onTargetReached;

    private float turned;
    private float lastHandAngle;
    private bool locked;
    private bool targetReached;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        GetHandAngle(args.interactorObject, out lastHandAngle);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        if (isSelected && firstInteractorSelecting != null)
        {
            if (GetHandAngle(firstInteractorSelecting, out float handAngle))
            {
                if (!locked)
                {
                    float delta = Mathf.DeltaAngle(lastHandAngle, handAngle);
                    if (reverseDirection) delta = -delta;

                    SetTurned(turned + delta);
                }

                lastHandAngle = handAngle;
            }
        }
        else if (returnWhenReleased && !locked && !targetReached)
        {
            SetTurned(Mathf.MoveTowards(turned, 0f, returnSpeed * Time.deltaTime));
        }
    }

    private bool GetHandAngle(IXRInteractor interactor, out float angle)
    {
        Transform attach = interactor.GetAttachTransform(this);
        Transform space = transform.parent;

        Vector3 v;
        if (useControllerTwist)
        {
            v = attach.up;
            if (space != null) v = space.InverseTransformDirection(v);
        }
        else
        {
            v = space != null ? space.InverseTransformPoint(attach.position) : attach.position;
            v -= transform.localPosition;
        }

        angle = Mathf.Atan2(v.z, v.y) * Mathf.Rad2Deg;
        
        return useControllerTwist || new Vector2(v.y, v.z).magnitude > minHandRadius;
    }

    private void SetTurned(float value)
    {
        turned = Mathf.Clamp(value, 0f, targetAngle);

        float visual = reverseDirection ? -turned : turned;
        transform.localRotation = Quaternion.Euler(visual, 0f, 90f);

        if (progressFill != null)
            progressFill.fillAmount = turned / targetAngle;

        bool reached = turned >= targetAngle;
        if (reached && !targetReached) onTargetReached?.Invoke();
        targetReached = reached;
    }

    public void Lock()
    {
        if (locked) return;
        locked = true;
        Debug.Log("Cog verrouillé à " + turned + "°");
        onLock?.Invoke();
    }

    public void Unlock()
    {
        if (!locked) return;
        locked = false;
        Debug.Log("Cog déverrouillé");
        onUnlock?.Invoke();
    }

    public bool IsLocked() => locked;
}