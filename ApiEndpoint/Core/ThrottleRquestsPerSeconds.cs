namespace ApiEndpoint.Core
{
    internal sealed class ThrottleRquestsPerSeconds : IThrottleRequests
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly long _delayBetweenCalls;
        private readonly object _lock = new();

        private DateTimeOffset _nextAllowedCall;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public ThrottleRquestsPerSeconds(uint requestsPerSecond, uint nbClients)
        {
            // Inputs
            {
                _delayBetweenCalls = TimeSpan.TicksPerSecond / (requestsPerSecond * nbClients);
            }

            // Tools
            {
                _nextAllowedCall = DateTimeOffset.UtcNow;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public Task WaitAsync()
        {
            DateTimeOffset nextAllowedCall = GetAndUpdateNextAllowedCall();
            TimeSpan delay = nextAllowedCall - DateTimeOffset.UtcNow;

            if (delay > TimeSpan.Zero)
            {
                return Task.Delay(delay);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                          PRIVATE METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private DateTimeOffset GetAndUpdateNextAllowedCall()
        {
            lock (_lock)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                DateTimeOffset nextAllowedCall = _nextAllowedCall;

                if (nextAllowedCall < now)
                {
                    nextAllowedCall = now;
                }

                _nextAllowedCall = nextAllowedCall.AddTicks(_delayBetweenCalls);

                return nextAllowedCall;
            }
        }
    }
}
