using UnityEngine;
using UnityEngine.Events;

public class EndConveyor : MonoBehaviour
{
    [SerializeField] private string _correctObjectTag;
    [SerializeField] private UnityEvent _onCorrectObjectCollected;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == _correctObjectTag)
        {
            _onCorrectObjectCollected.Invoke();
            Destroy(other.gameObject);
        }
    }
}
