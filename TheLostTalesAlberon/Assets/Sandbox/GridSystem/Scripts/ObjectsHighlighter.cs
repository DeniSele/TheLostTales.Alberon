using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsHighlighter : MonoBehaviour
{
    [Header ("Highlight settings")]

    [SerializeField] private Color highlightColor;

    [Range(0,1)]
    [SerializeField] private float opacity = 0.3f;
    private void OnDrawGizmos()
    {
        highlightColor.a = 1f;
        Gizmos.color = highlightColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        highlightColor.a = opacity;
        Gizmos.color = highlightColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
