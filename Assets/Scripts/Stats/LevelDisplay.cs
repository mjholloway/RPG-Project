using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        Text textBox;
        BaseStats stats;

        void Awake()
        {
            textBox = GetComponent<Text>();
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            textBox.text = string.Format("{0:0}", stats.GetLevel());
        }
    }
}
