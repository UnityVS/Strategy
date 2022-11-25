using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Managment : MonoBehaviour
{
    Camera _camera;
    SelectableObject _hovered;
    List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    [SerializeField] Image _frameSelect;
    Vector2 _frameStart;
    Vector2 _frameEnd;
    SelectionState _currentSelectionState;
    CollectableObject _currentCollectableObject;
    public static Managment Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _frameSelect.enabled = false;
    }
    private void Update()
    {
        CheckSelect();
        FrameSelect();
        _camera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
    }
    public List<SelectableObject> ListObjects()
    {
        return _listOfSelected;
    }
    void FrameSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;
            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;
            if (size.magnitude > 10)
            {
                _frameSelect.enabled = true;
                _frameSelect.rectTransform.anchoredPosition = min;
                _frameSelect.rectTransform.sizeDelta = size;
                Rect rect = new Rect(min, size);
                Unit[] allUnits = FindObjectsOfType<Unit>();
                UnselectAll();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPosition = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(allUnits[i]);
                    }
                }
                _currentSelectionState = SelectionState.Frame;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            _frameSelect.enabled = false;
            if (_listOfSelected.Count > 0)
            {
                _currentSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                _currentSelectionState = SelectionState.Other;
            }
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
                if (_hovered.GetComponent<CollectableObject>() is CollectableObject _currentCollectableObject)
                {
                    if (!Input.GetKey(KeyCode.LeftControl) && !_currentCollectableObject)
                    {
                        UnselectAll();
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        UnselectAll();
                    }
                }
                _currentSelectionState = SelectionState.UnitsSelected;
                Select(_hovered);
            }
        }
        if (_currentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground" && !EventSystem.current.IsPointerOverGameObject())
                {
                    for (int i = 0; i < _listOfSelected.Count; i++)
                    {
                        _listOfSelected[i].WhenClickOnGround(hit.point);
                    }
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
        if (_listOfSelected.Count > 1)
        {
            _listOfSelected[1].OnUnselect();
            _listOfSelected.RemoveAt(1);
        }
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
        _currentSelectionState = SelectionState.Other;
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
