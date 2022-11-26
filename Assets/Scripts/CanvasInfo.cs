using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    Transform _transform;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _transform.rotation = Quaternion.Euler(Vector3.forward * 10f + Vector3.right * 60f);
    }
}
