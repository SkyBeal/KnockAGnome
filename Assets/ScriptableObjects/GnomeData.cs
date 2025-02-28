using UnityEngine;

[CreateAssetMenu(fileName = "New Gnome Data", menuName = "Create GnomeData")]
public class GnomeData : ScriptableObject
{
    public GameObject gnomePrefab;
    public int gnomeID;
}