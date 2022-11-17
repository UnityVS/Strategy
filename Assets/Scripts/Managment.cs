using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Managment : MonoBehaviour
{
    Camera _camera;
    SelectableObject _hovered;
    List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    [SerializeField] Image _frameSelect;
    Vector2 _frameStart;
    Vector2 _frameEnd;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _frameSelect.enabled = false;
    }
    private void Update()
    {
        CheckSelect();
        FrameSelect();
    }
    void FrameSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _frameSelect.enabled = true;
            _frameStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;
            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            _frameSelect.rectTransform.anchoredPosition = min;
            Vector2 size = max - min;
            _frameSelect.rectTransform.sizeDelta = size;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _frameSelect.enabled = true;
        }
    }
    void CheckSelect()
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
            if (hit.collider.tag == "Ground")
            {
                for (int i = 0; i < _listOfSelected.Count; i++)
                {
                    _listOfSelected[i].WhenClickOnGround(hit.point);
                }
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
