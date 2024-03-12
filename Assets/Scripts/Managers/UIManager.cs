using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerCapacityUI;

    private int _currentMaxCapacity;
    private int _currentAmountCarrying = 0;

    private void Start()
    {
        UpdateUIText();
    }

    private void OnEnable()
    {
        GameManager.OnMaxCapacityUpdate += UpdateMaxCapacity;
        GameManager.OnCarryingUpdate += UpdateCarrying;
    }

    private void OnDisable()
    {
        GameManager.OnMaxCapacityUpdate -= UpdateMaxCapacity;
        GameManager.OnCarryingUpdate -= UpdateCarrying;
    }

    public void UpdateMaxCapacity(int amount)
    {
        _currentMaxCapacity += amount;
        UpdateUIText();
    }

    public void UpdateCarrying(int amount)
    {
        _currentAmountCarrying = amount;
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        _playerCapacityUI.text = _currentAmountCarrying.ToString() + "/" + _currentMaxCapacity.ToString();
    }
}
