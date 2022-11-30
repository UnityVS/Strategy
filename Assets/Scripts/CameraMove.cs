using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform _camera;
    Vector3 _startPoint;
    Vector3 _cameraStartPosition;
    Plane _plane;
    [SerializeField] float _speed = 0.3f;
    [SerializeField] Vector3 _direction = new Vector3(0, 0, 1);
    Coroutine _coroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _coroutine = StartCoroutine(CameraMovement());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(_coroutine);
    }

    IEnumerator CameraMovement()
    {
        while (true)
        {
            _camera.transform.Translate(_direction * _speed * Time.deltaTime);
            float _xPosition = Mathf.Clamp(_camera.transform.position.x, -15f, 15f);
            float _zPosition = Mathf.Clamp(_camera.transform.position.z, -15f, 15f);
            Vector3 _newPosition = new Vector3(_xPosition, _camera.transform.position.y, _zPosition);
            _camera.transform.position = _newPosition;
            yield return null;
        }
    }
}
//private void Start()
//{
//    _camera = FindObjectOfType<Camera>();
//    _plane = new Plane(Vector3.up, Vector3.zero);
//}
//    private void Update()
//    {
//        Ray ray = _camera.ScreenToViewportPoint(Input.mousePosition);
//        Vector3 rayPoint = _camera.ViewportToScreenPoint(Input.mousePosition);

//        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
//        float distance;
//        _plane.Raycast(ray, out distance);
//        Vector3 point = ray.GetPoint(distance);
//        if (Input.GetMouseButtonDown(2))
//        {
//            _startPoint = point;
//            _cameraStartPosition = transform.position;
//        }
//        if (Input.GetMouseButton(2))
//        {
//            Vector3 offset = point - _startPoint;
//            transform.position = _cameraStartPosition - offset * _speed;
//        }
//    }
//}
