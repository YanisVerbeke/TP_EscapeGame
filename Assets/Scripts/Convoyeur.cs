using UnityEngine;

public class Convoyeur : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float vitesse = 1.5f;

    void OnCollisionStay(Collision col)
    {
        Rigidbody rb = col.rigidbody;
        if (rb != null && !rb.isKinematic)
            rb.linearVelocity = direction.normalized * vitesse
                              + new Vector3(0, rb.linearVelocity.y, 0);
    }
}