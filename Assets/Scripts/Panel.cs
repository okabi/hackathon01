using UnityEngine;

/// <summary>
/// 1つのパネルの情報をまとめたクラスです。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Panel : MonoBehaviour
{
	#region フィールド

	/// <summary>アタッチされている SpriteRenderer。</summary>
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	#endregion

	#region プロパティ

	/// <summary>パネルの正しい（正解の）インデックス。</summary>
	public Vector2Int CorrectIndex { get; private set; }

	#endregion

	#region public メソッド

	/// <summary>
	/// パネルを初期化します。
	/// </summary>
	/// <param name="correctIndex">パネルの正しいインデックス座標。</param>
	/// <param name="imageInfo">画像情報。</param>
	/// <param name="indexSize">ゲームで用いるパネルの横縦枚数。</param>
	public void Init(Vector2Int correctIndex, ImageInfo imageInfo, Vector2Int indexSize)
	{
		spriteRenderer = GetComponent<SpriteRenderer>() ?? spriteRenderer;
		var rect = new Rect(
			(float)correctIndex.x / indexSize.x * imageInfo.Width,
			(float)correctIndex.y / indexSize.y * imageInfo.Height,
			(float)imageInfo.Width / indexSize.x,
			(float)imageInfo.Height / indexSize.y);
		spriteRenderer.sprite = Sprite.Create(imageInfo.Texture, rect, new Vector2(0.5f, 0.5f), 1);
		CorrectIndex = correctIndex;
	}

	#endregion
}
