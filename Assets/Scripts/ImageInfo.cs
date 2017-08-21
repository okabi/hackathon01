using System.IO;
using UnityEngine;

/// <summary>
/// 画像ファイルの情報をまとめたクラスです。
/// </summary>
public class ImageInfo
{
	#region プロパティ

	/// <summary>画像ファイルのパス。</summary>
	public string Path { get; private set; }

	/// <summary>画像ファイルのバイナリ列。</summary>
	public byte[] Binary { get; private set; }

	/// <summary>幅。</summary>
	public int Width { get; set; }

	/// <summary>高さ。</summary>
	public int Height { get; set; }

	/// <summary>画像ファイルのバイナリ列から生成された Texture。</summary>
	public Texture2D Texture { get; set; }

	#endregion

	#region コンストラクタ

	/// <summary>
	/// コンストラクタです。指定されたパスからバイナリ列を読み込みます。
	/// </summary>
	/// <param name="path">画像ファイルのパス。</param>
	public ImageInfo(string path)
	{
		Path = path;
		var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		var bin = new BinaryReader(fileStream);
		Binary = bin.ReadBytes((int)bin.BaseStream.Length);
		bin.Close();
		Width = 0;
		Height = 0;
	}

	#endregion
}
