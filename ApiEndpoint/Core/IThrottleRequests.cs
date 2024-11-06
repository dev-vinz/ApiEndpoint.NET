namespace ApiEndpoint.Core
{
    /// <summary>
    /// Represents a request that can be throttled.
    /// </summary>
    internal interface IThrottleRequests
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Waits for the throttle to be lifted.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task WaitAsync();
    }
}
