using GlobalEventAggregator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    [SerializeField] Button nextLevelBtn;
    [SerializeField] TextMeshProUGUI levelText;


    private void OnEnable()
    {
        nextLevelBtn.onClick.AddListener(NextLevel_Click);
    }

    private void OnDisable()
    {
        nextLevelBtn.onClick.RemoveListener(NextLevel_Click);
    }


    private void NextLevel_Click()
    {
        EventAggregator.Invoke(new OnNextLevel());
    }
}
