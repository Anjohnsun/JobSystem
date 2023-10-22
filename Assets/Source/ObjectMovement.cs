using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _speed;
    [SerializeField] private int _objectCount;
    [SerializeField] private float _logFrequancy;
    private float _timeToLog;


    private Transform[] _objectsOnScene;
    private TransformAccessArray _transformsOnScene;
    private MovementJob _movementJob;
    private LogJob _logJob;
    private JobHandle _movementJobHandle;
    private JobHandle _logJobHandle;

    private void Start()
    {
        _objectsOnScene = new Transform[_objectCount];

        for (int i = 0; i < _objectCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            GameObject instance = Instantiate(_prefab, position, Quaternion.identity);
            _objectsOnScene[i] = instance.transform;
        }

        _transformsOnScene = new TransformAccessArray(_objectsOnScene);

        _timeToLog = 0;
    }

    private void Update()
    {
        _movementJob = new MovementJob()
        {
            Speed = _speed,
            DeltaTime = Time.deltaTime
        };
        _logJob = new LogJob() { Number = Random.Range(1, 100) };

        _movementJobHandle = _movementJob.Schedule(_transformsOnScene);

        _timeToLog += Time.deltaTime;
        if (_timeToLog >= _logFrequancy)
        {
            _timeToLog = 0;
            foreach (Transform obj in _objectsOnScene)
                _logJob.Schedule();
        }
    }

    private void LateUpdate()
    {
        _movementJobHandle.Complete();
    }

    private void OnDestroy()
    {
        _transformsOnScene.Dispose();
    }

}
