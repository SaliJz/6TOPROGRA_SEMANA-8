using UnityEngine;
using System.Threading.Tasks;

public interface INodeResolver<T>
{
    public Task Resolve(DialogController controller, T node);
}