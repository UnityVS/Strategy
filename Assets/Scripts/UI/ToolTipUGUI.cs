using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipUGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool _longHover = true;
    [SerializeField] GameObject _objectToActivate;
    IEnumerator _coroutine;
    float _timer = 0.7f;
    [SerializeField] float _maxTime = 0.7f;
    private void Awake()
    {
        _objectToActivate.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_longHover)
        {
            _coroutine = ShowBlockAfterSomeTime();
            StartCoroutine(_coroutine);
            return;
        }
        _objectToActivate.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_longHover)
        {
            StopAllCoroutines();
            _coroutine = null;
            _timer = _maxTime;
        }
        _objectToActivate.SetActive(false);
    }
    IEnumerator ShowBlockAfterSomeTime()
    {
        while (true)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _objectToActivate.SetActive(true);
            }
            yield return null;
        }
    }
}
