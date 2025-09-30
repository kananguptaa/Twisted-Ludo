using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;

    public List<TokenMovement> playerAFinished = new List<TokenMovement>();
    public List<TokenMovement> playerBFinished = new List<TokenMovement>();

    [Header("UI")]
    public TMP_Text winMessageText;  // Drag TMP UI Text here in Inspector

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Make sure win text is hidden at start
        if (winMessageText != null)
            winMessageText.gameObject.SetActive(false);
    }

    public void CheckTokenReachedEnd(TokenMovement token)
    {
        if (token.owner == PlayerType.PlayerA && !playerAFinished.Contains(token))
        {
            playerAFinished.Add(token);
            Debug.Log("✅ Player A token finished. Total: " + playerAFinished.Count);
        }
        else if (token.owner == PlayerType.PlayerB && !playerBFinished.Contains(token))
        {
            playerBFinished.Add(token);
            Debug.Log("✅ Player B token finished. Total: " + playerBFinished.Count);
        }

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (playerAFinished.Count >= 2)
        {
            DeclareWinner(PlayerType.PlayerA);
        }
        else if (playerBFinished.Count >= 2)
        {
            DeclareWinner(PlayerType.PlayerB);
        }
    }

    private void DeclareWinner(PlayerType winner)
    {
        string playerName = (winner == PlayerType.PlayerA) ? "Player A" : "Player B";

        Debug.Log($" {playerName} has WON the game!");

        // Show bold message on screen
        if (winMessageText != null)
        {
            winMessageText.text = $" YAYYY {playerName} WON! ";
            winMessageText.gameObject.SetActive(true);
        }

        // Stop gameplay
        GameManager.instance.canSelectToken = false;
        Time.timeScale = 0f;
    }
}
