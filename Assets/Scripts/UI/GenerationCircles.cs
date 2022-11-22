using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationCircles : MonoBehaviour
{
    [SerializeField] Image _fish;
    [SerializeField] Image _fishSplash;
    [SerializeField] float _animationTime = 0.5f;
    Tween _tween;
    void Start()
    {
        _fish.DOFade(0, 0f);
        _fish.transform.DOScale(0, 0f);
        _fishSplash.DOFade(0, 0f);
        _fishSplash.transform.DOScale(0, 0f);
        Invoke(nameof(FirstStart), 1f);
    }
    void FirstStart()
    {
        StartCoroutine(StartGeneration());
    }
    IEnumerator StartGeneration()
    {
        while (true)
        {
            Vector2 positionFish = _fish.transform.localPosition = new Vector2(Random.Range(-1000, 1000), Random.Range(-500, 500));
            _tween = _fish.DOFade(0.7f, _animationTime);
            _fish.transform.DOScale(1, _animationTime);
            Invoke(nameof(CloseTweenFish), _animationTime * 1.5f);
            yield return new WaitForSeconds(0.4f);
            _fish.transform.DOShakeRotation(0.5f, 45f, 7);
            yield return new WaitForSeconds(0.5f);
            _fishSplash.transform.localPosition = positionFish;
            _fishSplash.DOFade(0.7f, _animationTime * 0.8f);
            _fishSplash.transform.DOScale(0.7f, _animationTime * 0.7f);
            Invoke(nameof(CloseTweenFishSplash), _animationTime * 1.2f);
            yield return new WaitForSeconds(1.8f);
        }
    }
    void CloseTweenFish()
    {
        _fish.DOFade(0, _animationTime / 2);
        _fish.transform.DOScale(0, _animationTime / 2);
    }
    void CloseTweenFishSplash()
    {
        _fishSplash.DOFade(0, _animationTime / 3);
        _fishSplash.transform.DOScale(0, _animationTime / 3);
        _tween = null;
    }
}
