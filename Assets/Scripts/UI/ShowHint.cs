using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShowHint : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _hintDuration = 1f;
    [SerializeField] float _fadeDuration = 1f;
    Tween _tween;
    private void Start()
    {
        _text.DOFade(0, 0);
        _image.DOFade(0, 0);
        _image.gameObject.SetActive(false);
    }
    public float CheckHintDuration()
    {
        return _hintDuration;
    }
    public void DisplayHint(string updateText)
    {
        if (_tween != null) return;
        _image.gameObject.SetActive(true);
        _text.text = updateText;
        _tween = _text.DOFade(1, _fadeDuration);
        _image.DOFade(1, _fadeDuration);
        Invoke(nameof(HideHint), _hintDuration + _fadeDuration);
    }
    public void HideHint()
    {
        _text.DOFade(0, _fadeDuration);
        _image.DOFade(0, _fadeDuration);
        Invoke(nameof(DeactivationObject), _fadeDuration);
    }
    void DeactivationObject()
    {
        _tween = null;
        _image.gameObject.SetActive(false);
    }
}
