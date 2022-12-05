using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipUGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _toolTip;
    [SerializeField] float _opacityText = 0.5f;
    [SerializeField] bool _gameObject = true;
    [SerializeField] bool _longHover = true;
    [SerializeField] GameObject _objectToActivate;
    IEnumerator _coroutine;
    float _timer = 0.7f;
    float _maxTime = 0.7f;
    private void Awake()
    {
        if (_gameObject)
        {
            _objectToActivate.SetActive(false);
            return;
        }
        _toolTip.DOFade(0f, 0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_gameObject)
        {
            if (_longHover)
            {
                _coroutine = ShowBlockAfterSomeTime();
                StartCoroutine(_coroutine);
                return;
            }
            _objectToActivate.SetActive(true);
            return;
        }
        _toolTip.DOFade(_opacityText, 0);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_gameObject)
        {
            _coroutine = null;
            _timer = _maxTime;
            _objectToActivate.SetActive(false);
            return;
        }
        _toolTip.DOFade(0f, 0);
    }
    IEnumerator ShowBlockAfterSomeTime()
    {
        while (true)
        {
            if (_objectToActivate.activeSelf)
            {
                break;
            }
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _objectToActivate.SetActive(true);
                break;
            }
            yield return null;
        }
    }
}
