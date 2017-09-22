
using System.Collections.Generic;


public class GDDispatchCenter<T>
{
    private Dictionary<int, GeneralDelegate<T>> _GDCntr;

    public int GDKeyCount
    {
        get
        {
            return this._GDCntr.Count;
        }
    }

    public GDDispatchCenter()
    {
        this._GDCntr = new Dictionary<int, GeneralDelegate<T>>();
    }

    public void AddGD(int id, GeneralDelegate<T> gd)
    {
        if (!this.HasType(id))
        {
            GeneralDelegate<T> generalDelegate = (GeneralDelegate<T>)null;
            this._GDCntr[id] = generalDelegate;
        }
        this._GDCntr[id] += gd;
    }

    public void RemoveGD(int id, GeneralDelegate<T> gd)
    {
        if (!this.HasType(id))
            return;
        this._GDCntr[id] -= gd;
    }

    public void removeType(int id)
    {
        if (!this.HasType(id))
            return;
        this._GDCntr.Remove(id);
    }

    public void ClearGD()
    {
        this._GDCntr.Clear();
    }

    public void dispatchNPrmD(int id, T t)
    {
        if (!this.HasType(id))
            return;
        this._GDCntr[id](t);
    }

    public bool HasType(int id)
    {
        return this._GDCntr.ContainsKey(id);
    }
}

