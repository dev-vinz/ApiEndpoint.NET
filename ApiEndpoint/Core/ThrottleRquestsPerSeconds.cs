namespace ApiEndpoint.Core
{
    internal sealed class ThrottleRquestsPerSeconds : IThrottleRequests
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly long _delayBetweenCalls;
        private readonly object _lock = new();

        private long _nextAllowedTicks;

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
                _nextAllowedTicks = DateTimeOffset.UtcNow.Ticks;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public Task WaitAsync()
        {
            TimeSpan delay;

            lock (_lock)
            {
                long currentTicks = DateTimeOffset.UtcNow.Ticks;

                if (_nextAllowedTicks <= currentTicks)
                {
                    // If no waiting is needed, move the next allowed time forward
                    _nextAllowedTicks = currentTicks + _delayBetweenCalls;
                    delay = TimeSpan.Zero;
                }
                else
                {
                    // Calculate the required delay and update the next allowed time
                    delay = TimeSpan.FromTicks(_nextAllowedTicks - currentTicks);
                    _nextAllowedTicks += _delayBetweenCalls;
                }
            }

            // Wait if delay is required
            return delay > TimeSpan.Zero ? Task.Delay(delay) : Task.CompletedTask;
        }
    }
}
