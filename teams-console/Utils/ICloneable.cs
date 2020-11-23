using System;

namespace teams_console.Utils
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
