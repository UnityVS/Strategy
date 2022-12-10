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
    public static Resources Instance;
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
    public int CheckBalance(FarmResource farmResource)
    {
        switch (farmResource)
        {
            case FarmResource.Gold:
                return _money;
            case FarmResource.Stone:
                return _stone;
            case FarmResource.Wood:
                return _wood;
            default:
                return _money;
        }
    }
    public void AddResources(FarmResource farmResource, int resource)
    {
        switch (farmResource)
        {
            case FarmResource.Gold:
                _money += resource;
                break;
            case FarmResource.Stone:
                _stone += resource;
                break;
            case FarmResource.Wood:
                _wood += resource;
                break;
            default:
                break;
        }
        UpdateUI();
    }
    public void UpdateResource(FarmResource farmResource, int resource)
    {
        switch (farmResource)
        {
            case FarmResource.Gold:
                _money = resource;
                break;
            case FarmResource.Stone:
                _stone = resource;
                break;
            case FarmResource.Wood:
                _wood = resource;
                break;
            default:
                break;
        }
        UpdateUI();
    }
}
