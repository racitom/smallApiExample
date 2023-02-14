using smallApiExample.Interfaces;

namespace smallApiExample.Core
{
    public class IdGenerator : IIdGenerator
    {
        public int Get()
        {
            Random rnd = new();
            return rnd.Next(1, int.MaxValue);
        }
    }
}
