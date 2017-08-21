using UnityEngine;

/// <summary>
/// 外部保存された画像を読み込み、Unity 上で利用できるようにするクラスです。
/// </summary>
public static class SpriteLoader
{
	#region public メソッド

	/// <summary>
	/// 画像ファイルを読み込みます。
	/// </summary>
	/// <param name="path">画像ファイルのパス。</param>
	/// <returns>読み込んだ画像ファイル。</returns>
	public static ImageInfo ReadImage(string path)
	{
		// 画像ファイルの読み込み
		ImageInfo ret = null;
		if (path.EndsWith("png", true, null))
		{
			ret = ReadPNGFile(path);
		}
		else if (path.EndsWith("jpg", true, null) || path.EndsWith("jpeg", true, null))
		{
			ret = ReadJPGFile(path);
		}

		// テクスチャの作成
		ret.Texture = new Texture2D(ret.Width, ret.Height);
		ret.Texture.LoadImage(ret.Binary);
		return ret;
	}

	#endregion

	#region private メソッド

	/// <summary>
	/// PNG ファイルを読み込みます。
	/// </summary>
	/// <param name="path">PNG ファイルのパス。</param>
	/// <returns>読み込んだ PNG ファイルの情報。</returns>
	private static ImageInfo ReadPNGFile(string path)
	{
		// バイナリ列の読み込み
		var ret = new ImageInfo(path);

		// 幅と高さの取得
		int pos = 16;
		for (int i = 0; i < 4; i++)
		{
			ret.Width = ret.Width * 256 + ret.Binary[pos++];
		}
		for (int i = 0; i < 4; i++)
		{
			ret.Height = ret.Height * 256 + ret.Binary[pos++];
		}

		return ret;
	}

	private static ImageInfo ReadJPGFile(string path)
	{
		// バイナリ列の読み込み
		var ret = new ImageInfo(path);

		// 幅と高さの取得
		for (int pos = 0; pos < ret.Binary.Length - 1; pos++)
		{
			if (ret.Binary[pos] == 0xff && ret.Binary[pos + 1] >= 0xc0 && ret.Binary[pos + 1] <= 0xcf)
			{
				ret.Width = 256 * ret.Binary[pos + 7] + ret.Binary[pos + 8];
				ret.Height = 256 * ret.Binary[pos + 5] + ret.Binary[pos + 6];
				break;
			}
		}

		return ret;
	}

	#endregion
}
