using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectablePrefabUI : MonoBehaviour
{
    public TMP_Dropdown _dropdown;
    public GameObject content; // The parent object that contains the dropdown items
    public ScrollRect scrollRect;
    public GameObject _selectedPrefab;
    public Transform _spawnPoint; 
    private List<GameObject> prefabs;
    private bool _initialized = false; 
    private void Awake()
    {
        _spawnPoint = GameObject.FindGameObjectWithTag("Scenario").transform;
        _dropdown = GetComponentInChildren<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(SelectItemRequested);
    }
    void Start()
    {
        LoadPrefabs();
        PopulateDropdown();
        InitializeData();
    }

    void LoadPrefabs()
    {
        // Load all prefabs from the Resources/Prefab directory
        prefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/Selectables"));
    }

    void PopulateDropdown()
    {
        // Clear current options
        _dropdown.ClearOptions();

        // Create a list to hold the prefab names
        List<string> options = new List<string>();

        // Add prefab names to the options list
        foreach (GameObject prefab in prefabs)
        {
            options.Add(prefab.name);
        }

        // Add options to the dropdown
        _dropdown.AddOptions(options);
    }

    void InitializeData()
    {
        GameObject initialObj = GameObject.FindGameObjectWithTag("Initial");
        for (int i = 0; i < prefabs.Count; i++)
        {
            GameObject prefab = prefabs[i];
            if (prefab.name == initialObj.name)
            {
                _selectedPrefab = initialObj;
                _dropdown.captionText.text = _selectedPrefab.name;
            }
        }
        _initialized = true; 
    }

    void SelectItemRequested(int index)
    {
        if (_selectedPrefab != null)
        {
            Destroy(_selectedPrefab);
        }
        if (!_initialized)
        {
            return;
        }

        _selectedPrefab = Instantiate(prefabs[index], _spawnPoint.position, _spawnPoint.rotation);
        _selectedPrefab.transform.SetParent(_spawnPoint.transform);
    }
}