using System;

namespace Cornerstone
{
    public interface IExceptionHandler
    {
        void Handle(Exception exception);
    }
}
