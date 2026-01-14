using UnityEngine;
using System.Collections.Generic;
public class PoolData
{
    public PoolObjectDataSo Data;
    public Queue<GameObject> InactiveObjects = new Queue<GameObject>();
    public List<GameObject> ActiveObjects = new List<GameObject>();
    public Transform PoolParent;

}