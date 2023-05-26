using System;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    [Serializable]
    public class Decal
    {
        public Enums.DecalTypes type;
        public GameObject decalObject;
    }

    [SerializeField]
    private Decal[] decals;

    public void CreateDecal(Enums.DecalTypes _decalType, Vector3 _point, Quaternion _direction)
    {
        for (int i = 0; i < decals.Length; i++)
        {
            if (decals[i].type == _decalType)
            {
                Instantiate<GameObject>(decals[i].decalObject, _point, _direction);
            }
        }
    }
}
