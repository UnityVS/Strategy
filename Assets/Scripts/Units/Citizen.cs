using System.Collections;
using UnityEngine;

public class Citizen : Unit
{
    [SerializeField] Renderer _renderer;
    [SerializeField] Renderer _hat;
    [SerializeField] Color _baseColor = new Color(0, 0, 0, 0);
    Color _lerpColor = new Color(0, 0, 0, 0);
    [SerializeField] Color _hightLight;
    Coroutine _coroutine;
    bool _isHightLight;
    public override void Start()
    {
        _selectCirle.SetActive(false);
        _hat.material = _renderer.material;
    }
    public override void OnUnhover()
    {
        StopCoroutine(_coroutine);
        _isHightLight = false;
        _coroutine = StartCoroutine(HightLightColor(false));
    }
    public override void OnHover()
    {
        _lerpColor = _baseColor;
        if (!_isHightLight)
        {
            _isHightLight = true;
            _coroutine = StartCoroutine(HightLightColor(true));
        }
    }
    IEnumerator HightLightColor(bool stutus)
    {
        float timer = 0.25f;
        for (float t = 0; t < 1f; t += Time.deltaTime / timer)
        {
            for (int i = 0; i < _renderer.materials.Length; i++)
            {
                if (stutus)
                {
                    _lerpColor = Color.Lerp(_baseColor, _hightLight, t);
                }
                else
                {
                    _lerpColor = Color.Lerp(_hightLight, _baseColor, t);
                }
                _renderer.materials[i].SetColor("_EmissionColor", _lerpColor);
            }
            yield return null;
        }
    }
}
