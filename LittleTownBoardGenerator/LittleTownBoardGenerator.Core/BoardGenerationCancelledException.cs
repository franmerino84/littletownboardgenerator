using System.Runtime.Serialization;

namespace LittleTownBoardGenerator.Core
{
    [Serializable]
    public class BoardGenerationCancelledException : Exception
    {
        public BoardGenerationCancelledException()
        {
        }

        public BoardGenerationCancelledException(string? message) : base(message)
        {
        }

        public BoardGenerationCancelledException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoardGenerationCancelledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}