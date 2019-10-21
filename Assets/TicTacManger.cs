using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacManger : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private int _neededInRow = 3; 
    private bool _playerTurn = true;
    private bool GameOver = false;

    private void Update()
    {
        if (GameOver)
            return;
        
        if (_playerTurn)
        {
            PlayerUpdate();
            return;
        }
   
        Ai();
    }

    private void PlayerUpdate()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            CellData data = hit.transform.GetComponent<CellData>();
            if (data)
            {
                if (data.OccupiedBy == Occupier.blank)
                {
                    data.OccupiedBy = Occupier.human;
                    EndRound();
                }
            }
        }
    }

    private void Ai()
    {
 
        Vector2Int move = AiFindBestMove(_gridManager._grid);
        _gridManager._grid[move.x][move.y].OccupiedBy = Occupier.computer;
        EndRound();
        
    }

    private int MiniMax(CellData[][] board, int depth, bool isMax, int score)
    {
        if(!IsMovesLeft())
        {
            return 0;
        }

        int bestValue;

        if (isMax)
        {
            bestValue = int.MinValue;

            for (int y = 0; y < _gridManager._gridHeight; y++)
            {
                for (int x = 0; x < _gridManager._gridWidth; x++)
                {
                    if (board[x][y].OccupiedBy == Occupier.blank)
                    {
                        board[x][y].OccupiedBy = Occupier.computer;
                        bestValue = score;
                        int value = MiniMax(board, depth + 1, !isMax, GetScore(depth+1));
                        bestValue = Math.Max(bestValue, value);
                        board[x][y].OccupiedBy = Occupier.blank;
                    }
                }
            }
            return bestValue;
        }
        else
        {
            bestValue = int.MaxValue;
            for (int y = 0; y < _gridManager._gridHeight; y++)
            {
                for (int x = 0; x < _gridManager._gridWidth; x++)
                {
                    if (board[x][y].OccupiedBy == Occupier.blank)
                    {
                        board[x][y].OccupiedBy = Occupier.human;
                        bestValue = score;
                        int value = MiniMax(board, depth + 1, !isMax, GetScore(depth+1));
                        bestValue = Math.Min(bestValue, value);
                        board[x][y].OccupiedBy = Occupier.blank;
                    }
                }
            }

            return bestValue;
        }
    }

    private Vector2Int AiFindBestMove(CellData[][] board)
    {
        int bestMove = int.MinValue;
        Vector2Int bestMovePos = new Vector2Int();
        
        for (int y = 0; y < _gridManager._gridHeight; y++)
        {
            for (int x = 0; x < _gridManager._gridWidth; x++)
            {
                if (board[x][y].OccupiedBy == Occupier.blank)
                {
                    board[x][y].OccupiedBy = Occupier.computer;

                    int moveValue = MiniMax(board, 0, false, GetScore(0));
                    board[x][y].OccupiedBy = Occupier.blank;

                    if (moveValue > bestMove)
                    {
                        bestMovePos = new Vector2Int(x,y);
                        bestMove = moveValue;
                    }
                }
            }
        }
        
        
        return bestMovePos;
    }

    private bool IsMovesLeft()
    {
        for (int y = 0; y < _gridManager._gridHeight; y++)
        {
            for (int x = 0; x < _gridManager._gridWidth; x++)
            {
                if (_gridManager._grid[x][y].OccupiedBy == Occupier.blank)
                    return true;
            }
        }

        return false;
    }
    

    private void EndRound()
    {
        _playerTurn = !_playerTurn;
    }

    private int GetScore(int depth)
    {
    //Check x rows
        for (int y = 0; y < _gridManager._gridHeight; y++)
        {
            int pc = 0;
            int human = 0;
            for (int x = 0; x < _gridManager._gridWidth; x++)
            {
            
                if (_gridManager._grid[x][y].OccupiedBy == Occupier.computer)
                    pc++;

                else if (_gridManager._grid[x][y].OccupiedBy == Occupier.human)
                    human++;
            }
            
            if (pc >= _neededInRow)
            {
                return 10 - depth;
            }
                
            if (human >= _neededInRow)
            {
                return depth -10;
            }
        }
        
        //Check Y Rows
        for (int x = 0; x < _gridManager._gridWidth; x++)
        {
            int pc = 0;
            int human = 0;
            for (int y = 0; y < _gridManager._gridHeight; y++)
            {
            
                if (_gridManager._grid[x][y].OccupiedBy == Occupier.computer)
                    pc++;

                else if (_gridManager._grid[x][y].OccupiedBy == Occupier.human)
                    human++;
            }
            if (pc >= _neededInRow)
            {
                return 10 - depth;
            }
                
            if (human >= _neededInRow)
            {
                return depth -10;
            }
        }
        
        //Check Diagonal Upwards
        for (int y = 0; y < _gridManager._gridHeight; y++)
        {
            int pc = 0;
            int human = 0;
            for (int x = 0; x < _gridManager._gridWidth; x++)
            {
                if (_gridManager._grid[x][y].OccupiedBy == Occupier.computer)
                    pc++;

                else if (_gridManager._grid[x][y].OccupiedBy == Occupier.human)
                    human++;
                y++;
            }
            
            if (pc >= _neededInRow)
            {
                return 10 - depth;
            }
                
            if (human >= _neededInRow)
            {
                return depth -10;
            }
        }
        
        //Check Diagonal Downwards
        for (int y = _gridManager._gridHeight-1; y >= 0; y--)
        {
            int pc = 0;
            int human = 0;
            for (int x = 0; x < _gridManager._gridWidth; x++)
            {
                if (_gridManager._grid[x][y].OccupiedBy == Occupier.computer)
                    pc++;

                else if (_gridManager._grid[x][y].OccupiedBy == Occupier.human)
                    human++;
                y--;
            }
            
            if (pc >= _neededInRow)
            {
                return 10 - depth;
            }
                
            if (human >= _neededInRow)
            {
                return depth -10;
            }
        }

        return 0;
    }


}