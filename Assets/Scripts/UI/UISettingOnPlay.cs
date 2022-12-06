using TMPro;
using UnityEngine;

public class UISettingOnPlay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextMeshProUGUI _textShadow;

    public void SetTextPrice(Building _building)
    {
        _text.text = _building.CheckPrice("gold").ToString() + "$";
        _textShadow.text = _building.CheckPrice("gold").ToString() + "$";
    }
}
