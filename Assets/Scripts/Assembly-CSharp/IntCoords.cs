using System;

internal struct IntCoords : IEquatable<IntCoords>
{
	private int X;

	private int Y;

	private int Z;

	public IntCoords(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Y.GetHashCode() * 27644437) ^ (Z.GetHashCode() * 1073676287);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		return Equals((IntCoords)obj);
	}

	public bool Equals(IntCoords other)
	{
		return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
	}

	public override string ToString()
	{
		return "(" + X + ", " + Y + ", " + Z + ")";
	}
}
