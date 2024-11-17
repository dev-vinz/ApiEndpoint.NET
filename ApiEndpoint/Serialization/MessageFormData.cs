namespace ApiEndpoint.Serialization
{
    /// <summary>
    /// Represents a form data for a message.
    /// </summary>
    public class MessageFormData : MultipartFormDataContent
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Adds a field to the form data.
        /// </summary>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <returns>The form data.</returns>
        public MessageFormData AddField(string key, string value)
        {
            Add(new StringContent(value), key);
            return this;
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="data">The data of the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The form data.</returns>
        public MessageFormData AddFile(string key, byte[] data, string fileName)
        {
            Add(new ByteArrayContent(data), key, fileName);
            return this;
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="stream">The stream of the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The form data.</returns>
        public MessageFormData AddFile(string key, Stream stream, string fileName)
        {
            Add(new StreamContent(stream), key, fileName);
            return this;
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="fileInfo">The file information of the file.</param>
        /// <returns>The form data.</returns>
        public MessageFormData AddFile(string key, FileInfo fileInfo)
        {
            Add(new StreamContent(fileInfo.OpenRead()), key, fileInfo.Name);
            return this;
        }
    }
}
