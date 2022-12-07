using TMPro;
using UnityEngine;

public class UISettingOnPlay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textGold;
    [SerializeField] TextMeshProUGUI _textWood;
    [SerializeField] TextMeshProUGUI _textStone;
    //[SerializeField] TextMeshProUGUI _textShadow;

    public void SetTextPrice(Building _building)
    {
        if (_textGold != null)
        {
            _textGold.text = _building.CheckPrice("gold").ToString() + "$";
        }
        if (_textWood != null)
        {
            _textWood.text = _building.CheckPrice("wood").ToString();
        }
        if (_textStone != null)
        {
            _textStone.text = _building.CheckPrice("stone").ToString();
        }
        //_textShadow.text = _building.CheckPrice("gold").ToString() + "$";

    }
}
