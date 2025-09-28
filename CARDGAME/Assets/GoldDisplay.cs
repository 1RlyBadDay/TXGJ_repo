using TMPro;
using UnityEngine;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        if (GlobalGameState.Instance)
            goldText.text = $"GOLD: {GlobalGameState.Instance.Gold}";
    }
}
