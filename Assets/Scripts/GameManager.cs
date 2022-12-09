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
        _showHint.DisplayHint("������, ������������ ��������� ���� ������");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("���������� ������, ����������� � wild turn");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("������� ������ ��� ������ ��������");
        yield return new WaitForSeconds(timeDuration);
        _showHint.DisplayHint("������ �� AWSD. ������������ ������ F1");
    }
}
