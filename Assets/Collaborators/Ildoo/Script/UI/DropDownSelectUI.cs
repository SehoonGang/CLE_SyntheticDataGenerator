using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DropDownSelectUI<T> : BaseUI
{
    public TMP_Dropdown _dropdown;
    public GameObject content; // The parent object that contains the dropdown items
    public ScrollRect scrollRect;
    public T _selectedPrefab;
    public Transform _spawnPoint; 
    protected List<T> _items;
    protected bool _initialized = false; 
    protected override void Awake()
    {
        base.Awake();
        _dropdown = GetComponentInChildren<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(SelectItemRequested);
    }
    protected abstract void LoadItems();

    protected abstract void PopulateDropdown();

    protected abstract void Init();

    protected abstract void SelectItemRequested(int index);
}