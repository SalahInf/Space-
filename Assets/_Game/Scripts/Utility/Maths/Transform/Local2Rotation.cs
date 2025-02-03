using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility.TweenScaleFunctions;

public class Local2Rotation : MonoBehaviour
{
    public Vector3 rotation1;
    public Vector3 rotation2;
    [Space]
    public float speed1 = 1;
    public float speed2 = 1;
    [Space]
    public TypeScale typeScaleRotation1;
    public TypeScale typeScaleRotation2;
    [Space]
    public float wait1 = 0.1f;
    public float wait2 = 0.1f;
    [Space]
    private Quaternion _targetRotation1;
    private Quaternion _targetRotation2;

    private System.Func<float, float> _tweenScaleFunctions1;
    private System.Func<float, float> _tweenScaleFunctions2;

    float _timer;
    float t;

    bool _move1;

    Renderer _renderer;

    private void Start()
    {
        _targetRotation1 = Quaternion.Euler(rotation1);
        _targetRotation2 = Quaternion.Euler(rotation2);

        _tweenScaleFunctions1 = GetTweenScaleFunctions(typeScaleRotation1, TypeScaleMode.In);
        _tweenScaleFunctions2 = GetTweenScaleFunctions(typeScaleRotation2, TypeScaleMode.In);

        _move1 = (Random.value > 0.5f);
        _timer = Random.value;

        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        if (!_renderer.isVisible)
            return;

        if (_move1)
        {
            _timer += Time.deltaTime * speed1;
            t = _tweenScaleFunctions1(_timer);
            transform.localRotation = Quaternion.Lerp(_targetRotation1, _targetRotation2, t);
            if (_timer > 1 + wait2)
            {
                _move1 = false;
                _timer = 0;
            }
        }
        else
        {
            _timer += Time.deltaTime*speed2;
            t = _tweenScaleFunctions2(_timer);
            transform.localRotation = Quaternion.Lerp(_targetRotation2, _targetRotation1, t);
            if(_timer > 1 + wait1)
            {
                _move1 = true;
                _timer = 0;
            }
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            _tweenScaleFunctions1 = GetTweenScaleFunctions(typeScaleRotation1, TypeScaleMode.In);
            _tweenScaleFunctions2 = GetTweenScaleFunctions(typeScaleRotation2, TypeScaleMode.In);
        }
    }
}
