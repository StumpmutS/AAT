using System.Collections.Generic;

public interface IActionCreator
{
    public List<UserAction> GetActions();
}