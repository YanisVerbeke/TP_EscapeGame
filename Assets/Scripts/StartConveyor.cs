using UnityEngine;

public class StartConveyor : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;

    [ContextMenu("Spawn Object")]
    public void SpawnObject()
    {
        Instantiate(_objectToSpawn, transform.position, Quaternion.identity);
    }

}
