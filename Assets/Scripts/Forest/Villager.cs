using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemTypes
{
    None,
    Meat
}

public class Villager : MonoBehaviour
{
    public Slider _fullnessSlider;

    private ItemTypes _carriedItem;

    [SerializeField] private float _energyBurnRate = 0.5f;//Determines how many % fullness is lost per minute
    [SerializeField] private float _fullness;

    private void Awake()
    {
        _Fullness = 1;
    }

    private void Update()
    {
        _Fullness -= Time.deltaTime * _energyBurnRate / 60;
    }

    public void EatMeat()
    {
        if(_CarriedItem == ItemTypes.Meat)
        {
            _CarriedItem = ItemTypes.None;
            _Fullness += 1f;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public float _Fullness
    {
        get => _fullness;
        set
        {
            _fullness = Mathf.Clamp(value, 0, 1);
            _fullnessSlider.value = _fullness;
            if (_fullness <= 0)
                Die();
        }
    }

    public ItemTypes _CarriedItem
    {
        get => _carriedItem;
        set
        {
            _carriedItem = value;
        }
    }
}
