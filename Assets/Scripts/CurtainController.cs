using UnityEngine;

public class CurtainController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float height = 3f;

    [SerializeField] private float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isRaising;

    private void Start()
    {
        closedPosition = transform.localPosition;

        openPosition = closedPosition;
        openPosition.y += height;
    }

    private void Update()
    {
        if (!isRaising)
            return;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, openPosition, speed * Time.deltaTime);

        if (transform.localPosition == openPosition)
        {
            isRaising = false;
        }
    }

    public void RaiseCurtain()
    {
        Debug.Log("Le rideau monte");

        isRaising = true;
    }
}