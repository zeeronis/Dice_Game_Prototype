using GlobalEventAggregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : BaseScreen
{
    [SerializeField] Button playButton;


    private void OnEnable()
    {
        playButton.onClick.AddListener(Play_Click);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(Play_Click);
    }


    private void Play_Click()
    {
        EventAggregator.Invoke(new OnLevelStart());
    }
}
