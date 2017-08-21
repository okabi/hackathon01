using UnityEngine;

/// <summary>
/// int 型の Vector2
/// </summary>
public struct Vector2Int
{
	#region フィールド

	public int x;
	public int y;

	#endregion

	#region コンストラクタ

	public Vector2Int(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	#endregion

	#region public メソッド

	public Vector2 ToFloat()
	{
		return new Vector2(x, y);
	}

	public override bool Equals(object other)
	{
		if (other == null) return false;

		var castedOther = other as Vector2Int?;
		if (castedOther == null) return false;

		return this == castedOther;
	}

	public override int GetHashCode()
	{
		return x ^ y;
	}

	public override string ToString()
	{
		return string.Format("(%d, %d)", x, y);
	}

	#endregion

	#region 演算子オーバーライド

	public static Vector2Int operator +(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.x + b.x, a.y + b.y);
	}

	public static Vector2Int operator -(Vector2Int a)
	{
		return new Vector2Int(-a.x, -a.y);
	}

	public static Vector2Int operator -(Vector2Int a, Vector2Int b)
	{
		return new Vector2Int(a.x - b.x, a.y - b.y);
	}

	public static Vector2Int operator *(Vector2Int a, float d)
	{
		return new Vector2Int((int)(a.x * d), (int)(a.y * d));
	}

	public static Vector2Int operator *(float d, Vector2Int a)
	{
		return a * d;
	}

	public static Vector2Int operator /(Vector2Int a, float d)
	{
		return new Vector2Int((int)(a.x / d), (int)(a.y / d));
	}

	public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y;
	}

	public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
	{
		return !(lhs == rhs);
	}

	#endregion
}
