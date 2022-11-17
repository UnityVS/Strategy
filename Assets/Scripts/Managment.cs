using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    Camera _camera;
    SelectableObject _hovered;
    List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>() is SelectableCollider selectableCollider)
            {
                SelectableObject hitSelectable = selectableCollider.SelectableObject;
                if (_hovered)
                {
                    if (hitSelectable != _hovered)
                    {
                        _hovered.OnUnhover();
                        _hovered = hitSelectable;
                    }
                }
                else
                {
                    _hovered = hitSelectable;
                }
                _hovered.OnHover();
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }
                Select(_hovered);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnselectAll();
        }
    }
    void Select(SelectableObject selectableObject)
    {
        if (!_listOfSelected.Contains(selectableObject))
        {
            _listOfSelected.Add(selectableObject);
            selectableObject.OnSelect();
        }
    }
    void UnselectAll()
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            _listOfSelected[i].OnUnselect();
        }
        _listOfSelected.Clear();
    }
    void UnhoverCurrent()
    {
        if (_hovered)
        {
            _hovered.OnUnhover();
            _hovered = null;
        }
    }
}
