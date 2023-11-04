using System.Runtime.Serialization;

namespace LittleTownBoardGenerator.UI.Maui
{
    [Serializable]
    internal class BoardGeneralConfigurationException : Exception
    {
        public BoardGeneralConfigurationException()
        {
        }

        public BoardGeneralConfigurationException(string message) : base(message)
        {
        }

        public BoardGeneralConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoardGeneralConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}