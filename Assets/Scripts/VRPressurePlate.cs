using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRPressurePlate : MonoBehaviour
{
    private static readonly List<VRPressurePlate> allPlates = new List<VRPressurePlate>();

    [Header("Détection")]
    [SerializeField] private string requiredTag = "";
    [SerializeField] private float requiredWeight = 0f;
    [SerializeField] private bool ignoreHeldObjects = true;
    [SerializeField] private bool stayActivated = false;

    [Header("Animation")]
    [SerializeField] private Transform plateVisual;
    [SerializeField] private float pressDistance = 0.02f;
    [SerializeField] private float pressSpeed = 10f;

    [Header("Matériaux")]
    [SerializeField] private Renderer plateRenderer;
    [SerializeField] private Material idleMaterial;
    [SerializeField] private Material activeMaterial;

    [Header("Action")]
    [SerializeField] private CurtainController curtain;
    [SerializeField] private bool requireAllPlatesInGroup = false;
    [SerializeField] private string groupId = "";

    [Header("Événements")]
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private UnityEvent onDeactivated;
    [SerializeField] private UnityEvent onAllActivated;

    private readonly HashSet<Collider> inside = new HashSet<Collider>();
    private readonly HashSet<Object> counted = new HashSet<Object>();
    private Vector3 startPosition;
    private Vector3 pressedPosition;
    private bool isActive;

    private void Awake()
    {
        if (plateVisual != null)
        {
            startPosition = plateVisual.localPosition;
            pressedPosition = startPosition;
            pressedPosition.y -= pressDistance;
        }

        ApplyMaterial();
    }

    private void OnEnable() => allPlates.Add(this);
    private void OnDisable() => allPlates.Remove(this);

    private void OnTriggerEnter(Collider other)
    {
        if (requiredTag != "" && !other.CompareTag(requiredTag))
            return;

        inside.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        inside.Remove(other);
    }

    private void Update()
    {
        bool loaded = IsLoaded();

        if (loaded && !isActive)
            SetPlateActive(true);
        else if (!loaded && isActive && !stayActivated)
            SetPlateActive(false);

        if (plateVisual != null)
        {
            Vector3 target = isActive ? pressedPosition : startPosition;
            plateVisual.localPosition = Vector3.Lerp(plateVisual.localPosition, target, Time.deltaTime * pressSpeed);
        }
    }

    private bool IsLoaded()
    {
        counted.Clear();
        float weight = 0f;

        foreach (Collider c in inside)
        {
            if (c == null || !c.gameObject.activeInHierarchy)
                continue;

            if (ignoreHeldObjects)
            {
                XRGrabInteractable grab = c.GetComponentInParent<XRGrabInteractable>();
                if (grab != null && grab.isSelected)
                    continue;
            }

            Rigidbody rb = c.attachedRigidbody;
            Object key = rb != null ? (Object)rb : c;

            if (counted.Add(key) && rb != null)
                weight += rb.mass;
        }

        return counted.Count > 0 && weight >= requiredWeight;
    }

    private void SetPlateActive(bool state)
    {
        isActive = state;
        ApplyMaterial();

        if (!state)
        {
            onDeactivated?.Invoke();
            return;
        }

        Debug.Log("Plaque activée");
        onActivated?.Invoke();

        if (!requireAllPlatesInGroup)
        {
            TriggerAction();
        }
        else if (AllPlatesInGroupActive())
        {
            Debug.Log("Toutes les plaques du groupe '" + groupId + "' sont activées");
            onAllActivated?.Invoke();
            TriggerAction();
        }
    }

    private bool AllPlatesInGroupActive()
    {
        foreach (VRPressurePlate plate in allPlates)
        {
            if (plate.groupId == groupId && !plate.isActive)
                return false;
        }

        return true;
    }

    private void TriggerAction()
    {
        if (curtain != null)
            curtain.RaiseCurtain();
    }

    private void ApplyMaterial()
    {
        if (plateRenderer == null)
            return;

        Material m = isActive ? activeMaterial : idleMaterial;
        if (m != null)
            plateRenderer.sharedMaterial = m;
    }
}