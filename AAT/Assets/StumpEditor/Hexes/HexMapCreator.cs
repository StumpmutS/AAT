using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using Utility.Scripts;

public class HexMapCreator : EditorWindow
{
    [SerializeField] private FactionColors _factionColors;
    [SerializeField] private GameObject _hexPrefab;
    [SerializeField] private GameObject _wallJointPrefab;
    [SerializeField] private GameObject _wallJointConnectorPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private GameObject _manager;
    [SerializeField] private GameObject _upgradesUI;
    [SerializeField] private GameObject _spawnPlot;
    
    private Vector2 _dragAmount;
    private float _r = 30;
    private float _a;
    int _doubleColumns = 5;
    int _rows = 5;
    private List<List<Rect>> _buttons = new List<List<Rect>>();
    private List<List<Vector3[]>> _hexes = new List<List<Vector3[]>>();
    private List<List<EFaction>> _factionGrid = new List<List<EFaction>>();
    Vector2 _hexPadding = new Vector2(10, 10);
    private float _outlineThickness = 5;
    private bool _showSelectionGrid;
    
    private Rect _objectMenuBar;
    private float _objectMenuBarHeight = 20;
    private Rect _toggleMenuBar;
    private float _toggleMenuBarHeight = 20;
    private Rect _inputMenuBar;
    private float _inputMenuBarWidth = 210;
    private Rect _clearButton;
    private Vector2 _clearButtonSize = new Vector2(210, 20);
    private Rect _generateButton;
    private Vector2 _generateButtonSize = new Vector2(210, 20);
    
    private EFaction _currentFaction = EFaction.None;
    private List<bool> _toggleStates = new List<bool> {true, false, false, false};
    
    private float _3DMapSize = 50;

    private bool _generate;
    private bool _clear;

    [MenuItem("Window/Hex Map Creator")]
    public static void OpenWindow()
    {
        var window = GetWindow<HexMapCreator>();
        window.titleContent = new GUIContent("Hex Map Creator");
    }

    private void OnGUI()
    {
        PopulateGridStorage();

        CheckInteractions(Event.current);
        DrawHexGrid();
        DrawUI();
        
        if (GUI.changed) OnChanged();
    }

    #region StoragePopulation
    private void PopulateGridStorage()
    {
        _buttons.Populate(new Rect(), _doubleColumns, _rows * 2);
        _hexes.Populate(new Vector3[]{}, _doubleColumns, _rows * 2);
        _factionGrid.Populate(EFaction.None, _doubleColumns, _rows * 2);
        _buttons.Equalize(_doubleColumns);
        _buttons.ForEach(l => l.Equalize(_rows * 2));
        _hexes.Equalize(_doubleColumns);
        _hexes.ForEach(l => l.Equalize(_rows * 2));
        _factionGrid.Equalize(_doubleColumns);
        _factionGrid.ForEach(l => l.Equalize(_rows * 2));
    }
    #endregion
    
    #region ChangeUpdates
    private void OnChanged()
    {
        Repaint();
        UpdateToggledFaction();
        if (_generate) Generate3DHexMap();
        if (_clear) ClearGrid();
    }
    private void UpdateToggledFaction()
    {
        for (int i = 0; i < _toggleStates.Count; i++)
        {
            if (!_toggleStates[i]) continue;
            if ((EFaction)i == _currentFaction) continue;
            for (int j = 0; j < _toggleStates.Count; j++)
            {
                if (j != i) _toggleStates[j] = false;
            }
            _currentFaction = (EFaction) i;
            return;
        }
        if (_toggleStates[(int) _currentFaction]) return;
        _toggleStates[0] = true;
        _currentFaction = EFaction.None;
    }
    #endregion
    
    #region UI
    private void DrawUI()
    {
        DrawTopObjectBar();
        DrawBottomObjectBar();
        DrawToggleMenuBar();
        DrawInputMenuBar();
        DrawClearButton();
        DrawGenerateButton();
    }
    
    private void DrawTopObjectBar()
    {
        _objectMenuBar = new Rect(0, 0, position.width, _objectMenuBarHeight);
        GUILayout.BeginArea(_objectMenuBar, EditorStyles.toolbar);
        EditorGUILayout.BeginHorizontal();
        _factionColors = (FactionColors)EditorGUILayout.ObjectField("Faction Colors", _factionColors, typeof(FactionColors), false);
        _hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", _hexPrefab, typeof(GameObject), false);
        _wallJointPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Joint Prefab", _wallJointPrefab, typeof(GameObject), false);
        _wallJointConnectorPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Joint Connector Prefab", _wallJointConnectorPrefab, typeof(GameObject), false);
        _wallPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Prefab", _wallPrefab, typeof(GameObject), false);
        _containerPrefab = (GameObject)EditorGUILayout.ObjectField("Container Prefab", _containerPrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    
    private void DrawBottomObjectBar()
    {
        _objectMenuBar = new Rect(0, _objectMenuBarHeight, position.width, _objectMenuBarHeight);
        GUILayout.BeginArea(_objectMenuBar, EditorStyles.toolbar);
        EditorGUILayout.BeginHorizontal();
        _manager = (GameObject)EditorGUILayout.ObjectField("Manager", _manager, typeof(GameObject), true);
        _upgradesUI = (GameObject) EditorGUILayout.ObjectField("Upgrades UI", _upgradesUI, typeof(GameObject), true);
        _spawnPlot = (GameObject) EditorGUILayout.ObjectField("Spawn Plot", _spawnPlot, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    
    private void DrawToggleMenuBar()
    {
        _toggleMenuBar = new Rect(0, _objectMenuBarHeight * 2, position.width, _toggleMenuBarHeight);
        GUILayout.BeginArea(_toggleMenuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < _toggleStates.Count; i++)
        {
            _toggleStates[i] = GUILayout.Toggle(_toggleStates[i], Enum.GetName(typeof(EFaction), i));
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawInputMenuBar()
    {
        _inputMenuBar = new Rect(0, _objectMenuBarHeight * 2 + _toggleMenuBarHeight, _inputMenuBarWidth, position.height);
        GUILayout.BeginArea(_inputMenuBar);
        GUILayout.BeginVertical();
        _showSelectionGrid = EditorGUILayout.Toggle("Show Selection Grid", _showSelectionGrid);
        _hexPadding = EditorGUILayout.Vector2Field("Hex Padding", _hexPadding);
        _outlineThickness = EditorGUILayout.FloatField("Outline Thickness", _outlineThickness);
        _objectMenuBarHeight = EditorGUILayout.FloatField("Objects Height", _objectMenuBarHeight);
        _toggleMenuBarHeight = EditorGUILayout.FloatField("Toggles Height", _toggleMenuBarHeight);
        _inputMenuBarWidth = EditorGUILayout.FloatField("Inputs Width", _inputMenuBarWidth);
        _generateButtonSize = EditorGUILayout.Vector2Field("Generate Size", _generateButtonSize);
        _doubleColumns = EditorGUILayout.IntField("Double Columns", _doubleColumns);
        _rows = EditorGUILayout.IntField("Rows", _rows);
        _3DMapSize = EditorGUILayout.FloatField("3D Map Size", _3DMapSize);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void DrawClearButton()
    {
        _clearButton = new Rect(position.width - _clearButtonSize.x - 10, position.height - _generateButtonSize.y - _clearButtonSize.y - 10,
            _clearButtonSize.x, _clearButtonSize.y);
        GUILayout.BeginArea(_clearButton);
        _clear = GUILayout.Button("Clear");
        GUILayout.EndArea();
    }
    
    private void DrawGenerateButton()
    {
        _generateButton = new Rect(position.width - _generateButtonSize.x - 10, position.height - _generateButtonSize.y - 10,
            _generateButtonSize.x, _generateButtonSize.y);
        GUILayout.BeginArea(_generateButton);
        _generate = GUILayout.Button("Generate");
        GUILayout.EndArea();
    }
    #endregion

    #region Interaction Processes
    private void CheckInteractions(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 2)
                {
                    OnMouseDrag(e.delta);
                } 
                else if (e.button == 0)
                {
                    PaintHex(e.mousePosition);
                }
                break;
            case EventType.ScrollWheel:
                OnMouseScroll(e.delta);
                break;
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    PaintHex(e.mousePosition);
                }
                break;
        }
    }
    
    private void PaintHex(Vector2 mousePos)
    {
        if (mousePos.x >= position.width - _generateButtonSize.x - 10 && mousePos.y >= position.height - _generateButtonSize.y - 10) return;
        
        Vector2 offset = _dragAmount + new Vector2(_inputMenuBarWidth, _objectMenuBarHeight * 2 + _toggleMenuBarHeight) + _hexPadding;
        var d = _a * .57735026919f; //tan30
        int column = Mathf.FloorToInt((mousePos.x - offset.x) / (_r + d));
        if (column < 0) return;
        var oddColumn = column % 2 == 1;
        if (oddColumn) column -= 1;

        column /= 2;

        var row = oddColumn? Mathf.FloorToInt((mousePos.y - offset.y - _a) / (_a * 2)) : Mathf.FloorToInt((mousePos.y - offset.y) / (_a * 2));
        if (oddColumn && row >= 0 && row < _rows * 2) row += _rows;

        if (column >= _doubleColumns || row >= _rows * 2 || row < 0 || (!oddColumn && row >= _rows)) return;
        
        var rect = _buttons[column][row];
        var x = mousePos.x - rect.position.x;
        var y = mousePos.y - rect.position.y;
        if (y < -(_a / d) * x + _a) //above
        {
            if (oddColumn)
            {
                row -= _rows;
            }
            else if (column != 0 && row != 0)
            {
                column -= 1;
                row += _rows;
                row -= 1;
            }
        }
        else if (y > (_a / d) * x + _a) //below
        {
            if (oddColumn && row != _rows * 2 - 1)
            {
                row -= _rows;
                row += 1;
            }
            else if (!oddColumn && column != 0)
            {
                column -= 1;
                row += _rows;
            }
        }

        GUI.changed = true;
        _factionGrid[column][row] = _currentFaction;
    }

    private void OnMouseDrag(Vector2 delta)
    {
        _dragAmount += delta;
        GUI.changed = true;
    }

    private void OnMouseScroll(Vector2 delta)
    {
        _r -= delta.y;
        GUI.changed = true;
    }
    #endregion

    #region 2D Hex Grid
    private void DrawHexGrid()
    {
        _a = _r * .866025403784439f; //sin60
        float rThree = _r * 3;
        float h = _a * 2;
        
        Handles.BeginGUI();
        Vector2 offset = _dragAmount + new Vector2(_r + _inputMenuBarWidth, _a + _objectMenuBarHeight * 2 + _toggleMenuBarHeight) + _hexPadding;

        for (int i = 0; i < _doubleColumns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                var hexPos = new Vector2(i * rThree, j * h) + offset;
                var otherHexPos = new Vector2(i * rThree + 1.5f * _r, j * h + _a) + offset;
                DrawHexagon(hexPos, i, j);
                DrawHexagon(otherHexPos, i, j + _rows);
                CreateButton(hexPos, i, j);
                CreateButton(otherHexPos, i, j + _rows);
            }
        }
        
        Handles.EndGUI();
    }

    private void ClearGrid()
    {
        foreach (var column in _factionGrid)
        {
            for (int i = 0; i < column.Count; i++)
            {
                column[i] = EFaction.None;
            }
        }
    }
    
    private void CreateButton(Vector2 pos, int columnIndex, int rowIndex)
    {
        var d = _a * .57735026919f; //tan30
        
        if (!_showSelectionGrid) return;
        var points = new Vector3[]
        {
            new Vector2(pos.x + _r - d, pos.y + _a),
            new Vector2(pos.x + _r - d, pos.y - _a),
            new Vector2(pos.x - _r, pos.y - _a),
            new Vector2(pos.x - _r, pos.y + _a),
            new Vector2(pos.x + _r - d, pos.y + _a),
        };
        Handles.color = Color.magenta;
        Handles.DrawAAPolyLine(4, points);
        
        _buttons[columnIndex][rowIndex] = new Rect(pos.x - _r, pos.y - _a, _r + d, _a * 2);
    }

    private void DrawHexagon(Vector2 pos, int columnIndex, int rowIndex)
    {
        float xAdd = _r * .5f;
        
        var points = new Vector3[]
        {
            new Vector2(pos.x + _r, pos.y),
            new Vector2(pos.x + xAdd, pos.y - _a),
            new Vector2(pos.x - xAdd, pos.y - _a),
            new Vector2(pos.x - _r, pos.y),
            new Vector2(pos.x - xAdd, pos.y + _a),
            new Vector2(pos.x + xAdd, pos.y + _a),
            new Vector2(pos.x + _r, pos.y)
        };
        Handles.color = _factionColors.FactionsByColor[_factionGrid[columnIndex][rowIndex]];
        Handles.DrawAAConvexPolygon(points);
        Handles.color = Color.black;
        Handles.DrawAAPolyLine(_outlineThickness, points);

        _hexes[columnIndex][rowIndex] = points;
    }
    #endregion

    #region 3D Hex Map
    private void Generate3DHexMap()
    {
        Debug.Log("GENERATE!");
        GameObject hexRef = null;
        var jointRefGrid = new List<List<WallJointController[]>>(); //0 is bottom right vertex, 1 is right vertex
        jointRefGrid.Populate(new WallJointController[2], _doubleColumns, _rows * 2);
        var hexPrefabs = new List<List<HexController>>();
        hexPrefabs.Populate(null, _doubleColumns, _rows * 2);
        for (int i = 0; i < _factionGrid.Count; i++)
        {
            for (int j = 0; j < _factionGrid[i].Count; j++)
            {
                if (_factionGrid[i][j] != EFaction.None)
                {
                    hexPrefabs[i][j] = ((GameObject)PrefabUtility.InstantiatePrefab(_hexPrefab)).GetComponent<HexController>();
                    hexRef ??= hexPrefabs[i][j].gameObject; //need ref to just one
                }
            }
        }
        
        var hexContainer = (GameObject)PrefabUtility.InstantiatePrefab(_containerPrefab);
        hexContainer.name = "Hex Container";
        var wallContainer = (GameObject)PrefabUtility.InstantiatePrefab(_containerPrefab);
        wallContainer.name = "Wall Container";
        var spawnPlotContainer = (GameObject)PrefabUtility.InstantiatePrefab(_containerPrefab);
        spawnPlotContainer.name = "Spawn Plot Container";

        var rConv = _3DMapSize / _r;
        var aConv = _3DMapSize * Mathf.Cos(Mathf.Deg2Rad*30) / _a;
        
        for (int i = 0; i < _hexes.Count; i++)
        {
            for (int j = 0; j < _hexes[i].Count; j++)
            {
                if (_factionGrid[i][j] == EFaction.None)
                {
                    continue;
                }
                var pos = new Vector2((_hexes[i][j][0].x + rConv) * rConv, -(_hexes[i][j][0].y) * aConv);
                var hex = hexPrefabs[i][j];
                hex.Setup(_factionGrid[i][j]);
                hex.transform.position = new Vector3(pos.x, 0, pos.y);
                hex.transform.localScale *= _3DMapSize;
                hex.transform.parent = hexContainer.transform;
                var spawnPlot = ((GameObject)PrefabUtility.InstantiatePrefab(_spawnPlot)).GetComponent<SpawnerPlotController>();
                spawnPlot.transform.position = new Vector3(pos.x, hexRef.transform.localScale.y / 2, pos.y);
                spawnPlot.Setup(_upgradesUI, hex.GetComponent<SectorController>(), _factionGrid[i][j]);
                spawnPlot.transform.parent = spawnPlotContainer.transform;
                _manager.GetComponent<SpawnPlotManager>().AddPlot(spawnPlot);
                
                WallJointController bottomLeftVertexJoint = null;
                WallJointController bottomRightVertexJoint = null;
                WallJointController rightVertexJoint = null;
                WallJointController topRightVertexJoint = null;

                var oddColumn = j >= _rows;

                if (AdjacentBottomSide(i, j))
                {
                    if (j >= _rows)
                    {
                        bottomLeftVertexJoint = jointRefGrid[i][j - _rows + 1][1];
                    } else if (i > 0)//TODO:
                    {
                        bottomLeftVertexJoint = jointRefGrid[i - 1][j + _rows][1];
                    }
                    bottomLeftVertexJoint ??= ((GameObject)PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    bottomRightVertexJoint = ((GameObject)PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    var bottomLeftPos = hex.transform.position + new Vector3(-_3DMapSize * .5f, 0, -aConv * _a);
                    var bottomRightPos = hex.transform.position + new Vector3(_3DMapSize * .5f, 0, -aConv * _a);
                    var sectors = new SerializableTuple<SectorController, SectorController>(hex.GetComponent<SectorController>(), hexPrefabs[i][j + 1].GetComponent<SectorController>());
                    SetupWalls(bottomLeftVertexJoint, bottomRightVertexJoint, bottomLeftPos, bottomRightPos, wallContainer.transform, sectors);
                }

                if (AdjacentLowerRightSide(i, j, oddColumn))
                {
                    bottomRightVertexJoint ??= ((GameObject)PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    rightVertexJoint = ((GameObject)PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    var bottomRightPos = hex.transform.position + new Vector3(_3DMapSize * .5f, 0, -aConv * _a);
                    var rightPos = hex.transform.position + new Vector3(_3DMapSize, 0, 0);
                    var sectors = new SerializableTuple<SectorController, SectorController>(hex.GetComponent<SectorController>(), (oddColumn ? hexPrefabs[i + 1][j - _rows + 1] : hexPrefabs[i][j + _rows]).GetComponent<SectorController>());
                    SetupWalls(bottomRightVertexJoint, rightVertexJoint, bottomRightPos, rightPos, wallContainer.transform, sectors);
                }

                if (AdjacentUpperRightSide(i, j, oddColumn))
                {
                    rightVertexJoint ??= ((GameObject)PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    if (j != _rows)
                    {
                        topRightVertexJoint = jointRefGrid[i][j - 1][0];
                    }
                    topRightVertexJoint ??= ((GameObject) PrefabUtility.InstantiatePrefab(_wallJointPrefab)).GetComponent<WallJointController>();
                    var rightPos = hex.transform.position + new Vector3(_3DMapSize, 0, 0);
                    var topRightPos = hex.transform.position + new Vector3(_3DMapSize * .5f, 0, aConv * _a);
                    var sectors = new SerializableTuple<SectorController, SectorController>(hex.GetComponent<SectorController>(), (oddColumn ? hexPrefabs[i + 1][j - _rows] : hexPrefabs[i][j + _rows - 1]).GetComponent<SectorController>());
                    SetupWalls(rightVertexJoint, topRightVertexJoint, rightPos, topRightPos, wallContainer.transform, sectors);
                }
                
                jointRefGrid[i][j] = new WallJointController[2];
                jointRefGrid[i][j][0] = bottomRightVertexJoint;
                jointRefGrid[i][j][1] = rightVertexJoint;
            }
        }

        if (hexRef is null) return;
        wallContainer.transform.position += new Vector3(0, hexRef.transform.localScale.y / 2, 0);
        hexRef.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void SetupWalls(WallJointController firstJoint, WallJointController secondJoint, Vector3 firstPos, Vector3 secondPos, Transform container, SerializableTuple<SectorController, SectorController> sectors)
    {
        var wallSize = _wallPrefab.GetComponent<WallController>().DimensionsContainer.XDimensions;
        var connectorSize = _wallJointConnectorPrefab.GetComponent<WallJointConnectorController>().DimensionsContainer.XDimensions;
        
        var wallDirection = secondPos - firstPos;
        var wallDirNorm = wallDirection.normalized;
        var wallNumber = (wallDirection.magnitude - 2 * connectorSize) / wallSize;
        var wallNumberFloor = Mathf.FloorToInt(wallNumber);
        var connectorScaleTarget =  (wallNumber - wallNumberFloor) * wallSize / 2 / connectorSize + 1;//this should be connector transform.scale.x
        
        for (int i = 0; i < wallNumberFloor; i++)
        {
            var wall = ((GameObject)PrefabUtility.InstantiatePrefab(_wallPrefab)).GetComponent<WallController>();
            wall.transform.position = firstPos + wallDirNorm * (wallSize / 2 + i * wallSize + connectorScaleTarget * connectorSize);
            wall.transform.right = wallDirection;
            wall.transform.parent = container;
            wall.GetComponent<SectorDivider>().SetSectors(sectors);
        }

        var offset = wallDirNorm * connectorSize * connectorScaleTarget / 2;
        SetupJoint(firstJoint, connectorScaleTarget, firstPos, firstPos + offset, wallDirNorm, container, false, sectors);
        SetupJoint(secondJoint, connectorScaleTarget, secondPos, secondPos - offset, wallDirNorm, container, true, sectors);
    }

    private void SetupJoint(WallJointController joint, float connectorScale, Vector3 jointPos, Vector3 connectPos, Vector3 dir, Transform container, bool negative, SerializableTuple<SectorController, SectorController> sectors)
    {
        joint.transform.position = jointPos;
        joint.transform.parent = container;
        joint.SetManager(_manager.GetComponent<WallPlacementManager>());
        var connector = (GameObject) PrefabUtility.InstantiatePrefab(_wallJointConnectorPrefab);
        connector.transform.localScale = new Vector3(connectorScale, 1, 1);
        connector.transform.right = dir;
        connector.transform.position = connectPos;
        connector.GetComponent<SectorDivider>().SetSectors(sectors);
        joint.SetupMainConnector(connector.GetComponent<WallJointConnectorController>(), negative? -dir : dir);
    }
    #endregion

    #region CheckAdjacent
    private bool AdjacentBottomSide(int column, int row)
        {
            if (row == _rows - 1 || row == _rows * 2 - 1) return false;
            return _factionGrid[column][row + 1] != EFaction.None;
        }

        private bool AdjacentLowerRightSide(int column, int row, bool odd)
        {
            if (row == _rows * 2 - 1) return false;
            if (!odd) return _factionGrid[column][row + _rows] != EFaction.None;
            if (column != _doubleColumns - 1) return _factionGrid[column + 1][row - _rows + 1] != EFaction.None;
            return false;
        }

        private bool AdjacentUpperRightSide(int column, int row, bool odd)
        {
            if (row == 0) return false;
            if (!odd && row > 0) return _factionGrid[column][row + _rows - 1] != EFaction.None;
            if (column != _doubleColumns - 1) return _factionGrid[column + 1][row - _rows] != EFaction.None;
            return false;
        }
    #endregion
}
