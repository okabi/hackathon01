using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームを制御するクラスです。
/// </summary>
public class GameController : MonoBehaviour
{
	#region フィールド

	/// <summary>読み込む画像ファイルのパス。</summary>
	[SerializeField]
	private string imagePath;

	/// <summary>パネルのプレハブ。</summary>
	[SerializeField]
	private GameObject panelPrefab;
	
	/// <summary>ゲームフィールドの幅。</summary>
	[SerializeField]
	private int width;

	/// <summary>ゲームフィールドの高さ。</summary>
	[SerializeField]
	private int height;

	/// <summary>パネル状態を表す辞書。空白箇所は null。</summary>
	private Dictionary<Vector2Int, Panel> panels;

	/// <summary>空白箇所。</summary>
	private Vector2Int nullPos;

	#endregion

	#region public メソッド

	public Vector2Int WorldToIndex(Vector2 worldPosition)
	{
		return new Vector2Int();
	}

	public Vector2 IndexToWorld(Vector2Int indexPosition)
	{
		return new Vector2();
	}

	#endregion

	#region Unity メソッド

	void Start()
	{
		var image = SpriteLoader.ReadImage(imagePath);
		panels = new Dictionary<Vector2Int, Panel>();
		var parent = GameObject.Find("Panels").transform;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var obj = Instantiate(panelPrefab, new Vector2(-200 + 50 * i, -200 + 50 * j), Quaternion.Euler(0, 0, 0), parent);
				var panel = obj.GetComponent<Panel>();
				panel.Init(new Vector2Int(i, j), image, new Vector2Int(width, height));
				panels[new Vector2Int(i, j)] = panel;

			}
		}
	}

	#endregion
}
