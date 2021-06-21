using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnImpact = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;

        Health target;
        GameObject instigator = null;
        float damage = 0f;


        void Update()
        {
            if (target == null) { return; }
            if (isHoming && !target.IsDead())
            {
                SetDirection();
            }
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }

        private void SetDirection()
        {
            transform.LookAt(GetAimLocation());
            
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            SetDirection();
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) { return target.transform.position; }
            return target.transform.position + Vector3.up * targetCapsule.center.y;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target || target.IsDead()) { return; }
            target.TakeDamage(instigator, damage);
            moveSpeed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnImpact)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }


    }
}
