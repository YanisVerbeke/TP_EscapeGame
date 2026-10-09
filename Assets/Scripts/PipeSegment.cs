using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipeSegment : MonoBehaviour
{
    private static readonly List<PipeSegment> allPipes = new List<PipeSegment>();

    [Header("Rotation")]
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;
    [SerializeField] private int steps = 4;
    [SerializeField] private float turnSpeed = 180f;

    [Header("Solution")]
    [SerializeField] private int correctStep = 0;
    [SerializeField] private string groupId = "";

    [Header("Action")]
    [SerializeField] private CurtainController curtain;
    [SerializeField] private UnityEvent onSolved;
    [SerializeField] private UnityEvent onUnsolved;

    private Quaternion baseRotation;
    private Quaternion targetRotation;
    private int currentStep;
    private bool aligned;

    private void Awake()
    {
        baseRotation = transform.localRotation;
        targetRotation = baseRotation;
    }

    private void OnEnable() => allPipes.Add(this);
    private void OnDisable() => allPipes.Remove(this);
    
    public void SetValue(float value)
    {
        currentStep = Mathf.RoundToInt(Mathf.Clamp01(value) * (steps - 1));

        float angle = currentStep * 360f / steps;
        targetRotation = baseRotation * Quaternion.AngleAxis(angle, rotationAxis.normalized);
    }

    private void Update()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, turnSpeed * Time.deltaTime);

        bool nowAligned = currentStep == correctStep
            && Quaternion.Angle(transform.localRotation, targetRotation) < 1f;

        if (nowAligned == aligned)
            return;

        aligned = nowAligned;
        
        if (!OthersAligned())
            return;

        if (aligned)
        {
            Debug.Log("Labyrinthe '" + groupId + "' résolu");
            if (curtain != null) curtain.RaiseCurtain();
            onSolved?.Invoke();
        }
        else
        {
            Debug.Log("Labyrinthe '" + groupId + "' défait");
            if (curtain != null) curtain.LowerCurtain();
            onUnsolved?.Invoke();
        }
    }

    private bool OthersAligned()
    {
        foreach (PipeSegment pipe in allPipes)
        {
            if (pipe != this && pipe.groupId == groupId && !pipe.aligned)
                return false;
        }

        return true;
    }
}