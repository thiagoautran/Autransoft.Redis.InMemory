using System.Collections.Generic;

namespace Autransoft.Redis.InMemory.Lib.Entities
{
    public class ValueEntity
    {
        public object Obj { get; set; }
        public HashSet<string> Tag { get; set; }
    }
}