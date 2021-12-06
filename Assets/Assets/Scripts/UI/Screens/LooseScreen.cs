using GlobalEventAggregator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LooseScreen : BaseScreen
{
    [SerializeField] Button restartBtn;
    [SerializeField] TextMeshProUGUI levelText;


    private void OnEnable()
    {
        restartBtn.onClick.AddListener(RestartLevel_Click);
    }

    private void OnDisable()
    {
        restartBtn.onClick.RemoveListener(RestartLevel_Click);
    }


    private void RestartLevel_Click()
    {
        EventAggregator.Invoke(new OnRestartLevel());
    }
}
