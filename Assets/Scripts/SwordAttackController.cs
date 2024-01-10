using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttackController : MonoBehaviour, IWeapon
{
    Input_Manager _playerInput;
    public List<SwordSO> Combo;
    float lastClickedTime;
    int comboCounter;
    [SerializeField] Animator _anim;
    float comboCooldown = 0.35f;
    float lastComboTime;

    float _damage;
    bool _isAttackPressed;
    string _weapnName; 
    public string WeaponName =>_weapnName;
    public bool ActivateWeapon(WeaponSwitch weaponSwitch)
    {
        return true;
    }
    private void Awake()
    {
        _playerInput = new Input_Manager();

        _playerInput.Player.Attack.started +=OnAttack;
        _playerInput.Player.Attack.canceled += OnAttack;
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        _isAttackPressed = ctx.ReadValueAsButton();
    }
    private void Update()
    {
        if (_isAttackPressed)
        {
            Attack();
        }

        //reset combo when too long no press
        if (Time.time - lastClickedTime >= 1f)
        {
            comboCounter = 0;
        }
    }

    void Attack()
    {
        if (Time.time - lastComboTime >= comboCooldown)
        {
            _anim.runtimeAnimatorController = Combo[comboCounter].animatorOV;
            if (comboCounter < 3)
            {
                _anim.Play("SwordAttack", 1, 0);
            }
            else
            {
                _anim.Play("SwordAttack", 0, 0);
            }
            _damage = Combo[comboCounter].damage;
            //add VFX
            comboCounter++;
            lastClickedTime = Time.time;
            if (comboCounter + 1 > Combo.Count)
            {
                comboCounter = 0;
            }

            // Record the time of the last combo
            lastComboTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //other.gameObject.GetComponent<EnemyHealth>().TakeDamage(_damage);
            Debug.Log("Attack Enemy , Damage = "+ _damage);
        }
    
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }

}
