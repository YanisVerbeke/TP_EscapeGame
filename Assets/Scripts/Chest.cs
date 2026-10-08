using UnityEngine;

public class Chest : MonoBehaviour
{
    private Transform _topTransform;

    private void Awake()
    {
        _topTransform = transform.Find("Chest_Lid");
    }

    public void OpenChest()
    {
        _topTransform.Rotate(-90, 0, 0);
    }
}
