using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleLibrary", menuName = "Scriptable/ObstacleLibrary", order = 1)]
public class ObstacleLibrary : ScriptableObject
{
    #region Properties
    // PUBLIC
    // PRIVATE
    [SerializeField] private List<ObstacleLink> library;
    #endregion

    public GameObject GetObjForm(ObstacleForm formToSearch)
    {
        ObstacleLink link = library.Find(x => x.form == formToSearch);
        if (link.objRef)
            return link.objRef;

        return null;
    }

    public GameObject GetRandomForm()
    {
        int index = Random.Range(0, library.Count);

        return library[index].objRef;
    }
}

public enum ObstacleForm
{
    FORM_T,
    FORM_L,
    FORM_I,
    FORM_SINGLE
}

[System.Serializable]
public struct ObstacleLink
{
    public ObstacleForm form;
    public GameObject objRef;
}