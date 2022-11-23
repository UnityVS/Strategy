using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    //[SerializeField] GameObject _panel;
    [SerializeField] Transform _navigation;
    public void SwitchActiveState(GameObject panel)
    {
        //_panel.SetActive(!_panel.activeSelf);
        for (int i = 0; i < _navigation.childCount; i++)
        {
            if (_navigation.GetChild(i).gameObject == panel)
            {
                panel.SetActive(!panel.activeSelf);
            }
            else
            {
                _navigation.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
