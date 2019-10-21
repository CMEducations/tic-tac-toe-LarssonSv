using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("GridSettings:")]
    [SerializeField] public int _gridHeight = 3;
    [SerializeField] public int _gridWidth = 3;
    [SerializeField] private float _offset;
    [SerializeField] private GameObject CellPrefab;
    
    public CellData[][] _grid;
    

    private void Awake()
    {
        _grid = new CellData[_gridWidth][];

        for (int i = 0; i < _gridWidth; i++)
        {
            _grid[i] = new CellData[_gridHeight]; 
        }
        
        
        for (int y = 0; y < _gridHeight; y++)
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                Vector3 spawnPos = transform.position;
                spawnPos.x += _offset * x;
                spawnPos.y += _offset * y;
                CellData data = Instantiate(CellPrefab, spawnPos, Quaternion.identity).GetComponent<CellData>();
                data.GridPosition = new Vector2Int(x, y);
                _grid[x][y] = data;
            }
        }
    }
    


}
