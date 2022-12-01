using System;

namespace LawnMowerRobot
{
  public class Coords
  {
    public int X { get; }
    public int Y { get; }

    public Coords(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) 
        {
            return false;
        } else {
            Coords other = obj as Coords ?? throw new ArgumentException();
            return X == other.X && Y == other.Y;
        }
    }

    public static Coords operator + (Coords a, Coords b)
    {
        return new Coords(a.X + b.X, a.Y + b.Y);
    }

        
    public static Coords operator + (Coords a, int b)
    {
        return new Coords(a.X + b, a.Y + b);
    }
        
    public static Coords operator + (int a, Coords b)
    {
        return new Coords(b.X + a, b.Y + a);
    }

    public static Coords operator - (Coords a, Coords b)
    {
        return new Coords(a.X - b.X, a.Y - b.Y);
    }

    public static Coords operator ++(Coords a)
    {
        return new Coords(a.X + 1, a.Y + 1);
    }
        
    public static Coords operator --(Coords a)
    {
        return new Coords(a.X - 1, a.Y - 1);
    }

    public static bool operator == (Coords a, Coords b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Coords a, Coords b)
    {
        return !a.Equals(b);
    }

    public override string ToString() => $"({X}, {Y})";
    
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ X.GetHashCode();
            hash = (hash * 16777619) ^ Y.GetHashCode();
            return hash;
        }
    }
 }
}