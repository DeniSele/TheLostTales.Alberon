using System.Collections;
using System.Collections.Generic;

public interface IUploader<out T>
{
    T Updload();
}
