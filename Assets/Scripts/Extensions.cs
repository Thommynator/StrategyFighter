using UnityEngine;
public static class Extension
{
    public static int ManhattenDistanceTo(this Vector3 from, Vector3 to)
    {
        return (int)Mathf.Abs(to.x - from.x) + (int)Mathf.Abs(to.y - from.y);
    }

    public static Player Opposite(this Player player)
    {
        return player == Player.PLAYER1 ? Player.PLAYER2 : Player.PLAYER1;
    }
}
