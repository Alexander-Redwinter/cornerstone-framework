using System;

namespace Cornerstone
{
    public class BaseExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            //TODO Localization
            Framework.Logger.LogCriticalSource("Unhandled Exception", exception: exception);
        }
    }
}
