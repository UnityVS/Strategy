using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    int _currentHealthValue;
    [SerializeField] int _maxHealth = 10;
    [SerializeField] Image _healthBar;
    [SerializeField] TextMeshProUGUI _textHealth;
    [SerializeField] TextMeshProUGUI _textHealthShadow;
    private void Start()
    {
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
            Destroy(gameObject);
        }
    }
    void UpdateUI()
    {
        _textHealth.text = _currentHealthValue + "/" + _maxHealth;
        _textHealthShadow.text = _currentHealthValue + "/" + _maxHealth;
    }
}
