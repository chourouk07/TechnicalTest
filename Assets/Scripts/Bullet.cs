using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private int _damage = 8;
    // Start is called before the first frame update
    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }
    private void Update()
    {
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            Debug.Log("Attack Enemy , Damage = " + _damage);
            other.gameObject.GetComponent<CharacterStats>().ChangeHealth(-_damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
    public void SetBulletDamage(int newValue)
    {
        _damage= +newValue;
    }
}
