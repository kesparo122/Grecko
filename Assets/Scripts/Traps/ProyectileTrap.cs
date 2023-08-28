using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProyectileTrap : MonoBehaviour, IActivable
{
     [SerializeField]
     private BulletPool _pool;

     [SerializeField]
     private Transform _shootPoint;

     [SerializeField]
     [Range(0f, 0.25f)]
     private float _bulletSpeed;

     [SerializeField]
     private float _shootTimeRemaining;

     [SerializeField]
     private bool _TrapIsActive = false;


     private float _timeRemainigInitial;

     private Animator _anim;

     private void Start()
     {
          _timeRemainigInitial = _shootTimeRemaining;

          _anim = GetComponent<Animator>();
     }

     private void Update()
     {
          if(_TrapIsActive)
          {
               if (_shootTimeRemaining < 0)
               {
                    Shoot(_shootPoint);
                    _shootTimeRemaining = _timeRemainigInitial;
               }
               else
               {
                    _shootTimeRemaining -= Time.deltaTime;
               }
          }
     }

     private void Shoot(Transform shootPoint)
     {
          // Triggerea la animacion si la hay
          if (_anim != null)
          {
               _anim.SetTrigger("Shoot");
          }

          GameObject bullet = _pool.RequestBullet();
          bullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
          if (bullet.transform.TryGetComponent(out Bullet bl))
          {
               bl.speed = _bulletSpeed;
          }   
     }

     public virtual void Activate()
     {
          _TrapIsActive = true;
     }

     public virtual void Deactivate()
     {
          _TrapIsActive = false;
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
          if (collision.CompareTag("Player")){
               Activate();
          } 
     }

     private void OnTriggerExit2D(Collider2D collision)
     {
          if (collision.CompareTag("Player")){
               Deactivate();
          }
     }
}