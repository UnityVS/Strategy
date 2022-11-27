using System.Collections;
using TMPro;
using UnityEngine;

public enum FarmResource
{
    Gold,
    Stone,
    Wood
}

public class Mine : PlayerBuildings
{
    bool _isActive = false;
    [SerializeField] AddedValue _addedValue;
    [SerializeField] float _timeResource = 1.5f;
    [SerializeField] int _generatedCount = 10;
    public FarmResource _currentFarm;
    [SerializeField] Coroutine _coroutine;
    [SerializeField] int _capacity = 2;
    [SerializeField] int _availabelCapacity = 2;
    [SerializeField] string _nameOfBuilding;
    [SerializeField] string _nameOfBuyingElement;
    [SerializeField] TextMeshProUGUI _textNameBuilding;
    [SerializeField] TextMeshProUGUI _textNameBuildingShadow;
    [SerializeField] TextMeshProUGUI _textCapacityUI;
    [SerializeField] TextMeshProUGUI _shadowCapacityTextUI;
    [SerializeField] TextMeshProUGUI _priceUIOriginal;
    [SerializeField] TextMeshProUGUI _priceUIShadow;
    [SerializeField] int[] _updatePrice;
    [SerializeField] int[] _updateValue;
    [SerializeField] TextMeshProUGUI _textDescription;
    [SerializeField] TextMeshProUGUI _textDescriptionShadow;
    [SerializeField] bool _workWithUnit;
    public override void Start()
    {
        UpdateUI(0);
        _textDescription.text = "You'll get " + _updateValue[_capacity - _availabelCapacity] + "+ materials";
        _textNameBuilding.text = _nameOfBuilding;
        _textNameBuildingShadow.text = _nameOfBuilding;
        _priceUIOriginal.text = _updatePrice[_capacity - _availabelCapacity] + "$";
        _priceUIShadow.text = _updatePrice[_capacity - _availabelCapacity] + "$";
        ShowHideUI(false);
    }
    public void ChangeGenerationCount(int value)
    {
        _generatedCount += value;
        _addedValue.SetValueMining(_generatedCount);
    }
    public override void OnHover()
    {
    }
    void Update()
    {
        if (_workWithUnit) return;
        if (_isActive == false)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            return;
        }
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(AddValue());
        }
    }
    public void FromUnitAddValue()
    {
        _addedValue.AddResource(_timeResource);
        Resources.Instance.AddResources(_currentFarm, _generatedCount);
    }
    public void BuildingSetInScene()
    {
        SelectObjectStatus(false);
    }
    public void MineWork()
    {
        _addedValue.SetValueMining(_generatedCount);
        SelectObjectStatus(false);
        _addedValue.SetStartPosition();
        _isActive = true;
    }
    IEnumerator AddValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeResource);
            _addedValue.AddResource(_timeResource);
            yield return new WaitForSeconds(_timeResource / 2);
            Resources.Instance.AddResources(_currentFarm, _generatedCount);
        }
    }
    void UpdateUI(int valueAdd)
    {
        _availabelCapacity -= valueAdd;
        if (_availabelCapacity != 0)
        {
            _textDescription.text = "You'll get " + _updateValue[_capacity - _availabelCapacity] + "+ materials";
            _priceUIOriginal.text = _updatePrice[_capacity - _availabelCapacity] + "$";
            _priceUIShadow.text = _updatePrice[_capacity - _availabelCapacity] + "$";
        }
        string newText = _nameOfBuyingElement + " - " + (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
        _textCapacityUI.text = newText;
        _shadowCapacityTextUI.text = newText;
    }
    public void TryBuyUpdate()
    {
        int balance = Resources.Instance.CheckBalance();
        if (_availabelCapacity > 0)
        {
            if (balance >= (int)(_updatePrice[_capacity - _availabelCapacity]))
            {
                Resources.Instance.UpdateResource(balance - (int)(_updatePrice[_capacity - _availabelCapacity]));
                ChangeGenerationCount(_updateValue[_capacity - _availabelCapacity]);
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
