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

	/// <summary>読み込んだ画像の情報。</summary>
	private ImageInfo imageInfo;

	/// <summary>パネル状態を表す辞書。空白箇所は null。</summary>
	private Dictionary<Vector2Int, Panel> panels;

	/// <summary>空白箇所。</summary>
	private Vector2Int nullPos;

	#endregion

	#region プロパティ

	/// <summary>画像のスケール。</summary>
	public Vector2 Scale
	{
		get
		{
			return new Vector2((float)Define.BoardWidth / imageInfo.Width, (float)Define.BoardHeight / imageInfo.Height);
		}
	}

	/// <summary>1インデックスごとのワールド座標の差。</summary>
	public Vector2 PerIndex
	{
		get
		{
			return new Vector2((float)imageInfo.Width / width * Scale.x, (float)imageInfo.Height / height * Scale.y);
		}
	}

	#endregion

	#region Unity メソッド

	void Start()
	{
		// 画像の読み込み
		// TODO: ダイアログから選択できるようにしたい
		imageInfo = SpriteLoader.ReadImage(imagePath);

		// パネルの設置
		panels = new Dictionary<Vector2Int, Panel>();
		var parent = GameObject.Find("Panels").transform;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var obj = Instantiate(panelPrefab, IndexToWorld(new Vector2Int(i, j)), Quaternion.Euler(0, 0, 0), parent);
				var panel = obj.GetComponent<Panel>();
				var rect = new Rect((float)i / width * imageInfo.Width, (float)j / height * imageInfo.Height, (float)imageInfo.Width / width, (float)imageInfo.Height / height);
				panel.Init(new Vector2Int(i, j), imageInfo, rect, Scale);
				panels[new Vector2Int(i, j)] = panel;
			}
		}

		// パネルを一箇所消す
		Delete(new Vector2Int(0, 0));

		// シャッフルする
		Shuffle(1000);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var clickedIndex = WorldToIndex(ScreenToWorld(Input.mousePosition));
			Debug.Log(clickedIndex);
			Move(clickedIndex);
		}
	}

	#endregion

	#region private メソッド

	/// <summary>
	/// ワールド座標をインデックス座標に変換します。
	/// </summary>
	/// <param name="worldPosition">ワールド座標。</param>
	/// <returns>インデックス座標。</returns>
	private Vector2Int WorldToIndex(Vector2 worldPosition)
	{
		var ret = worldPosition - new Vector2(-Define.BoardWidth, -Define.BoardHeight) / 2f - PerIndex / 2f;
		return new Vector2Int(Mathf.RoundToInt(ret.x / PerIndex.x), Mathf.RoundToInt(ret.y / PerIndex.y));
	}

	/// <summary>
	/// インデックス座標をワールド座標に変換します。
	/// </summary>
	/// <param name="indexPosition">インデックス座標。</param>
	/// <returns>ワールド座標。</returns>
	private Vector2 IndexToWorld(Vector2Int indexPosition)
	{
		var per = new Vector2((float)imageInfo.Width / width * Scale.x, (float)imageInfo.Height / height * Scale.y);
		return new Vector2(-Define.BoardWidth, -Define.BoardHeight) / 2f + per / 2f + new Vector2(indexPosition.x * per.x, indexPosition.y * per.y);
	}

	/// <summary>
	/// スクリーン座標をワールド座標に変換します。
	/// </summary>
	/// <param name="screenPosition">スクリーン座標。</param>
	/// <returns>ワールド座標。</returns>
	private Vector2 ScreenToWorld(Vector2 screenPosition)
	{
		return Camera.main.ScreenToWorldPoint(screenPosition);
	}

	/// <summary>
	/// 指定した箇所のパネルを削除します。
	/// </summary>
	/// <param name="pos">削除するパネルのインデックス座標。</param>
	private void Delete(Vector2Int pos)
	{
		Destroy(panels[pos].gameObject);
		panels[pos] = null;
		nullPos = pos;
	}

	/// <summary>
	/// 指定した箇所のパネルを空白箇所へ動かします。
	/// </summary>
	/// <param name="pos">動かすパネルのインデックス座標。</param>
	/// <returns>パネルを動かすことができたか。</returns>
	private bool Move(Vector2Int pos)
	{
		// 空白座標が pos の上下左右にあるか確認
		if (nullPos != pos + new Vector2Int(0, 1) && nullPos != pos + new Vector2Int(0, -1) && nullPos != pos + new Vector2Int(1, 0) && nullPos != pos + new Vector2Int(-1, 0))
		{
			return false;
		}

		// パネルを動かす
		panels[pos].transform.position = IndexToWorld(nullPos);
		panels[nullPos] = panels[pos];
		panels[pos] = null;
		nullPos = pos;
		return true;
	}

	/// <summary>
	/// 盤面をシャッフルします。
	/// </summary>
	/// <param name="count">パネルを動かす回数。</param>
	private void Shuffle(int count)
	{
		for (int i = 0; i < count; i++)
		{
			while (true)
			{
				// nullPos の上下左右で動かす先を決める
				var rand = Random.Range(0, 5);
				var pos = nullPos + new Vector2Int(rand / 2 * (int)Mathf.Pow(-1, rand % 2), (1 - rand / 2) * (int)Mathf.Pow(-1, rand % 2));
				if (pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height) continue;

				// 動かす先と入れ替える
				Move(pos);
				break;
			}
		}
	}

	#endregion
}
