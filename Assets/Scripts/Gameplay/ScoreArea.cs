using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public Side side;

    private void Start()
    {
        if (side == Side.UNSET)
            throw new UnassignedReferenceException("Score Area side unset");
    }
}
