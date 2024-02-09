using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova
{
  public class AutoRoad : IComparable<AutoRoad>
  {
    public string Type
    {
      get; private set;
    }
    public double Length
    {
      get; private set;
    }
    public int NumberLanes
    {
      get; private set;
    }
    public bool Footpath
    {
      get; private set;
    }
    public bool Divider
    {
      get; private set;
    }

    public AutoRoad()
    {

    }
    public AutoRoad(string type, double length, int numberLanes, bool footpath, bool divider)
    {
      Type = type;
      Length = length;
      NumberLanes = numberLanes;
      Footpath = footpath;
      Divider = divider;
    }
    // Конструктор копіювання
    public AutoRoad(AutoRoad other)
    {
      if (other == null)
      {
        throw new ArgumentNullException(nameof(other), "The provided AutoRoad object is null.");
      }

      Type = other.Type;
      Length = other.Length;
      NumberLanes = other.NumberLanes;
      Footpath = other.Footpath;
      Divider = other.Divider;
    }

    public int CompareTo(AutoRoad other)
    {
      if (other == null)
      {
        return 1; // Якщо інший об'єкт null, то поточний об'єкт більший
      }

      return Length.CompareTo(other.Length);
    }
  }
}
