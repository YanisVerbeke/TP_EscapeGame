using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Ça n'a AUCUN sens je déteste mais c'est ce qui marche le mieux ??? aled
        Vector3 initPos = _rigidbody.position;
        _rigidbody.position += 2f * Time.fixedDeltaTime * -transform.forward;
        _rigidbody.MovePosition(initPos);
    }
}
