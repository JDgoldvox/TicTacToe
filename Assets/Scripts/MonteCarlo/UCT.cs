using UnityEngine;

public static class UCT
{
    public static float Calculate(float winRatio, int parentVisits, int thisNodeVisits, float constant)
    {
        return winRatio + constant * Mathf.Sqrt(Mathf.Log(thisNodeVisits) / parentVisits);
    }
}

