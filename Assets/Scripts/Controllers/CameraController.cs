using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CamerMode _mode = Define.CamerMode.QuaterView;

    [SerializeField]
    public Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);
    public GameObject _player = null;
    void Start()
    {
    }

    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
        {
            float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
            transform.position = _player.transform.position + _delta.normalized * dist;
        }
        else
        {
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CamerMode.QuaterView;
        _delta = delta;
    }
}
