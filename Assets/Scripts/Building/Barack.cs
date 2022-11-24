using TMPro;
using UnityEngine;

public enum Units
{
    Citizen,
    Knight
}

public class Barack : Building
{
    [SerializeField] int _capacity = 2;
    [SerializeField] int _availabelCapacity = 2;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _shadowCapacityText;
    [SerializeField] TextMeshProUGUI _textCapacityUI;
    [SerializeField] TextMeshProUGUI _shadowCapacityTextUI;
    [SerializeField] TextMeshProUGUI _priceUIOriginal;
    [SerializeField] TextMeshProUGUI _priceUIShadow;
    [SerializeField] Unit _unitToCreate;
    //[SerializeField] Units _currentTypeUnit;
    Unit _currentUnit;
    [SerializeField] Transform _spawnPoint;
    override public void Start()
    {
        UpdateUI(0);
        _priceUIOriginal.text = _unitToCreate.CheckPrice().ToString() + "$";
        _priceUIShadow.text = _unitToCreate.CheckPrice().ToString() + "$";
    }
    public void BuildingSetInScene()
    {
        SelectObjectStatus(false);
    }
    void UpdateUI(int valueAdd)
    {
        _availabelCapacity -= valueAdd;
        _textCapacity.text = (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
        _shadowCapacityText.text = (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
        string newText = " Citizen - " + (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
        _textCapacityUI.text = newText;
        _shadowCapacityTextUI.text = newText;
    }
    public void TryBuyUnit(Unit unit)
    {
        int balance = Resources.Instance.CheckBalance();
        int price = unit.CheckPrice();
        if (_availabelCapacity > 0)
        {
            if (balance >= price)
            {
                Unit newUnit = Instantiate(unit, _spawnPoint.position, Quaternion.identity);
                _currentUnit = newUnit;
                Resources.Instance.UpdateResource(balance - price);
                UpdateUI(1);
            }
            else
            {
                GameManager.Instance._showHint.DisplayHint("You can't buy this unit. Need more money");
            }
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("You can't buy this unit. No more slots");
        }

    }
}
