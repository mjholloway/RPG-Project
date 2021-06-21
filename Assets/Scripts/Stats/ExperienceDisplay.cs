using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Text textBox;
        Experience experience;

        void Awake()
        {
            textBox = GetComponent<Text>();
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            textBox.text = string.Format("{0:0}", experience.GetExperience());
        }
    }
}
