using UnityEngine;
using System.Collections;

public interface GameState
{
    void Init();
    void BeginState();
    void EndState();
    void UpdateState();
    string GetStateName();
}
