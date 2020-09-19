using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserView
{
    void SetChips(IEnumerable<Chip> chips);
    void ClearAll();
    void SwitchMove(IEnumerable<Cell> allowed);
    void ShowResult(Color winner);
}
