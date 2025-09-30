using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DiceRoll : MonoBehaviour, IPointerClickHandler
{
    [Header("Dice Faces (Sprites 1-6)")]
    public Sprite[] diceFaces;        // Drag your 6 dice sprites here

    [Header("UI Text")]
    public TMP_Text resultText;       // DiceValueText
    public TMP_Text turnText;         // PlayerTurnText

    private Image diceImage;          // Image component of this GameObject

    private void Awake()
    {
        // Get Image component automatically
        diceImage = GetComponent<Image>();
        if (diceImage == null)
            Debug.LogError("DiceRoll: No Image component found on this GameObject!");
    }

    private void Start()
    {
        UpdateTurnText();
    }

    // Called automatically when the image is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        RollDice();
    }

    private void RollDice()
    {
        if (GameManager.instance.canSelectToken)
        {
            Debug.Log("⚠️ Wait! Token not moved yet.");
            return;
        }

        int value = Random.Range(1, 7); // 1 to 6
        resultText.text = "Dice: " + value;

        // Update dice face
        if (diceFaces.Length == 6)
            diceImage.sprite = diceFaces[value - 1];
        else
            Debug.LogError("DiceRoll: Dice Faces array must have 6 sprites!");

        // Call GameManager dice roll logic
        GameManager.instance.DiceRolled(value);
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        string playerName = (GameManager.instance.currentPlayer == PlayerType.PlayerA) ? "Player A" : "Player B";
        turnText.text = $"{playerName}'s Turn";
    }
}
