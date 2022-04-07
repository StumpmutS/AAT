using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SectorManager : MonoBehaviour
{
    private Dictionary<SectorController, List<SectorController>> _sectorConnections =
        new Dictionary<SectorController, List<SectorController>>();
    private Dictionary<(SectorController, SectorController), (float, TeleportPoint)> _paths = 
        new Dictionary<(SectorController, SectorController), (float, TeleportPoint)>();

    private List<List<SectorController>> _connectedSectorGroups = new List<List<SectorController>>();


    public static SectorManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool PathBetween(SectorController from, SectorController target, out List<TeleportPoint> points)
    {
        points = null;
        List<SectorController> sectorGroup = null;
        foreach (var group in _connectedSectorGroups.Where(group => group.Contains(from) && group.Contains(target)))
        {
            sectorGroup = group;
        }

        if (sectorGroup is null) return false;

        points = CalculateTeleporterPath(from, target, sectorGroup);
        return points.Count > 0;
    }

    private List<TeleportPoint> CalculateTeleporterPath(SectorController from, SectorController target, List<SectorController> group)
    {
        var dijkstraTable = new Dictionary<SectorController, (float, SectorController)>();
        foreach (var sector in group)
        {
            dijkstraTable[sector] = (Mathf.Infinity, null);
        }
        dijkstraTable[from] = (0, from);
        SolveTable(dijkstraTable, from);
        var points = new List<TeleportPoint>();
        var current = target;
        
        while (current != from)
        {
            points.Insert(0, _paths[(dijkstraTable[current].Item2, current)].Item2);
            current = dijkstraTable[current].Item2;
        }

        return points;
    }

    private void SolveTable(Dictionary<SectorController, (float, SectorController)> table, SectorController start)
    {
        var visitedSectors = new HashSet<SectorController>();
        while (visitedSectors.Count < table.Count)
        {
            var currentSector = StumpDictionaryExtensions.MinKeyByValue(table.Where(kvp => 
                !visitedSectors.Contains(kvp.Key)).ToDictionary(x => x.Key, x => x.Value));
            
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
        if (toGroup != null && fromGroup is null)
        {
            toGroup.Add(fromSector);
        } 
        else if (fromGroup != null && toGroup is null)
        {
            fromGroup.Add(toSector);
        }
        else if (fromGroup is null && toGroup is null)
        {
            _connectedSectorGroups.Add(new List<SectorController>() {fromSector, toSector});
        }
        else
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
