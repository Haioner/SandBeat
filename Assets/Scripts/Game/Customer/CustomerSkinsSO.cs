using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public List<Sprite> IdleSprites;
}

[CreateAssetMenu (fileName = "CustomerSkins", menuName ="Customers/Skins")]
public class CustomerSkinsSO : ScriptableObject
{
    public List<Skin> Skins;
}
