using UnityEngine;

public class UIManager : SingletonMonoBehavior<UIManager>
{
    [Header("For set data inside")]
    [SerializeField] StartScreen startScreen = default;
    [SerializeField] GameScreen gameScreen = default;
    [SerializeField] WinScreen winScreen = default;
    [SerializeField] LooseScreen looseScreen = default;
    
    [Header("Switching screens")]
    [SerializeField] BaseScreen[] screensList = default;


    public StartScreen StartScreen => startScreen;
    public GameScreen GameScreen => gameScreen;
    public WinScreen WinScreen => winScreen;
    public LooseScreen LooseScreen => looseScreen;


    public void ActivScreen(Screens screen)
    {
        for (int i = 0; i < screensList.Length; i++)
        {
            if (screensList[i] != null)
                screensList[i].gameObject.SetActive((int)screen == i);
        }
    }
}

[System.Serializable]
public enum Screens
{
    Start,
    Game,
    Win,
    Loose,
}
