using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲームを制御するクラスです。
/// </summary>
public class GameController : MonoBehaviour
{
	#region フィールド

	/// <summary>パネルのプレハブ。</summary>
	[SerializeField]
	private GameObject panelPrefab;
	
	/// <summary>やったねテキスト。</summary>
	[SerializeField]
	private Text omedetou;

	/// <summary>タイトルに戻るボタン。</summary>
	[SerializeField]
	private GameObject buttonTitle;

	/// <summary>パネル状態を表す辞書。空白箇所は null。</summary>
	private Dictionary<Vector2Int, Panel> panels;

	/// <summary>空白箇所。</summary>
	private Vector2Int nullPos;

	/// <summary>おめでとうをチカチカさせるためのカウント。</summary>
	private int omedetouCount;

	#endregion

	#region プロパティ

	/// <summary>画像のスケール。</summary>
	public Vector2 Scale
	{
		get
		{
			return new Vector2((float)Define.BoardWidth / GameSettings.ImageInfo.Width, (float)Define.BoardHeight / GameSettings.ImageInfo.Height);
		}
	}

	/// <summary>1インデックスごとのワールド座標の差。</summary>
	public Vector2 PerIndex
	{
		get
		{
			return new Vector2((float)GameSettings.ImageInfo.Width / GameSettings.Width * Scale.x, (float)GameSettings.ImageInfo.Height / GameSettings.Height * Scale.y);
		}
	}

	/// <summary>パズルが完成しているか。</summary>
	public bool isFinished
	{
		get
		{
			foreach (var kv in panels)
			{
				if (kv.Value == null) continue;
				if (kv.Key != kv.Value.CorrectIndex) return false;
			}
			return true;
		}
	}


	#endregion

	#region Unity メソッド

	void Start()
	{
		// パネルの設置
		panels = new Dictionary<Vector2Int, Panel>();
		var parent = GameObject.Find("Panels").transform;
		for (int i = 0; i < GameSettings.Width; i++)
		{
			for (int j = 0; j < GameSettings.Height; j++)
			{
				var obj = Instantiate(panelPrefab, IndexToWorld(new Vector2Int(i, j)), Quaternion.Euler(0, 0, 0), parent);
				var panel = obj.GetComponent<Panel>();
				var rect = new Rect(
					(float)i / GameSettings.Width * GameSettings.ImageInfo.Width,
					(float)j / GameSettings.Height * GameSettings.ImageInfo.Height,
					(float)GameSettings.ImageInfo.Width / GameSettings.Width,
					(float)GameSettings.ImageInfo.Height / GameSettings.Height);
				panel.Init(new Vector2Int(i, j), GameSettings.ImageInfo, rect, Scale);
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
		// 完成判定
		if (isFinished)
		{
			// おめでと～～～
			if (omedetouCount % 5 == 0)
			{
				// 目に悪いのでほどほどにチカらせる
				omedetou.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
			}
			omedetouCount++;

			// 3秒経ったらタイトルへ戻るボタンを出す
			if(omedetouCount == 180)
			{
				buttonTitle.SetActive(true);
			}

			return;
		}

		// マウス入力
		if (Input.GetMouseButtonDown(0))
		{
			var clickedIndex = WorldToIndex(ScreenToWorld(Input.mousePosition));
			Move(clickedIndex);
		}

		// 完成判定
		if (isFinished)
		{
			// すべてのパネルを消して、一枚絵を表示する
			foreach (var kv in panels)
			{
				if (kv.Value == null) continue;
				Destroy(kv.Value);
			}
			var obj = Instantiate(panelPrefab, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
			var panel = obj.GetComponent<Panel>();
			var rect = new Rect(0, 0, GameSettings.ImageInfo.Width, GameSettings.ImageInfo.Height);
			panel.Init(new Vector2Int(0, 0), GameSettings.ImageInfo, rect, Scale);
			omedetouCount = 0;
		}
	}

	#endregion

	#region UI イベント

	public void OnButtonTitleClick()
	{
		SceneManager.LoadScene("Title");
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
		var per = new Vector2((float)GameSettings.ImageInfo.Width / GameSettings.Width * Scale.x, (float)GameSettings.ImageInfo.Height / GameSettings.Height * Scale.y);
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
		for (int i = 0; i < count || isFinished; i++)
		{
			while (true)
			{
				// nullPos の上下左右で動かす先を決める
				var rand = Random.Range(0, 5);
				var pos = nullPos + new Vector2Int(rand / 2 * (int)Mathf.Pow(-1, rand % 2), (1 - rand / 2) * (int)Mathf.Pow(-1, rand % 2));
				if (pos.x < 0 || pos.y < 0 || pos.x >= GameSettings.Width || pos.y >= GameSettings.Height) continue;

				// 動かす先と入れ替える
				Move(pos);
				break;
			}
		}
	}

	#endregion
}
