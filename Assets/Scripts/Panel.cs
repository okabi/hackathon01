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
	/// <param name="rect">画像の切り取り情報。</param>
	/// <param name="scale">画像の拡縮率。</param>
	public void Init(Vector2Int correctIndex, ImageInfo imageInfo, Rect rect, Vector2 scale)
	{
		spriteRenderer = GetComponent<SpriteRenderer>() ?? spriteRenderer;
		spriteRenderer.sprite = Sprite.Create(imageInfo.Texture, rect, new Vector2(0.5f, 0.5f), 1);
		transform.localScale = scale;
		CorrectIndex = correctIndex;
	}

	#endregion
}
