using UnityEngine;
using UnityEngine.Serialization;

public class CurtainController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float height = 3f;
    [FormerlySerializedAs("speed")]
    [SerializeField] private float raiseSpeed = 2f;
    [SerializeField] private float lowerSpeed = 0.3f;
    [SerializeField] private float knobRaiseSpeed = 0.3f;

    [Header("Salle suivante")]
    [SerializeField] private GameObject[] zonesSalleSuivante;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private float targetAmount;
    private bool followingKnob;
    private bool locked;

    private void Start()
    {
        closedPosition = transform.localPosition;

        openPosition = closedPosition;
        openPosition.y += height;

        ActiverZones(false);
    }

    private void Update()
    {
        if (locked)
            return;

        Vector3 target = Vector3.Lerp(closedPosition, openPosition, targetAmount);

        bool goingUp = target.y > transform.localPosition.y;
        float currentSpeed = goingUp ? (followingKnob ? knobRaiseSpeed : raiseSpeed) : lowerSpeed;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, currentSpeed * Time.deltaTime);
    }

    public void RaiseCurtain()
    {
        Debug.Log("Le rideau monte");
        followingKnob = false;
        targetAmount = 1f;
        ActiverZones(true);
    }

    public void LowerCurtain()
    {
        Debug.Log("Le rideau descend");
        followingKnob = false;
        targetAmount = 0f;
        ActiverZones(false);
    }

    public void SetOpenAmount(float amount)
    {
        followingKnob = true;
        targetAmount = Mathf.Clamp01(amount);
    }

    public void SetLocked(bool value)
    {
        locked = value;
    }

    private void ActiverZones(bool etat)
    {
        foreach (var z in zonesSalleSuivante)
            if (z != null) z.SetActive(etat);
    }
}