using UnityEngine;

public class GoToCardGameFade : MonoBehaviour
{
    public void LoadCardGame()
    {
        // Black fade over 2 seconds to the “cardgame” scene
        Initiate.Fade("cardgame", Color.black, 2.0f);
    }

    public void LoadMM()
    {
        // Black fade over 2 seconds to the MM scene
        Initiate.Fade("MainMenuScene", Color.black, 2.0f);
    }

    public void LoadLimbo()
    {
        // Black fade over 2 seconds to the limbo scene
        Initiate.Fade("LimboScene", Color.black, 2.0f);
    }
}
