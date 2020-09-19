using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotViewer : MonoBehaviour, IUserView
{
    public void ClearAll()
    {
        throw new System.NotImplementedException();
    }

    public void SetChips(IEnumerable<Chip> chips)
    {
        throw new System.NotImplementedException();
    }

    public void ShowResult(Color winner)
    {
        throw new System.NotImplementedException();
    }

    public void SwitchMove(IEnumerable<Cell> allowed)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
