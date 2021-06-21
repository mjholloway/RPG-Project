using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health health = fighter.GetTarget();
            if (health != null)
            {
                GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealth());
            }
            else
            {
                GetComponent<Text>().text = string.Format("N/A");
            }
        }
    }
}
