using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : BaseScreen
{
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetStartLevelInfo(int levelNum)
    {
        levelText.text = (levelNum + 1).ToString();
    }
}
