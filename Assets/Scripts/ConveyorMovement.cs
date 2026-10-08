using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    private Rigidbody _movableRigidbody;

    private void Awake()
    {
        _movableRigidbody = transform.Find("movablePart").GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Ça n'a AUCUN sens je déteste mais c'est ce qui marche le mieux ??? aled
        Vector3 initPos = _movableRigidbody.position;
        _movableRigidbody.position += 2f * Time.fixedDeltaTime * -transform.forward;
        _movableRigidbody.MovePosition(initPos);
    }
}