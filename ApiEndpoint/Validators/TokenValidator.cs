namespace ApiEndpoint.Validators
{
    internal static class TokenValidator
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public static void Validate(params string[] tokens)
        {
            if (tokens.Length == 0)
            {
                throw new ArgumentException("At least one API token is required.");
            }

            foreach (string token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentException("API token mustn't be null or empty.");
                }
            }
        }
    }
}
