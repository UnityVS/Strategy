using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipUGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _toolTip;
    [SerializeField] float _fadeOutTime = 0.8f;
    [SerializeField] float _fadeInTime = 1.5f;
    [SerializeField] float _opacityText = 0.5f;
    Tween _myTweenUnFade = null;
    Coroutine _coroutine;
    private void Awake()
    {
        _toolTip.DOFade(0f, 0);
        _toolTip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(WaitingShowText());
        //if (_myTweenUnFade != null) return;
        //_toolTip.gameObject.SetActive(true);
        //await ShowText();
    }
    public async void OnPointerExit(PointerEventData eventData)
    {
        await HideText(_fadeOutTime);
    }
    async Task HideText(float fadeOutTime)
    {
        if (_myTweenUnFade != null) { await _myTweenUnFade.AsyncWaitForCompletion(); }
        _myTweenUnFade = _toolTip.DOFade(0f, fadeOutTime);
        Invoke(nameof(NullingFade), fadeOutTime);
    }
    void ShowText()
    {
        //if (_myTweenUnFade != null) { await _myTweenUnFade.AsyncWaitForCompletion(); }
        _myTweenUnFade = _toolTip.DOFade(_opacityText, _fadeInTime);
    }
    IEnumerator WaitingShowText()
    {
        while (true)
        {
            if (_myTweenUnFade == null)
            {
                _toolTip.gameObject.SetActive(true);
                ShowText();
                StopAllCoroutines();
            }
            yield return null;
        }
    }
    void NullingFade()
    {
        if (_myTweenUnFade != null)
        {
            _myTweenUnFade = null;
        }
    }
}
