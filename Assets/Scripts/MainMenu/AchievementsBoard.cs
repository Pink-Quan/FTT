using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsBoard : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenuManager;

    [SerializeField] private TMPro.TMP_Text faintInHospital;
    [SerializeField] private TMPro.TMP_Text diedAtCampsite;
    [SerializeField] private TMPro.TMP_Text arrested;
    [SerializeField] private TMPro.TMP_Text foundTheTruth;

    [SerializeField] private Color lockColor;
    [SerializeField] private Color unlockColor;

    private void OnEnable()
    {
        SetText();
        if (PlayerPrefs.GetInt("FAINT IN HOSPITAL", 0) == 1) faintInHospital.color = unlockColor; else faintInHospital.color = lockColor;
        if (PlayerPrefs.GetInt("Died at campsite", 0) == 1) diedAtCampsite.color = unlockColor; else diedAtCampsite.color = lockColor;
        if (PlayerPrefs.GetInt("Arrested in hospital", 0) == 1) arrested.color = unlockColor; else arrested.color = lockColor;
        if (PlayerPrefs.GetInt("Found the truth", 0) == 1) foundTheTruth.color = unlockColor; else foundTheTruth.color = lockColor;
    }

    private void SetText()
    {
        faintInHospital.text = mainMenuManager.texts.faintInHospital;
        diedAtCampsite.text = mainMenuManager.texts.diedAtCampsite;
        arrested.text = mainMenuManager.texts.arrested;
        foundTheTruth.text = mainMenuManager.texts.foundTheTruth;
    }
}
