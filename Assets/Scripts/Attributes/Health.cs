using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        public UnityAction restoreHealthBar;

        LazyValue<float> health;
        BaseStats stats;
        bool isDead = false;

        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return stats.GetStat(Stat.Health);
        }

        private void Start()
        {
            health.ForceInit();
        }

        private void OnEnable()
        {
            stats.OnLevelUpEvent += LevelUpHealth;
        }

        private void OnDisable()
        {
            stats.OnLevelUpEvent -= LevelUpHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            health.value = Mathf.Max(health.value - damage, 0);

            takeDamage.Invoke(damage);

            if (health.value == 0 && !isDead)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float amountToHeal)
        {
            health.value = Mathf.Min(health.value + amountToHeal, GetMaxHealth());
        }

        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxHealth()
        {
            return stats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return health.value / stats.GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void LevelUpHealth()
        {
            health.value = stats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;
            if (health.value == 0 && !isDead)
            {
                Die();
            }
            if (health.value != 0 && isDead)
            {
                isDead = false;
                GetComponent<Animator>().SetTrigger("unDie");
            }
            if (restoreHealthBar != null)
            {
                restoreHealthBar.Invoke();
            }
        }
    }
}