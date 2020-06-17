using System.Collections;
using System.Collections.Generic;

public interface IDisplayable<in T>
{
    void Display(T arg);
}
