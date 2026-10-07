using UnityEngine;

public class SpawnConvoyeur : MonoBehaviour
{
    public GameObject prefab;
    public float intervalle = 2f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, intervalle);
    }

    void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}