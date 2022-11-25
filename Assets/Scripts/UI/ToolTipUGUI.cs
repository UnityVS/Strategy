using DG.Tweening;
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
    Tween _myTweenFade = null;
    Tween _myTweenDelay = null;
    private void Awake()
    {
        _toolTip.DOFade(0f, 0);
        _toolTip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTip.gameObject.SetActive(true);
        ShowText();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideText(_fadeOutTime);
    }
    void HideText(float fadeOutTime)
    {
        _myTweenFade = _toolTip.DOFade(0f, fadeOutTime);
        Invoke(nameof(NullingFade), fadeOutTime);
    }
    void ShowText()
    {
        if (_myTweenFade == null && _myTweenUnFade == null)
        {
            _myTweenUnFade = _toolTip.DOFade(_opacityText, _fadeInTime);
        }
    }
    void FadeDelay()
    {
        _myTweenDelay = _toolTip.DOFade(0f, _fadeInTime);
        Invoke(nameof(NullingFade), _fadeInTime);
    }
    void NullingFade()
    {
        _myTweenUnFade = null;
        _myTweenFade = null;
        _myTweenDelay = null;
    }
}
