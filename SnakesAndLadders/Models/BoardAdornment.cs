using SnakesAndLadders.Common.Enums;
using SnakesAndLadders.Common.Extensions;

namespace SnakesAndLadders.Models
{
    public class BoardAdornment
    {
        public readonly int Start;
        
        public readonly int End;

        public readonly BoardAdornmentType Type;

        public BoardAdornment(int start, int end, BoardAdornmentType type)
        {
            Start = start;
            End = end;
            Type = type;
        }

        public bool StartsOn(int position)
        {
            return Start.Equals(position);
        }

        public string GetTypeDescription()
        {
            return Type.GetDescription();
        }
    }
}
