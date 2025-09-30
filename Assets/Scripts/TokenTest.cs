using UnityEngine;

public class TokenTest : MonoBehaviour
{
    public TokenMovement testToken; // Assign in Inspector
    public int stepsToMove = 4;

    void Start()
    {
        if (testToken != null)
        {
            testToken.MoveToken(stepsToMove);
        }
        else
        {
            Debug.LogWarning("No TokenMovement assigned!");
        }
    }
}
