namespace Zion.Vectors
{
    public enum HorizontalDirection
    {
        None = 0,
        Left = -1,
        Right = 1
    }

    public enum VerticalDirection
    {
        None = 0,
        Down = -1,
        Up = 1
    }

    
    public enum DepthDirection
    {
        None = 0,
        Back = -1,
        Forward = 1
    }

    [Flags]
    public enum Direction : byte
    {
        None  = 0,
        Left  = 0b___10,
        Right = 0b___11,
        Down  = 0b10_00,
        Up    = 0b11_00
    }

    [Flags]
    public enum Direction3 : byte
    {
        None    = 0,
        Left    = 0b______10,
        Right   = 0b______11,
        Down    = 0b___10_00,
        Up      = 0b___11_00,
        Back    = 0b10_00_00,
        Forward = 0b11_00_00
    }

    [Flags]
    public enum Axes
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }
}