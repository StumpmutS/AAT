using System.Collections.Generic;
using System.Linq;

public static class RestrictionHelper
{
    public static bool CheckRestrictions(IEnumerable<Restriction> restrictions, UnitController unit)
    {
        return restrictions.All(restriction => restriction.CheckRestriction(unit));
    }
}