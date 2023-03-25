using System.Collections.Generic;

public interface IGameActionInfoGetter
{
    public IEnumerable<GameActionInfo> GetInfo();
}