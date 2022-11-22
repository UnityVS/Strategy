using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Barack : Building
{
    [SerializeField] int _capacity = 2;
    [SerializeField] int _availabelCapacity = 2;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _shadowCapacityText;
    private void Start()
    {
        _textCapacity.text = (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
        _shadowCapacityText.text = (_capacity - _availabelCapacity).ToString() + "/" + _capacity.ToString();
    }
    public void BuildingSetInScene()
    {
        SelectObjectStatus(false);
    }
}
