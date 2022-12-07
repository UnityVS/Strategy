using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Units
{
    Citizen,
    Knight
}

public class Barack : PlayerBuildings
{
    [SerializeField] string _nameOfBarack;
    [SerializeField] string _nameOfUnits;
    [SerializeField] TextMeshProUGUI _textNameOfBarack;
    [SerializeField] TextMeshProUGUI _textNameOfBarackShadow;
    [SerializeField] int _capacity = 2;
    [SerializeField] int _availabelCapacity = 2;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _shadowCapacityText;
    [SerializeField] TextMeshProUGUI _textCapacityUI;
    [SerializeField] TextMeshProUGUI _shadowCapacityTextUI;
    [SerializeField] TextMeshProUGUI _priceUIOriginal;
    [SerializeField] TextMeshProUGUI _priceUIShadow;
    [SerializeField] Unit _unitToCreate;
    Unit _currentUnit;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] TextMeshProUGUI _upgradeStonePrice;
    [SerializeField] TextMeshProUGUI _upgradeCount;
    [SerializeField] List<int> _updatePrice;
    [SerializeField] int _currentUpdate = 0;
    override public void Start()
    {
        UpdateUI(0);
        _textNameOfBarack.text = _nameOfBarack;
        _textNameOfBarackShadow.text = _nameOfBarack;
        _priceUIOriginal.text = _unitToCreate.CheckPrice().ToString() + "$";
        _priceUIShadow.text = _unitToCreate.CheckPrice().ToString() + "$";
        UpdateUpgradeUI();
        base.Start();
    }
    public void BuildingSetInScene()
    {
        SelectObjectStatus(false);
    }
    public override void OnHover()
    {
        //base.OnHover();
    }
    void UpdateUI(int valueAdd)
    {
        _availabelCapacity -= valueAdd;
        RegualUpdateUI();
    }
    void RegualUpdateUI()
    {
        _textCapacity.text = (_capacity - _availabelCapacity) + "/" + _capacity;
        _shadowCapacityText.text = (_capacity - _availabelCapacity) + "/" + _capacity;
        string newText = _nameOfUnits + " - " + (_capacity - _availabelCapacity) + "/" + _capacity;
        _textCapacityUI.text = newText;
        _shadowCapacityTextUI.text = newText;
    }
    void UpdateUpgradeUI()
    {
        _upgradeCount.text = "Update attack " + _currentUpdate + "/" + _updatePrice.Count;
        _upgradeStonePrice.text = _updatePrice[_currentUpdate].ToString();
    }
    public override void ReturnUnit()
    {
        _availabelCapacity += 1;
        RegualUpdateUI();
    }
    public void TryUpgradeAllKnightUnits()
    {
        int stoneBalance = Resources.Instance.CheckBalance("stone");
        if (_currentUpdate < _updatePrice.Count + 1)
        {
            if (stoneBalance >= _updatePrice[_currentUpdate])
            {
                Resources.Instance.UpdateResource("stone", stoneBalance - _updatePrice[_currentUpdate]);
                _currentUpdate += 1;
                Knight[] allKnights = FindObjectsOfType<Knight>();
                for (int i = 0; i < allKnights.Length; i++)
                {
                    allKnights[i].ChangeAttackPower(1);
                }
                UpdateUpgradeUI();
            }
            else
            {
                GameManager.Instance._showHint.DisplayHint("You can't buy this upgrade. Need more stone");
            }
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("You can't buy this unit. No more slots");
        }
    }
    public void TryBuyUnit(Unit unit)
    {
        int balance = Resources.Instance.CheckBalance("gold");
        int price = unit.CheckPrice();
        if (_availabelCapacity > 0)
        {
            if (balance >= price)
            {
                Unit newUnit = Instantiate(unit, _spawnPoint.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-0.5f, 0.5f)), Quaternion.identity);
                _currentUnit = newUnit;
                Resources.Instance.UpdateResource("gold", balance - price);
                UpdateUI(1);
                newUnit.SetLivingBuilding(this);
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
    private void OnDestroy()
    {
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        BuildingPlacer.Instance.DeleteBuilding(x, z, this);
    }
}
