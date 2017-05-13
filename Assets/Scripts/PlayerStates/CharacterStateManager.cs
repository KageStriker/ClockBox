using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IKController))]
public class CharacterStateManager : MonoBehaviour
{
    PlayerState _State;

    public RectTransform healthBar;
    const int maxHealth = 100;
    int currentHealth = maxHealth;
    
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _State = new GroundedState(gameObject);
        _State.UpdateIK();
        Debug.Log(_State);
    }
	
	void FixedUpdate ()
    {
        PlayerState State = _State.UpdateState();
        if (State != null)
        {
            _State = State;
            Debug.Log(_State);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerState State = _State.OnTriggerEnter(other);
        if (State != null)
        {
            _State = State;
            Debug.Log(_State);
        }
    }

    void OnTriggerStay(Collider other)
    {
        PlayerState State = _State.OnTriggerStay(other);
        if (State != null)
        {
            _State = State;
            Debug.Log(_State);
        }
    }
}
