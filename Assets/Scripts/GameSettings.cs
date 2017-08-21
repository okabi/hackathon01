using UnityEngine;

/// <summary>
/// ゲーム設定を表すクラスです。
/// </summary>
public static class GameSettings
{
	#region プロパティ

	/// <summary>読み込まれた画像情報。</summary>
	public static ImageInfo ImageInfo { get; set; }

	/// <summary>画像のヨコ分割数。</summary>
	public static int Width { get; set; }

	/// <summary>画像のタテ分割数。</summary>
	public static int Height { get; set; }

	#endregion

	#region 静的コンストラクタ

	static GameSettings()
	{
		ImageInfo = null;
		Width = 0;
		Height = 0;
	}

	#endregion
}
