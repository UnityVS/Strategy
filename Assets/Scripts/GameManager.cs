using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ShowHint _showHint;
    public static GameManager Instance;
    bool _skip = false;
    [SerializeField] GameObject _skipButton;
    float _timer = 0f;
    float _maxTimer = 5f;
    List<string> _tutorialTexts = new List<string>();
    int _currentLineOfTutorialText = 0;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _tutorialTexts.Add("Задача, продержаться несколько волн врагов");
        _tutorialTexts.Add("Уничтожить врагов, появившихся с диких волн");
        _tutorialTexts.Add("Стройте здания для добычи ресурсов");
        _tutorialTexts.Add("Перемещаться на AWSD. Перезагрузка уровня F1");
        _tutorialTexts.Add("Shift+A;W;S;D ускорение перемещения");
        float timeDuration = _showHint.CheckHintDuration() * 3;
        StartCoroutine(StartInformation(timeDuration));
    }
    public void SkipTutorial()
    {
        _skip = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Resources.Instance.AddResources(FarmResource.Gold, 500);
            Resources.Instance.AddResources(FarmResource.Wood, 500);
            Resources.Instance.AddResources(FarmResource.Stone, 500);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void Restart()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    IEnumerator StartInformation(float timeDuration)
    {
        _showHint.DisplayHintTutorial(_tutorialTexts[_currentLineOfTutorialText]);
        _skipButton.SetActive(true);
        _maxTimer = timeDuration;
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > _maxTimer || _skip == true)
            {
                _currentLineOfTutorialText++;
                _showHint.HideHintTutorial();
                yield return new WaitForSeconds(0.5f);
                _timer = 0f;
                _skip = false;
                if (_tutorialTexts.Count != _currentLineOfTutorialText)
                {
                    _showHint.DisplayHintTutorial(_tutorialTexts[_currentLineOfTutorialText]);
                }
                else
                {
                    _skipButton.SetActive(false);
                    break;
                }
            }
            yield return null;
        }
    }
}
