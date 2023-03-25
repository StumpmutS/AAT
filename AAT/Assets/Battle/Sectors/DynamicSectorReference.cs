public class DynamicSectorReference : SectorReference
{
    public override void FixedUpdateNetwork()
    {
        var posY0 = transform.position;
        posY0.y = 0;
        var found = FindSector();
        if (found != null) Sector = found;
    }
}