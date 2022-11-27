using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    Transform _transform;
    bool _enemy = false;
    float _timer;
    float _maxTimer = 5f;
    Quaternion _rotateAngle;
    Vector3 _rotateVector = Vector3.forward * 5f + Vector3.right * 60f;
    private void Start()
    {
        _rotateAngle = Quaternion.Euler(_rotateVector);
        _timer = _maxTimer;
        _transform = GetComponent<Transform>();
        _transform.rotation = _rotateAngle;
        if (transform.GetComponentInParent<Enemy>()) _enemy = true;
    }
    private void Update()
    {
        if (!_enemy) return;
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            if (_transform.rotation != _rotateAngle)
            {
                RotateUI();
                return;
            }
            _timer = _maxTimer;
        }
    }
    void RotateUI()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, _rotateAngle, 5f * Time.deltaTime);
    }
}
