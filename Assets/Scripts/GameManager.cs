using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ShowHint _showHint;
    public static GameManager Instance;
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
        float timeDuration = _showHint.CheckHintDuration() * 2;
        StartCoroutine(StartInformation(timeDuration));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Restart();
        }
    }
    public void Restart()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    IEnumerator StartInformation(float timeDuration)
    {
        _showHint.DisplayHint("Задача, продержаться несколько волн врагов");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("Уничтожить врагов, появившихся с wild turn");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("Стройте здания для добычи ресурсов");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("Летать на AWSD. Перезагрузка уровня F1");
    }
}
