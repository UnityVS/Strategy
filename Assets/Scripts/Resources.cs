using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] int _money;
    [SerializeField] TextMeshProUGUI _textMoney;
    [SerializeField] TextMeshProUGUI _textShadowMoney;
    [SerializeField] int _wood;
    [SerializeField] TextMeshProUGUI _textWood;
    [SerializeField] TextMeshProUGUI _textShadowWood;
    [SerializeField] int _stone;
    [SerializeField] TextMeshProUGUI _textStone;
    [SerializeField] TextMeshProUGUI _textShadowStone;
    private void Awake()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        _textMoney.text = _money.ToString();
        _textShadowMoney.text = _money.ToString();
        _textWood.text = _wood.ToString();
        _textShadowWood.text = _wood.ToString();
        _textStone.text = _stone.ToString();
        _textShadowStone.text = _stone.ToString();
    }
    public int CheckBalance()
    {
        return _money;
    }
    public void AddResources(FarmResource _farmResource, int resources)
    {
        if (_farmResource == FarmResource.Gold)
        {
            _money += resources;
            UpdateUI();
        }
        else if (_farmResource == FarmResource.Wood)
        {
            _wood += resources;
            UpdateUI();
        }
        else if (_farmResource == FarmResource.Stone)
        {
            _stone += resources;
            UpdateUI();
        }
    }
    public void UpdateResource(int resource)
    {
        _money = resource;
        UpdateUI();
    }
}
