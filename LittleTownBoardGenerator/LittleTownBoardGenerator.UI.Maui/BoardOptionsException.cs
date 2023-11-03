using System.Runtime.Serialization;

namespace LittleTownBoardGenerator.UI.Maui
{
    [Serializable]
    internal class BoardOptionsException : Exception
    {
        public BoardOptionsException()
        {
        }

        public BoardOptionsException(string message) : base(message)
        {
        }

        public BoardOptionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoardOptionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}