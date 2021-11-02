using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrKillSquare : MonoBehaviour
{
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

}
