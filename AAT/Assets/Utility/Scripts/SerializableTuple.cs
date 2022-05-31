using System;

namespace Utility.Scripts
{
    [Serializable]
    public class SerializableTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;
    
        public SerializableTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        //doesnt follow hashcode rule
        public override bool Equals(object obj)
        {
            if (obj is Tuple<T1, T2> tuple)
            {
                return Item1.Equals(tuple.Item1) && Item2.Equals(tuple.Item2);
            }

            if (!(obj is SerializableTuple<T1, T2> sTuple)) return false;
            return Item1.Equals(sTuple.Item1) && Item2.Equals(sTuple.Item2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
