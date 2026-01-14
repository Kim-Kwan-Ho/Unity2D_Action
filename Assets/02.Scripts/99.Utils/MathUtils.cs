using UnityEngine;

public static class MathUtils
{
    public static Vector2 GetRandomVector2(Vector2 min, Vector2 max)
    {
        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        return new Vector2(randomX, randomY);
    }
}
