using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsBoard : MonoBehaviour
{
    [SerializeField] private GameObject faintInHospital;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("FAINT IN HOSPITAL", 0) == 1) faintInHospital.SetActive(false); else faintInHospital.SetActive(true);
    }
}
