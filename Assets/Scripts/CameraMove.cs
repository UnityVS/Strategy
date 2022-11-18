using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera _camera;
    Vector3 _startPoint;
    Vector3 _cameraStartPosition;
    Plane _plane;
    float _speed = 0.3f;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);
        if (Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _cameraStartPosition = transform.position;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 offset = point - _startPoint;
            transform.position = _cameraStartPosition + offset * _speed;
        }
    }
}
