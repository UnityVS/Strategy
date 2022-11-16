using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] GameObject _selectCirle;
    private void Start()
    {
        _selectCirle.SetActive(false);
    }
    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }
    public virtual void OnUnhover()
    {
        transform.localScale = Vector3.one;
    }
    public virtual void OnSelect()
    {
        _selectCirle.SetActive(true);
    }
    public virtual void OnUnselect()
    {
        _selectCirle.SetActive(false);
    }
}
