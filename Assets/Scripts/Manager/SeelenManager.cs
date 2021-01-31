using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeelenManager : MonoBehaviour
{
    public static SeelenManager instance;

    [Header("Refs")]
    [SerializeField] TextMeshProUGUI soulText; 

    [Header("Values")]
    [SerializeField] int maxSoulParts = 4;
    int soulParts = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        UpdateSoulText();
    }

    public void AddSoul()
    {
        soulParts++;
        UpdateSoulText();
    }

    private void UpdateSoulText()
    {
        soulText.text = soulParts.ToString() + " / " + maxSoulParts.ToString();
    }
}
