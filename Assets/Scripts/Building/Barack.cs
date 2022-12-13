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
    [SerializeField] TextMeshProUGUI _currentUnitAttack;
    int _currentAttack = 1;
    List<Unit> _unitsOfThisBuilding = new List<Unit>();
    [SerializeField] List<int> _buyCapacityGoldPrice;
    [SerializeField] List<int> _buyCapacityStonePrice;
    int _currentUpgradeLvlOfCapacity = 0;
    [SerializeField] TextMeshProUGUI _textUpgradeUnitsCount;
    [SerializeField] TextMeshProUGUI _textGoldPrice;
    [SerializeField] TextMeshProUGUI _textStonePrice;
    [SerializeField] TextMeshProUGUI _textHowValueWillBeAdded;
    int attackToAdd = 1;
    override public void Start()
    {
        UpdateUI(0);
        _textNameOfBarack.text = _nameOfBarack;
        _textNameOfBarackShadow.text = _nameOfBarack;
        _priceUIOriginal.text = _unitToCreate.CheckPrice().ToString() + "$";
        _priceUIShadow.text = _unitToCreate.CheckPrice().ToString() + "$";
        if (_updatePrice.Capacity != 0)
        {
            UpdateUpgradeUI();
        }
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
    public void IncreasteCountOfUnits()
    {
        int stoneBalance = Resources.Instance.CheckBalance(FarmResource.Stone);
        int goldBalance = Resources.Instance.CheckBalance(FarmResource.Gold);
        if (_currentUpgradeLvlOfCapacity != _buyCapacityGoldPrice.Count)
        {
            if (goldBalance >= _buyCapacityGoldPrice[_currentUpgradeLvlOfCapacity])
            {
                if (stoneBalance >= _buyCapacityStonePrice[_currentUpgradeLvlOfCapacity])
                {
                    _capacity++;
                    _availabelCapacity++;
                    _currentUpgradeLvlOfCapacity++;
                    RegualUpdateUI();
                    Resources.Instance.UpdateResource(FarmResource.Stone, stoneBalance - _buyCapacityStonePrice[_currentUpgradeLvlOfCapacity - 1]);
                    Resources.Instance.UpdateResource(FarmResource.Gold, goldBalance - _buyCapacityStonePrice[_currentUpgradeLvlOfCapacity - 1]);
                }
                else
                {
                    GameManager.Instance._showHint.DisplayHint("Нехватает камня");
                }
            }
            else
            {
                GameManager.Instance._showHint.DisplayHint("Нехватает золота");
            }
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("Больше нет расширений");
        }
    }
    void UpdateUI(int valueAdd)
    {
        _availabelCapacity -= valueAdd;
        RegualUpdateUI();
    }
    void RegualUpdateUI()
    {
        if (_textUpgradeUnitsCount != null)
        {
            if (_currentUpgradeLvlOfCapacity != _buyCapacityGoldPrice.Count)
            {
                _textGoldPrice.text = _buyCapacityGoldPrice[_currentUpgradeLvlOfCapacity] + "$";
                _textStonePrice.text = _buyCapacityStonePrice[_currentUpgradeLvlOfCapacity].ToString();
            }
            else if (_textGoldPrice != null)
            {
                Destroy(_textGoldPrice.transform.parent.gameObject);
            }
        }
        _textUpgradeUnitsCount.text = "Увеличить число \nжителей - " + _currentUpgradeLvlOfCapacity + "/" + _buyCapacityGoldPrice.Count;
        if (_currentUnitAttack != null)
        {
            _textHowValueWillBeAdded.text = "Добавит +" + attackToAdd + "к аттаке юнитам";
            _currentUnitAttack.text = "Атака юнитов этого здания \n" + _currentAttack + " урон в секунду";
        }
        _textCapacity.text = (_capacity - _availabelCapacity) + "/" + _capacity;
        _shadowCapacityText.text = (_capacity - _availabelCapacity) + "/" + _capacity;
        string newText = _nameOfUnits + " - " + (_capacity - _availabelCapacity) + "/" + _capacity;
        _textCapacityUI.text = newText;
        _shadowCapacityTextUI.text = newText;
    }
    void UpdateUpgradeUI()
    {
        //if (_currentUpdate - 1 == _updatePrice.Count) return;
        _upgradeCount.text = "Улучшить аттаку " + _currentUpdate + "/" + _updatePrice.Count;
        if (_currentUpdate == _updatePrice.Count)
        {
            _upgradeStonePrice.text = _updatePrice[_currentUpdate - 1].ToString();
            Destroy(_upgradeStonePrice.transform.parent.gameObject);
            return;
        }
        _upgradeStonePrice.text = _updatePrice[_currentUpdate].ToString();
        RegualUpdateUI();
    }
    public override void ReturnUnit()
    {
        _availabelCapacity += 1;
        RegualUpdateUI();
    }
    public void TryUpgradeAllKnightUnits()
    {
        int stoneBalance = Resources.Instance.CheckBalance(FarmResource.Stone);
        if (_currentUpdate - 1 < _updatePrice.Count)
        {
            if (_updatePrice.Count != _currentUpdate)
            {
                if (stoneBalance >= _updatePrice[_currentUpdate])
                {
                    Resources.Instance.UpdateResource(FarmResource.Stone, stoneBalance - _updatePrice[_currentUpdate]);
                    _currentUpdate += 1;
                    if (_unitsOfThisBuilding.Count != 0)
                    {
                        for (int i = 0; i < _unitsOfThisBuilding.Count; i++)
                        {
                            _unitsOfThisBuilding[i].GetComponent<Knight>().ChangeAttackPower(1);
                        }
                    }
                    UpdateUpgradeUI();
                    _currentAttack++;
                    RegualUpdateUI();
                }
                else
                {
                    GameManager.Instance._showHint.DisplayHint("Вы не можете улучшить здание. Нужно больше камня");
                }
            }
            else
            {
                GameManager.Instance._showHint.DisplayHint("Больше нет обновлений");
            }
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("Вы не можете купить юнита. Нет больше свободных мест");
        }
    }
    public override void TryBuyUnit(Unit unit)
    {
        int balance = Resources.Instance.CheckBalance(FarmResource.Gold);
        int price = unit.CheckPrice();
        if (_availabelCapacity > 0)
        {
            if (balance >= price)
            {
                Unit newUnit = Instantiate(unit, _spawnPoint.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-0.5f, 0.5f)), Quaternion.identity);
                _currentUnit = newUnit;
                Resources.Instance.UpdateResource(FarmResource.Gold, balance - price);
                UpdateUI(1);
                newUnit.SetLivingBuilding(this);
                if (newUnit.GetComponent<Knight>() is Knight _knightUnit)
                {
                    _knightUnit.SetAttackPower(_currentAttack);
                }
                SetDeleteUnitToThisBuilding(newUnit, true);
            }
            else
            {
                GameManager.Instance._showHint.DisplayHint("Вы не можете купить юнита. Нужно больше денег");
            }
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("Вы не можете купить юнита. Нет больше свободных мест");
        }
    }
    public override void SetDeleteUnitToThisBuilding(Unit newUnit, bool createNew)
    {
        if (createNew)
        {
            _unitsOfThisBuilding.Add(newUnit);
        }
        else
        {
            _unitsOfThisBuilding.Remove(newUnit);
        }
    }
    public void SellBuilding()
    {
        if (_goldPrice != 0)
        {
            Resources.Instance.AddResources(FarmResource.Gold, _goldPrice / 2);
        }
        if (_stonePrice != 0)
        {
            Resources.Instance.AddResources(FarmResource.Stone, _stonePrice / 2);
        }
        if (_woodPrice != 0)
        {
            Resources.Instance.AddResources(FarmResource.Wood, _woodPrice / 2);
        }
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        BuildingPlacer.Instance.DeleteBuilding(x, z, this);
        Managment.Instance.UnselectAll();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        BuildingPlacer.Instance.DeleteBuilding(x, z, this);
    }
}
