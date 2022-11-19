using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AddedValue : MonoBehaviour
{

    Tween _showValue = null;
    Vector3 _startPosition;
    [SerializeField] Transform _transform;
    [SerializeField] Text _text;
    [SerializeField] Text _textShadow;
    [SerializeField] float _time = 3f;
    [SerializeField] float _fadeOutTime = 0.5f;
    [SerializeField] float _fadeValue = 0.5f;
    [SerializeField] AnimationCurve _curve;
    private void Awake()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
        _startPosition = _transform.position;
    }
    public void AddResource(float timeResource)
    {
        if (_showValue == null)
        {
            if (timeResource > 1 && timeResource < 2)
            {
                _showValue = _transform.DOMove(_text.transform.position + Vector3.up, 0.95f);
                _text.DOFade(_fadeValue, 0.7f);
                _textShadow.DOFade(_fadeValue, 0.7f);
                Invoke(nameof(FadeMediumFast), 0.7f);
                Invoke(nameof(Nulling), 0.95f + 0.05f);
            }
            else if (timeResource >= 0.6f && timeResource <= 1)
            {
                _showValue = _transform.DOMove(_text.transform.position + Vector3.up, 0.59f);
                _text.DOFade(_fadeValue, 0.4f);
                _textShadow.DOFade(_fadeValue, 0.4f);
                Invoke(nameof(FadeUltraFast), 0.4f);
                Invoke(nameof(Nulling), 0.59f + 0.05f);
            }
            else if (timeResource < 0.6f)
            {
                _showValue = _transform.DOMove(_text.transform.position + Vector3.up, 0.3f);
                _text.DOFade(_fadeValue, 0.15f);
                _textShadow.DOFade(_fadeValue, 0.15f);
                Invoke(nameof(FadeTooFast), 0.15f);
                Invoke(nameof(Nulling), 0.3f + 0.05f);
            }
            else
            {
                _showValue = _transform.DOMove(_text.transform.position + Vector3.up, _time);
                _text.DOFade(_fadeValue, _curve.Evaluate(_time));
                _textShadow.DOFade(_fadeValue, _curve.Evaluate(_time));
                Invoke(nameof(Fade), _time - _fadeOutTime);
                Invoke(nameof(Nulling), _time + 0.1f);
            }

        }
    }
    void Fade()
    {
        _text.DOFade(0f, _curve.Evaluate(_fadeOutTime));
        _textShadow.DOFade(0f, _curve.Evaluate(_fadeOutTime));
    }
    void FadeMediumFast()
    {
        _text.DOFade(0f, 0.25f);
        _textShadow.DOFade(0f, 0.25f);
    }
    void FadeUltraFast()
    {
        _text.DOFade(0f, 0.21f);
        _textShadow.DOFade(0f, 0.21f);
    }
    void FadeTooFast()
    {
        _text.DOFade(0f, 0.15f);
        _textShadow.DOFade(0f, 0.15f);
    }
    void Nulling()
    {
        _transform.position = _startPosition;
        _showValue = null;
    }
}
