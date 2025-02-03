using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility.TweenScaleFunctions;

public class LocalPointsMove : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3> { new Vector3(-6, 0, 0), new Vector3(6, 0, 0) };
    public bool isClosed;

    public bool lockX, lockY, lockZ = true;

    [Space]
    public float speedMove;
    [Space]
    public bool rotation;
    public float rotationDamping;
    [Space]
    public TypeScaleFunctions typeScaleMovement;

    private Vector3 _starPosition;
    private Quaternion _starRotation;
    private Vector3 _fromPosition;
    private Vector3 _toPosition;

    private Quaternion _targetRotation;

    private int _indexMove;

    private float _timer;
    private float _duration;

    private System.Func<float, float> _tweenScaleFunctions;

    public Vector3 this[int i]
    {
        get
        {
            if (Application.isPlaying)
                return _starPosition + _starRotation * points[i];
            return transform.position + transform.rotation * points[i];
        }

        private set => points[i] = value;
    }

    public int numPoints => points.Count;

    Renderer _renderer;

    void Start()
    {
        _starPosition = transform.position;
        _starRotation = transform.rotation;

        _tweenScaleFunctions = GetTweenScaleFunctions(typeScaleMovement);
        SwitchLine();

        _renderer = GetComponentInChildren<Renderer>();
    }

    public void AddSegment(Vector3 pos)
    {
        pos = Quaternion.Inverse(transform.rotation) * (pos - transform.position);
        if (lockX)
            pos.x = 0;
        if (lockY)
            pos.y = 0;
        if (lockZ)
            pos.z = 0;
        points.Add(pos);
    }

    public void SplitSegment(Vector3 pos, int segmentIndex)
    {
        pos = Quaternion.Inverse(transform.rotation) * (pos - transform.position);
        if (lockX)
            pos.x = 0;
        if (lockY)
            pos.y = 0;
        if (lockZ)
            pos.z = 0;
        points.Insert(segmentIndex, pos);
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (numPoints > 2)
        {
            points.RemoveAt(anchorIndex);
        }
    }

    public void MovePoint(int i, Vector3 pos)
    {
        pos = Quaternion.Inverse(transform.rotation) * (pos - transform.position);
        if (lockX)
            pos.x = 0;
        if (lockY)
            pos.y = 0;
        if (lockZ)
            pos.z = 0;
        points[i] = pos;
    }

    public int LoopIndex(int i)
    {
        if (isClosed)
            return (i + points.Count) % points.Count;
        return (int)Mathf.PingPong(i, points.Count - 1);
    }

    private void Update()
    {
        if (!_renderer.isVisible)
            return;

        _timer += Time.deltaTime;

        float t = _tweenScaleFunctions(_timer / _duration);
        transform.position = Vector3.Lerp(_fromPosition, _toPosition, t);

        if (_timer > _duration)
        {
            SwitchLine();
        }

        if (rotation)
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, rotationDamping * Time.deltaTime);
    }

    void SwitchLine()
    {
        _fromPosition = this[LoopIndex(_indexMove)];
        _toPosition = this[LoopIndex(_indexMove + 1)];

        _timer = 0;
        _duration = Vector3.Distance(_fromPosition, _toPosition) / speedMove;

        _targetRotation = Quaternion.LookRotation(_toPosition - _fromPosition);

        _indexMove++;
    }
}
