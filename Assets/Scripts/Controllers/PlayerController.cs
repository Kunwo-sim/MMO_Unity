using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;

    Texture2D _attackIcon;
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }
    CursorType _cursorType = CursorType.None;
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    void UpdateDie()
    {
    }
    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    PlayerState _state = PlayerState.Idle;
    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green) ;
            if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    _state = PlayerState.Idle;
                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _stat.MoveSpeed);
    }
    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        UpdateMouseCursor();
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if(_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0.0f), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if(_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0.0f), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        switch(evt)
        {
            case Define.MouseEvent.PointDown:
                {
                    if(raycastHit)
                    {
                        _destPos = hit.point;
                        _state = PlayerState.Moving;
                    }
                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                    {
                        _lockTarget = hit.collider.gameObject;
                    }
                    else
                    {
                        _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if (raycastHit)
                    {
                            _destPos = hit.point;
                    }
                }
                break;
            case Define.MouseEvent.PointUp:
                _lockTarget = null;
                break;
        }
    }
}
