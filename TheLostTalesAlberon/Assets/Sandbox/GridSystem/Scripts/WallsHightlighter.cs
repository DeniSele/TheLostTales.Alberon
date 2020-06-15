using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsHightlighter : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
