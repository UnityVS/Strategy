using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WildTurn : MonoBehaviour
{
    float _timer;
    [SerializeField] float _maxTimer;
    [SerializeField] TextMeshProUGUI _textTurnCount;
    [SerializeField] TextMeshProUGUI _textTimeToTurn;
    [SerializeField] List<float> _timers;
    int _currentTurn = 0;
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] int _currentEnemyCount;
    [SerializeField] List<int> _listOfEnemyCount;
    [SerializeField] Transform _boxGenerationArea;
    [SerializeField] GameObject _win;
    public List<Enemy> _enemysList;
    private void Awake()
    {
        _maxTimer = _timers[_currentTurn];
        _timer = _maxTimer;
        UpdateTurnCount();
    }
    private void Update()
    {
        if (_currentTurn == _timers.Count) return;
        _timer -= Time.deltaTime;
        UpdateTurnTime();
        if (_timer < 0) GenerationEnemy();
    }
    public void Win()
    {
        if (_currentTurn == _timers.Count && _enemysList.Count == 0) _win.SetActive(true);
    }
    void GenerationEnemy()
    {
        Vector3 newPosition = _boxGenerationArea.TransformPoint(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        _currentEnemyCount = _listOfEnemyCount[_currentTurn];
        for (int i = 0; i < _currentEnemyCount; i++)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, newPosition + Vector3.back * Random.Range(-2f, 2f), Quaternion.identity);
            newEnemy.ChangeDistanceToFollow();
            //newEnemy.ChangeStoppingDistance();
            _enemysList.Add(newEnemy);
        }
        AddTurn();
        UpdateTurnCount();
        if (_currentTurn == _timers.Count) return;
        _maxTimer = _timers[_currentTurn];
        _timer = _maxTimer;
    }
    void UpdateTurnCount()
    {
        _textTurnCount.text = "<color=#adadad>Текущая волна: </color>" + _currentTurn + "/" + _timers.Count;
    }
    void AddTurn()
    {
        _currentTurn++;
    }
    void UpdateTurnTime()
    {
        _textTimeToTurn.text = "<color=#adadad>Следующая дикая волна: </color>" + _timer.ToString("0") + " сек";
    }
}
