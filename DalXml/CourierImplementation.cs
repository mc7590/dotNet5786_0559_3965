using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;

 ///////////////////////////////implement!/////////////////////////
internal class CourierImplementation : ICourier
{
    //working by 2nd method
    public void Create(Courier item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Courier? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Courier? Read(Func<Courier, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Courier item)
    {
        throw new NotImplementedException();
    }
}
