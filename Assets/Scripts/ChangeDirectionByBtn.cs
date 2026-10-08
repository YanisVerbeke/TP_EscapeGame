using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionByBtn : MonoBehaviour
{
    [SerializeField] private List<Vector3> _directionsList;
    private int _index = 0;

    [ContextMenu("Change direction")]
    public void ChangeDirection()
    {
        _index++;
        if (_index >= _directionsList.Count)
        {
            _index = 0;
        }
        transform.rotation = Quaternion.Euler(_directionsList[_index]);
    }
}
