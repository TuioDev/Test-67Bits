using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null) _Instance = FindFirstObjectByType<GameManager>();
            return _Instance;
        }
    }

    public static event Action<int> OnCarryingUpdate;
    public static event Action<int> OnMaxCapacityUpdate;

    public void UpdateMaxCapacity(int amount)
    {
        OnMaxCapacityUpdate?.Invoke(amount);
    }

    public void UpdateCarrying(int amount)
    {
        OnCarryingUpdate?.Invoke(amount);
    }
}
