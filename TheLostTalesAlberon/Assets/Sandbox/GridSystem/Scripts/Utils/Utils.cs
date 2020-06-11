using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        // Handle equality as beeing greater
        if (result == 0)
            return 1;
        else
            return result;
    }

    #endregion
}
