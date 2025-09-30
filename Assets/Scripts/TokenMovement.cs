using UnityEngine;

public class TokenMovement : MonoBehaviour
{
    public WaypointManager waypointManager;
    public PlayerType owner; // Set in Inspector (PlayerA or PlayerB)

    public enum TokenType { Grass, Snake }
    public TokenType tokenType; // Set in Inspector

    public Transform homePosition; // Assign WaypointA_0 or WaypointB_0
    public int currentIndex = 0;

    private bool isMoving = false;
    private bool moveInReverseNextTurn = false;

    // 🎯 Your danger tile indexes
    private int[] dangerZonesA = { 4, 9, 16, 19, 34, 48, 55, 60 };
    private int[] dangerZonesB = { 1, 6, 10, 28, 38, 41, 47, 58 };

    private const float sameTileEpsilon = 0.05f; // tolerance for same tile check

    private void OnMouseDown()
    {
        if (GameManager.instance.canSelectToken &&
            GameManager.instance.currentPlayer == owner &&
            !isMoving)
        {
            GameManager.instance.SelectToken(this);
        }
    }

    public void MoveToken(int steps)
    {
        if (!isMoving)
        {
            StartCoroutine(Move(steps));
        }
    }

    private System.Collections.IEnumerator Move(int steps)
    {
        isMoving = true;

        int stepDirection = moveInReverseNextTurn ? -1 : 1;
        moveInReverseNextTurn = false;

        for (int i = 0; i < steps; i++)
        {
            currentIndex += stepDirection;
            currentIndex = Mathf.Clamp(currentIndex, 0, waypointManager.waypoints.Count - 1);

            Transform target = waypointManager.waypoints[currentIndex];

            while (Vector3.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }

        // ✅ Danger tile check
        if (IsDangerZone(currentIndex))
        {
            moveInReverseNextTurn = true;
            Debug.Log($"⚠️ {gameObject.name} landed on a danger tile! Will move in reverse next turn.");
        }

        // ✅ Win detection
        if (currentIndex == waypointManager.waypoints.Count - 1)
        {
            Debug.Log($"🏁 {gameObject.name} reached final tile!");
            Destroy(gameObject);
            WinManager.instance.CheckTokenReachedEnd(this);
        }

        // ✅ Cutting check
        CheckAndResolveCut();

        isMoving = false;
        GameManager.instance.canSelectToken = false;
        GameManager.instance.EndTurn();
    }

    private bool IsDangerZone(int index)
    {
        int[] danger = owner == PlayerType.PlayerA ? dangerZonesA : dangerZonesB;
        foreach (int dz in danger)
        {
            if (index == dz) return true;
        }
        return false;
    }

    public void SendToHome()
    {
        if (homePosition != null)
        {
            Debug.Log($"🏠 {gameObject.name} sent to home.");
            transform.position = homePosition.position;
            currentIndex = 0;
        }
    }

    // 🔥 NEW CUTTING RULE
    private void CheckAndResolveCut()
    {
        GameObject[] allTokens = GameObject.FindGameObjectsWithTag("Token"); // Make sure all tokens are tagged "Token"

        foreach (GameObject go in allTokens)
        {
            if (go == this.gameObject) continue; // skip self

            TokenMovement other = go.GetComponent<TokenMovement>();
            if (other == null) continue;
            if (other.owner == this.owner) continue; // only cut opponent tokens

            // Same tile check (either same index on same board, or nearly same position in world space)
            bool sameIndex = (other.waypointManager == this.waypointManager && other.currentIndex == this.currentIndex);
            bool samePosition = Vector3.Distance(other.transform.position, this.transform.position) <= sameTileEpsilon;

            if (sameIndex || samePosition)
            {
                Debug.Log($"✂ Cut! {gameObject.name} ({owner}) collided with {other.gameObject.name} ({other.owner}). Both return to origin.");
                this.SendToHome();
                other.SendToHome();
                return; // stop after handling one collision
            }
        }
    }
}
