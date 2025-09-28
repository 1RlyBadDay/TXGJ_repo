using UnityEngine;

public class GoToCardGameFade : MonoBehaviour
{
    public void LoadCardGame()
    {
        // Black fade over 2 seconds to the “cardgame” scene
        Initiate.Fade("cardgame", Color.black, 2.0f);
    }
}
