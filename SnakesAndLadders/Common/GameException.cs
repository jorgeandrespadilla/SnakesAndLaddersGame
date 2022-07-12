using System.Runtime.Serialization;

namespace SnakesAndLadders.Common
{
    /// <summary>
    /// Custom game exception
    /// </summary>
    [Serializable]
    public class GameException : Exception
    {
        public GameException() 
        {
        }

        public GameException(string message): base(message) 
        {
        }

        protected GameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
