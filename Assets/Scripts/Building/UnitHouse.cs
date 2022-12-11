using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitHouse : MonoBehaviour
{
    [SerializeField] Image _buttonUnit;
    [SerializeField] Image _buttonKickUnit;
    [SerializeField] TextMeshProUGUI _countField;
    [SerializeField] TextMeshProUGUI _countFieldShadow;
    [SerializeField] Sprite _imageQuestion;
    [SerializeField] Sprite _imageApprove;
    Unit _currentUnit = null;
    Unit _targetUnit = null;
    [SerializeField] Mine _mine;
    private void Start()
    {
        _countField.text = "Жителей - 0/1";
        _countFieldShadow.text = "Жителей - 0/1";
        _buttonKickUnit.gameObject.SetActive(false);
        _buttonUnit.sprite = _imageQuestion;
        _buttonUnit.color = Color.yellow;
        _buttonUnit.GetComponent<Button>().enabled = false;
    }
    private void Update()
    {
        if (_targetUnit != null && !_targetUnit.gameObject.activeSelf)
        {
            _buttonUnit.sprite = _imageQuestion;
            _buttonUnit.color = Color.yellow;
            _targetUnit = null;
            _buttonUnit.GetComponent<Button>().enabled = false;
        }
        if (_currentUnit != null && _mine != null)
        {
            _mine.MiningStatus(true);
        }
        else
        {
            _mine.MiningStatus(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.attachedRigidbody.gameObject.activeSelf) return;
        if (_targetUnit = other.attachedRigidbody.GetComponent<Citizen>())
        {
            if (_currentUnit == null)
            {
                _buttonUnit.sprite = _imageApprove;
                _buttonUnit.color = Color.green;
                _buttonUnit.GetComponent<Button>().enabled = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.attachedRigidbody.gameObject.activeSelf) return;
        if (_targetUnit == other.attachedRigidbody.GetComponent<Citizen>())
        {
            if (!_currentUnit)
            {
                _buttonUnit.GetComponent<Button>().enabled = false;
                _targetUnit = null;
                _buttonUnit.sprite = _imageQuestion;
                _buttonUnit.color = Color.yellow;
            }
        }
    }
    public void SetUnitToWork()
    {
        _buttonUnit.sprite = _imageQuestion;
        _buttonUnit.color = Color.yellow;
        _buttonUnit.gameObject.SetActive(false);
        _currentUnit = _targetUnit;
        _targetUnit = null;
        _buttonKickUnit.gameObject.SetActive(true);
        _countField.text = "Жителей - 1/1";
        _countFieldShadow.text = "Жителей - 1/1";
        _currentUnit.gameObject.SetActive(false);
    }
    public void KickUnitFrmoBuilding()
    {
        _buttonKickUnit.gameObject.SetActive(false);
        _buttonUnit.gameObject.SetActive(true);
        _buttonUnit.sprite = _imageQuestion;
        _buttonUnit.color = Color.yellow;
        _countField.text = "Жителей - 0/1";
        _countFieldShadow.text = "Жителей - 0/1";
        _currentUnit.gameObject.SetActive(true);
        _currentUnit = null;
    }
}
