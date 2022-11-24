using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject _selectCirle;
    public virtual void Start()
    {
        _selectCirle.SetActive(true);
    }
    public virtual void OnHover()
    {
    }
    public virtual void OnUnhover()
    {

    }
    public virtual void OnSelect()
    {
        _selectCirle.SetActive(true);
    }
    public virtual void OnUnselect()
    {
        if (!this) return;
        _selectCirle.SetActive(false);
    }
    public virtual void WhenClickOnGround(Vector3 point)
    {

    }
    public void SelectObjectStatus(bool status)
    {
        _selectCirle.SetActive(status);
    }
}
