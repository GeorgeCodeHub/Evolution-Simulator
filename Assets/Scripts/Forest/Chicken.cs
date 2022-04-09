using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Chicken : MonoBehaviour
{
    public float _baseEnergyBurn = 0.25f;
    public float _energyPerSpeed = 0.1f;//How much more energy is burned per _moveSpeed
    public float _energyPerRange = 0.025f;//How much energy is burned per _detectionRange
    public Slider _fullnessSlider;
    public GameObject _chickenPrefab;

    public event Action OnDeath;

    [SerializeField] private float _fullness;
    private float _timeSinceLastPregnancy = 0;
    private ParticleSystem _ps;
    private bool _isBurningEnergy = true;

    void Awake()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        GetComponent<NavMeshAgent>().speed = _MoveSpeed;
    }

    void Update()
    {
        float energyBurnRate = _MoveSpeed * _energyPerSpeed + LevelManager.startingSize * _energyPerRange;
        _timeSinceLastPregnancy += Time.deltaTime;
        if (_IsBurningEnergy)
            _Fullness -= Time.deltaTime * (_baseEnergyBurn + energyBurnRate) / 60;
    }

    public void SpawnBabies()
    {
        _timeSinceLastPregnancy = 0;
        Chicken newChick = Instantiate(_chickenPrefab, transform.position, Quaternion.identity).GetComponent<Chicken>();
        newChick._Fullness = 0.25f;
        //Inherit Stats from Parent and mutate them a bit
        newChick._DetectionRange = _DetectionRange + UnityEngine.Random.Range(-2f, 2f);
        newChick._MoveSpeed = _MoveSpeed + UnityEngine.Random.Range(-0.3f, 0.3f);
        _Fullness -= 0.3f;
    }

    public void PlaySexyParticle(float duration)
    {
        _ps.Play();
        Invoke("StopSexyParticle", duration);
    }

    public void StopSexyParticle()
    {
        _ps.Stop();
    }

    public void Die()
    {
        if (OnDeath != null)
            OnDeath();
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

    public bool _IsBurningEnergy
    {
        get => _isBurningEnergy;
        set => _isBurningEnergy = value;
    }

    public float _DetectionRange
    {
        get => LevelManager.startingSize;
        set => LevelManager.startingSize = value;
    }

    public float _TimeSinceLastPregnancy
    {
        get => _timeSinceLastPregnancy;
        set => _timeSinceLastPregnancy = value;
    }
    public float _MoveSpeed
    {
        get => LevelManager.startingSpeed;
        set
        {
            LevelManager.startingSpeed = value;
            GetComponent<NavMeshAgent>().speed = LevelManager.startingSpeed;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        Gizmos.DrawSphere(transform.position, _DetectionRange);
        Gizmos.color = new Color(1, 1, 1, 1);
    }
}
