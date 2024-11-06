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
        public void AddField(string key, string value)
        {
            Add(new StringContent(value), key);
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="data">The data of the file.</param>
        /// <param name="fileName">The name of the file.</param>
        public void AddFile(string key, byte[] data, string fileName)
        {
            Add(new ByteArrayContent(data), key, fileName);
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="stream">The stream of the file.</param>
        /// <param name="fileName">The name of the file.</param>
        public void AddFile(string key, Stream stream, string fileName)
        {
            Add(new StreamContent(stream), key, fileName);
        }

        /// <summary>
        /// Adds a file to the form data.
        /// </summary>
        /// <param name="key">The key of the file.</param>
        /// <param name="fileInfo">The file information of the file.</param>
        public void AddFile(string key, FileInfo fileInfo)
        {
            Add(new StreamContent(fileInfo.OpenRead()), key, fileInfo.Name);
        }
    }
}
