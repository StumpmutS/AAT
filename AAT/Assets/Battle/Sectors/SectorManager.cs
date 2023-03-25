using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SectorManager : MonoBehaviour
{
    private Dictionary<SectorController, List<SectorController>> _sectorConnections = new();
    private Dictionary<(SectorController, SectorController), (float, TeleportPoint)> _paths = new();

    private List<List<SectorController>> _connectedSectorGroups = new();
    
    public static SectorManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public float PathBetween(Vector3 pos, float speed, SectorController from, SectorController target, out List<TeleportPoint> points)
    {
        points = null;
        List<SectorController> sectorGroup = null;
        foreach (var group in _connectedSectorGroups.Where(group => group.Contains(from) && group.Contains(target)))
        {
            sectorGroup = group;
        }

        if (sectorGroup is null) return -1;

        points = CalculateTeleporterPath(pos, speed, from, target, sectorGroup, out var moveTime);
        return points.Sum(tp => tp.TeleportTime) + moveTime;
    }

    private List<TeleportPoint> CalculateTeleporterPath(Vector3 pos, float speed, SectorController from, SectorController target, List<SectorController> group, out float moveTime)
    {
        var dijkstraTable = new Dictionary<SectorController, (float, SectorController)>();
        foreach (var sector in group)
        {
            dijkstraTable[sector] = (Mathf.Infinity, null);
        }
        dijkstraTable[from] = (0, from);
        SolveTable(dijkstraTable);
        var points = new List<TeleportPoint>();
        var current = target;
        
        while (current != from)
        {
            points.Insert(0, _paths[(dijkstraTable[current].Item2, current)].Item2);
            current = dijkstraTable[current].Item2;
        }

        moveTime = 0;
        if (points.Count < 1) return points;
        
        moveTime += Vector3.Distance(pos, points[0].transform.position) / speed;
        for (int i = 1; i < points.Count; i++)
        {
            moveTime += Vector3.Distance(points[i].transform.position,
                         points[i - 1].OtherTeleportPoint.transform.position) / speed;
        }

        return points;
    }

    //TODO: own class
    private void SolveTable(IDictionary<SectorController, (float, SectorController)> table)
    {
        var visitedSectors = new HashSet<SectorController>();
        while (visitedSectors.Count < table.Count)
        {
            var currentSector = table.Where(kvp => !visitedSectors.Contains(kvp.Key))
                .ToDictionary(x => x.Key, x => x.Value.Item1).MinKeyByValue();
            
            foreach (var sector in _sectorConnections[currentSector])
            {
                var distance = _paths[(currentSector, sector)].Item1 + table[currentSector].Item1;
                if (distance < table[sector].Item1)
                {
                    table[sector] = (distance, currentSector);
                }
            }

            visitedSectors.Add(currentSector);
        }
    }

    public void AddTeleportPointPair(TeleportPoint fromPoint, TeleportPoint toPoint, SectorController fromSector, SectorController toSector)
    {
        if (fromSector == toSector) return;
        AddData((fromSector, toSector), fromPoint);
        AddData((toSector, fromSector), toPoint);
        List<SectorController> fromGroup = null;
        List<SectorController> toGroup = null;
        foreach (var group in _connectedSectorGroups)
        {
            if (group.Contains(fromSector)) fromGroup = group;
            if (group.Contains(toSector)) toGroup = group;
        }
        if (toGroup != null && fromGroup == null)
        {
            toGroup.Add(fromSector);
        } 
        else if (fromGroup != null && toGroup == null)
        {
            fromGroup.Add(toSector);
        }
        else if (fromGroup == null && toGroup == null)
        {
            _connectedSectorGroups.Add(new List<SectorController>() {fromSector, toSector});
        }
        else if (fromGroup != toGroup)
        {
            _connectedSectorGroups[_connectedSectorGroups.IndexOf(fromGroup)].AddRange(toGroup);
            _connectedSectorGroups.Remove(toGroup);
        }
    }

    private void AddData((SectorController, SectorController) sectorLink, TeleportPoint point)
    {
        if (!_paths.ContainsKey(sectorLink)) _paths[sectorLink] = (point.TeleportTime, point);
        if (point.TeleportTime < _paths[sectorLink].Item1) _paths[sectorLink] = (point.TeleportTime, point);
        if (!_sectorConnections.ContainsKey(sectorLink.Item1)) _sectorConnections[sectorLink.Item1] = new List<SectorController>();
        _sectorConnections[sectorLink.Item1].Add(sectorLink.Item2);
    }
}
