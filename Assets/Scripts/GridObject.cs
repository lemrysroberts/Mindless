using UnityEngine;

public interface GridObject 
{
    GameObject GetGameObject();

    bool IsInteractive();
    void HandleMouseOver();
    void HandleMouseExit();
    void OnSelected();
    void OnDeselected();
    void OnPrimarySelect();
    void OnPrimaryDeselect();
    void OnAltSelect();
    void OnAltDeselect();
    void OnCellHovered(int cellX, int cellY);
    void UpdateMovement(float progress);
    void UpdateAction(float progress);
    void OnEndRound();
    void SetGridPosition(int cellX, int cellY);
    void GetGridPosition(ref int cellX, ref int cellY);
}
