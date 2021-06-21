using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform healthBar = null;
        [SerializeField] Health health = null;
        [SerializeField] Canvas healthBarCanvas = null;

        private void Awake()
        {
            healthBarCanvas.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            health.restoreHealthBar += UpdateHealth;
        }

        private void OnDisable()
        {
            health.restoreHealthBar -= UpdateHealth;
        }

        public void UpdateHealth()
        {
            if (healthBarCanvas.gameObject.activeSelf == false) { healthBarCanvas.gameObject.SetActive(true); }

            float healthFraction = health.GetPercentage();
            if (Mathf.Approximately(healthFraction, 0) || Mathf.Approximately(healthFraction, 1)) { healthBarCanvas.gameObject.SetActive(false); }

            Vector3 healthFactor = new Vector3(healthFraction, 1, 1);
            healthBar.localScale = healthFactor;
        }
    }
}
