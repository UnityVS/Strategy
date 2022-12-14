using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other,
    UI
}

public class Managment : MonoBehaviour
{
    Camera _camera;
    [SerializeField] Transform _cameraMoveController;
    [SerializeField] float _cameraMoveSpeed;
    SelectableObject _hovered;
    List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    [SerializeField] Image _frameSelect;
    Vector2 _frameStart;
    Vector2 _frameEnd;
    SelectionState _currentSelectionState;
    public static Managment Instance;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _yMin = 5f;
    [SerializeField] float _yMax = 10f;
    [SerializeField] float _xMinMax = 20f;
    [SerializeField] float _zMinMax = 15f;
    float _shiftSpeed = 2;
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
    private void LateUpdate()
    {
        CheckSelect();
        FrameSelect();
        float decreasteSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            decreasteSpeed = _shiftSpeed;
        }
        else
        {
            decreasteSpeed = 1f;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (_camera.transform.position.y >= _yMax || _camera.transform.position.y <= _yMin)
            {
                if (_camera.transform.position.y >= _yMax && Input.mouseScrollDelta.y > 0)
                {
                    _camera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
                }
                else if (_camera.transform.position.y <= _yMin && Input.mouseScrollDelta.y < 0)
                {
                    _camera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
                }
                return;
            }
            _camera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            CameraDirectionMove(new Vector3(0f, 0f, _cameraMoveSpeed * Time.deltaTime), decreasteSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            CameraDirectionMove(new Vector3(0f, 0f, -_cameraMoveSpeed * Time.deltaTime), decreasteSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CameraDirectionMove(new Vector3(_cameraMoveSpeed * Time.deltaTime, 0f, 0f), decreasteSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            CameraDirectionMove(new Vector3(-_cameraMoveSpeed * Time.deltaTime, 0f, 0f), decreasteSpeed);
        }
    }
    void CameraDirectionMove(Vector3 direction, float decreasteSpeed)
    {
        if (_cameraMoveController.transform.position.x >= _xMinMax || _cameraMoveController.transform.position.x <= -_xMinMax)
        {
            if (_cameraMoveController.transform.position.x >= _xMinMax && direction.x < 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
            }
            else if (_cameraMoveController.transform.position.x <= -_xMinMax && direction.x > 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
            }
            else if (_cameraMoveController.transform.position.z <= _zMinMax && direction.x == 0 || _cameraMoveController.transform.position.z >= -_zMinMax && direction.x == 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
                return;
            }
            return;
        }
        if (_cameraMoveController.transform.position.z >= _zMinMax || _cameraMoveController.transform.position.z <= -_zMinMax)
        {
            if (_cameraMoveController.transform.position.z >= _zMinMax && direction.z < 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
            }
            else if (_cameraMoveController.transform.position.z <= -_zMinMax && direction.z > 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
            }
            else if (_cameraMoveController.transform.position.x <= _xMinMax && direction.z == 0 || _cameraMoveController.transform.position.x >= -_xMinMax && direction.z == 0)
            {
                _cameraMoveController.Translate(direction * decreasteSpeed);
                return;
            }
            return;
        }
        _cameraMoveController.Translate(direction * decreasteSpeed);
    }
    public List<SelectableObject> ListObjects()
    {
        return _listOfSelected;
    }
    void FrameSelect()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _frameStart = Input.mousePosition;
            _currentSelectionState = SelectionState.Other;
        }
        else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            _currentSelectionState = SelectionState.UI;
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _frameEnd = Input.mousePosition;
            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;
            if (size.magnitude > 10 && _currentSelectionState != SelectionState.UI)
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
        if (Physics.Raycast(ray, out hit, 50, _layerMask, QueryTriggerInteraction.Ignore))
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
            if (_hovered && !EventSystem.current.IsPointerOverGameObject())
            {
                if (_listOfSelected.Count > 0 && _hovered.GetComponent<CollectableObject>())
                {
                    if (_listOfSelected[0].GetComponent<Building>())
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected[0].GetComponent<CollectableObject>())
                    {
                        UnselectAll();
                    }
                    if (_listOfSelected.Count > 1)
                    {
                        if (_listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<CollectableObject>())
                        {
                            _listOfSelected[1].OnUnselect();
                            _listOfSelected.RemoveAt(1);

                        }
                        else if (_listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Building>())
                        {
                            _listOfSelected[1].OnUnselect();
                            _listOfSelected.RemoveAt(1);
                        }
                        else if (_listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Unit>())
                        {
                            UnselectAll();
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            UnselectAll();
                        }
                    }
                }
                else
                {
                    if (_listOfSelected.Count > 0 && _listOfSelected[0].GetComponent<CollectableObject>() && !_hovered.GetComponent<Unit>() && _hovered != null)
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected.Count > 0 && _listOfSelected[0].GetComponent<Building>() && !_listOfSelected[0].GetComponent<CollectableObject>() && _hovered.GetComponent<Building>())
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected.Count > 0 && _listOfSelected[0].GetComponent<Building>() && !_listOfSelected[0].GetComponent<CollectableObject>() && _hovered.GetComponent<Unit>())
                    {
                        UnselectAll();
                    }
                    else if (!Input.GetKey(KeyCode.LeftControl) && _listOfSelected.Count > 0 && _listOfSelected[0].GetComponent<Unit>() && _hovered.GetComponent<Unit>())
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[1].GetComponent<Building>() && !_hovered.GetComponent<Building>())
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Building>() && _hovered.GetComponent<Building>())
                    {
                        _listOfSelected[1].OnUnselect();
                        _listOfSelected.RemoveAt(1);
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Building>() && _hovered.GetComponent<CollectableObject>())
                    {
                        _listOfSelected[1].OnUnselect();
                        _listOfSelected.RemoveAt(1);
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<CollectableObject>() && _hovered.GetComponent<CollectableObject>())
                    {
                        _listOfSelected[1].OnUnselect();
                        _listOfSelected.RemoveAt(1);
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<CollectableObject>() && _hovered.GetComponent<Building>())
                    {
                        _listOfSelected[1].OnUnselect();
                        _listOfSelected.RemoveAt(1);
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Unit>() && _hovered.GetComponent<Building>())
                    {
                        UnselectAll();
                    }
                    else if (_listOfSelected.Count > 1 && _listOfSelected[0].GetComponent<Unit>() && _listOfSelected[1].GetComponent<Unit>() && _hovered.GetComponent<CollectableObject>())
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
            if (Input.GetMouseButtonUp(0) && hit.collider.tag == "Ground" && !EventSystem.current.IsPointerOverGameObject())
            {
                UnselectAll();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (hit.collider.tag == "Ground" && !EventSystem.current.IsPointerOverGameObject())
                {
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(_listOfSelected.Count));
                    for (int i = 0; i < _listOfSelected.Count; i++)
                    {
                        int row = i / rowNumber;
                        int column = i % rowNumber;
                        Vector3 point = hit.point + new Vector3(row, 0f, column);
                        _listOfSelected[i].WhenClickOnGround(point);
                    }
                }
            }
        }
        else if (_currentSelectionState == SelectionState.Other)
        {
            if (Input.GetMouseButtonUp(0) && hit.collider.tag == "Ground" && !EventSystem.current.IsPointerOverGameObject())
            {
                UnselectAll();
            }
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
    public void UnselectAll()
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            _listOfSelected[i].OnUnselect();
        }
        _currentSelectionState = SelectionState.Other;
        _listOfSelected.Clear();
    }
    public void UnselectIfSelect(Unit unit)
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            if (unit == _listOfSelected[i])
            {
                _listOfSelected[i].OnUnselect();
                _listOfSelected.RemoveAt(i);
            }
        }

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
