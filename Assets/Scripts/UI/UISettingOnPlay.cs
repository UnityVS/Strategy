using TMPro;
using UnityEngine;

public class UISettingOnPlay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textGold;
    [SerializeField] TextMeshProUGUI _textWood;
    [SerializeField] TextMeshProUGUI _textStone;
    public void SetTextPrice(Building _building)
    {
        if (_textGold != null)
        {
            _textGold.text = _building.CheckPrice(FarmResource.Gold).ToString() + "$";
        }
        if (_textWood != null)
        {
            _textWood.text = _building.CheckPrice(FarmResource.Wood).ToString();
        }
        if (_textStone != null)
        {
            _textStone.text = _building.CheckPrice(FarmResource.Stone).ToString();
        }
    }
}
