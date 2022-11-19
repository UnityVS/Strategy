using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text _toolTip;
    [SerializeField] Text _toolTipShadow;
    [SerializeField] float _fadeOutTime = 0.8f;
    [SerializeField] float _fadeInTime = 1.5f;
    [SerializeField] float _fadeAmount = 0.5f;
    Tween _myTweenUnFade = null;
    Tween _myTweenFade = null;
    Tween _myTweenDelay = null;
    bool _unFadeReady = false;
    private void Awake()
    {
        _toolTip.DOFade(0f, 0);
        _toolTipShadow.DOFade(0f, 0);
        _toolTip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTip.gameObject.SetActive(true);
        UnFade();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Fade(_fadeOutTime);
    }
    void Fade(float fadeOutTime)
    {
        if (_myTweenFade == null && _unFadeReady)
        {
            _myTweenFade = _toolTip.DOFade(0f, fadeOutTime);
            _toolTipShadow.DOFade(0f, fadeOutTime);
            Invoke(nameof(NullingFade), fadeOutTime);
        }
        else
        {
            if (_myTweenDelay == null)
            {
                _myTweenDelay = _myTweenUnFade;
                Invoke(nameof(FadeDelay), _fadeInTime);
            }
        }
    }
    void UnFade()
    {
        if (_myTweenFade == null && _myTweenUnFade == null)
        {
            _myTweenUnFade = _toolTip.DOFade(_fadeAmount, _fadeInTime);
            _toolTipShadow.DOFade(_fadeAmount, _fadeInTime);
            Invoke(nameof(UnFadeReady), _fadeInTime);
        }
    }
    void UnFadeReady()
    {
        _unFadeReady = true;
    }
    void FadeDelay()
    {
        _myTweenDelay = _toolTip.DOFade(0f, _fadeOutTime);
        _toolTipShadow.DOFade(0f, _fadeOutTime);
        Invoke(nameof(NullingFade), _fadeOutTime);
    }
    void NullingFade()
    {
        _unFadeReady = false;
        _myTweenUnFade = null;
        _myTweenFade = null;
        _myTweenDelay = null;
    }
    void ObjectOff()
    {
        _toolTip.gameObject.SetActive(false);
    }
}
