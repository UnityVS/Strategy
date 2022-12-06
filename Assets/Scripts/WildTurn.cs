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
        if (_timer < 0)
        {
            Vector3 newPositionOfEnemyes = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            _currentEnemyCount = _listOfEnemyCount[_currentTurn];
            for (int i = 0; i < _currentEnemyCount; i++)
            {
                Instantiate(_enemyPrefab, newPositionOfEnemyes + Vector3.back * Random.Range(-2f, 2f), Quaternion.identity);
            }
            AddTurn();
            UpdateTurnCount();
            if (_currentTurn == _timers.Count) return;
            _maxTimer = _timers[_currentTurn];
            _timer = _maxTimer;
        }
    }
    void UpdateTurnCount()
    {
        _textTurnCount.text = "<color=#adadad>Current turn: </color>" + _currentTurn + "/" + _timers.Count;
    }
    void AddTurn()
    {
        _currentTurn += 1;
    }
    void UpdateTurnTime()
    {
        _textTimeToTurn.text = "<color=#adadad>Next Wild Turn: </color>" + _timer.ToString("0") + " sec";
    }
}
