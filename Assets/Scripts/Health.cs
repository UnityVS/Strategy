using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    int _currentHealthValue;
    [SerializeField] int _maxHealth = 10;
    [SerializeField] int _randomMinHP;
    [SerializeField] int _randomMaxHP;
    [SerializeField] bool _building;
    [SerializeField] Image _healthBar;
    [SerializeField] TextMeshProUGUI _textHealth;
    [SerializeField] TextMeshProUGUI _textHealthShadow;
    [SerializeField] ValueAddEffect _effectAddValuePrefab;
    [SerializeField] int _randomValueRangeMin;
    [SerializeField] int _randomValueRangeMax;
    WildTurn _wildTurn;
    private void Start()
    {
        _wildTurn = FindObjectOfType<WildTurn>();
        if (!_building)
        {
            _maxHealth = Random.Range(_randomMinHP, _randomMaxHP);
        }
        _currentHealthValue = _maxHealth;
        UpdateUI();
    }
    public void ChangeHealthAdd(int value)
    {
        _currentHealthValue += value;
        UpdateUI();
    }
    public void ChangeHealthSubtract(int value)
    {
        _currentHealthValue -= value;
        _healthBar.fillAmount = (float)_currentHealthValue / (float)_maxHealth;
        UpdateUI();
        if (_currentHealthValue < 1)
        {
            if (gameObject.GetComponent<Enemy>())
            {
                Die();
            }
            Destroy(gameObject);
        }
    }
    private void Die()
    {
        int addValue = Random.Range(_randomValueRangeMin, _randomValueRangeMax);
        Resources.Instance.AddResources(FarmResource.Gold, addValue);
        ValueAddEffect effect = Instantiate(_effectAddValuePrefab, transform.position, Quaternion.Euler(60, -11, 0));
        effect.SetValueMining(addValue);
        effect.AddResource(1.5f);
        for (int i = 0; i < _wildTurn._enemysList.Count; i++)
        {
            if (_wildTurn._enemysList[i] == gameObject.GetComponent<Enemy>())
            {
                _wildTurn._enemysList.RemoveAt(i);
                _wildTurn.Win();
                break;
            }
        }
    }
    void UpdateUI()
    {
        _textHealth.text = _currentHealthValue + "/" + _maxHealth;
        _textHealthShadow.text = _currentHealthValue + "/" + _maxHealth;
    }
}
