using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player/Respawn Variables")]
public class RespawnPointVariables : ScriptableObject
{
    [SerializeField] Vector2 Position;
    public NewRespawnPoint ActualRespawnPoint;

    public void SetRespawnPoint(NewRespawnPoint respawnPoint)
    {
        if(ActualRespawnPoint != null)ActualRespawnPoint.ResetRespawn();
        ActualRespawnPoint = respawnPoint;
        SetPosition(ActualRespawnPoint.transform.position);
    }

    public void ResetPoint()
    {
        ActualRespawnPoint = null;
        SetPosition(Vector2.zero);
    }

    public Vector2 GetPosition() { return Position; }
    public void SetPosition(Vector2 position) {  Position = position; }
}