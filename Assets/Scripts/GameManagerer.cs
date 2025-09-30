using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int diceValue = 0;
    public bool canSelectToken = false;

    public PlayerType currentPlayer = PlayerType.PlayerA;

    private void Awake()
    {
        instance = this;
    }

    public void DiceRolled(int value)
    {
        diceValue = value;
        canSelectToken = true;
        Debug.Log($"Dice rolled: {value} | Current Player: {currentPlayer}");
    }

    public void SelectToken(TokenMovement token)
    {
        if (canSelectToken && token.owner == currentPlayer)
        {
            token.MoveToken(diceValue);
        }
    }

    public void EndTurn()
    {
        Debug.Log("Turn Ended for " + currentPlayer);
        currentPlayer = (currentPlayer == PlayerType.PlayerA) ? PlayerType.PlayerB : PlayerType.PlayerA;
        Debug.Log("Now it's " + currentPlayer + "'s turn");
    }
}
