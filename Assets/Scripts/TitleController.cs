using System;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトル画面を制御するクラスです。
/// </summary>
public class TitleController : MonoBehaviour
{
	#region フィールド

	/// <summary>読み込んだ画像の表示。</summary>
	[SerializeField]
	private Image image;

	/// <summary>画像サイズの表示。</summary>
	[SerializeField]
	private Text textImageSize;

	/// <summary>ヨコ分割数。</summary>
	[SerializeField]
	private GameObject inputFieldWidth;

	/// <summary>タテ分割数。</summary>
	[SerializeField]
	private GameObject inputFieldHeight;

	/// <summary>ゲームをはじめる。</summary>
	[SerializeField]
	private GameObject buttonStart;

	/// <summary>エラー表示用 UI。</summary>
	[SerializeField]
	private Text error;

	/// <summary>ヨコ分割数の設定が正常か。</summary>
	private bool widthFlag;

	/// <summary>タテ分割数の設定が正常か。</summary>
	private bool heightFlag;

	#endregion

	#region Unity メソッド

	void Awake()
	{
		widthFlag = false;
		heightFlag = false;
	}

	void Start()
	{
		// 二周目以降は最初から入力値を有効化しておく
		if (GameSettings.ImageInfo == null) return;
		ChangeUIAfterImageValidation(true);
		inputFieldWidth.GetComponent<InputField>().text = GameSettings.Width.ToString();
		inputFieldHeight.GetComponent<InputField>().text = GameSettings.Height.ToString();
		buttonStart.SetActive(true);
	}

	#endregion

	#region UI イベント

	public void OnButtonImageClick()
	{
		// 画像選択ダイアログを開く
		var dialog = new OpenFileDialog();
		dialog.Filter = "画像ファイル (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";

		// 選択された画像を読み込んで次のフェーズに移る
		if (dialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				GameSettings.ImageInfo = SpriteLoader.ReadImage(dialog.FileName);
			}
			catch (Exception)
			{
				ChangeUIAfterImageValidation(false);
				return;
			}
			ChangeUIAfterImageValidation(true);
		}

		// キャンセルされた、または正しく処理が完了した場合、エラーテキストを消す。
		error.text = "";
	}

	public void OnInputFieldWidthValueChanged(string text)
	{
		// バリデーションの準備
		widthFlag = false;
		buttonStart.SetActive(false);

		// 入力が空白だった場合はエラーテキストを消す。
		if (text == "")
		{
			error.text = "";
			return;
		}

		// エラーが出ることを見越して予めエラーメッセージを仕込んでおく
		error.text = "Error: ヨコ分割数には画像横サイズの約数(2以上)を入力してください。";

		// 入力が整数以外だった場合はエラーを表示する。
		try
		{
			GameSettings.Width = int.Parse(text);
		}
		catch (Exception)
		{
			return;
		}

		// 入力整数がおかしかった場合はエラーを表示する。
		if (GameSettings.Width < 2 || GameSettings.Width > GameSettings.ImageInfo.Width) return;

		// 入力整数が約数でない場合はエラーを表示する。
		if (GameSettings.ImageInfo.Width % GameSettings.Width != 0) return;

		// 正常な入力だったのでエラーテキストを消す。
		error.text = "";
		widthFlag = true;

		// ヨコもタテも正常な値なら Start ボタンを有効化する。
		if (widthFlag && heightFlag)
		{
			buttonStart.SetActive(true);
		}
	}

	public void OnInputFieldHeightValueChanged(string text)
	{
		// バリデーションの準備
		heightFlag = false;
		buttonStart.SetActive(false);

		// 入力が空白だった場合はエラーテキストを消す。
		if (text == "")
		{
			error.text = "";
			return;
		}

		// エラーが出ることを見越して予めエラーメッセージを仕込んでおく
		error.text = "Error: タテ分割数には画像縦サイズの約数(2以上)を入力してください。";

		// 入力が整数以外だった場合はエラーを表示する。
		try
		{
			GameSettings.Height = int.Parse(text);
		}
		catch (Exception)
		{
			return;
		}

		// 入力整数がおかしかった場合はエラーを表示する。
		if (GameSettings.Height < 2 || GameSettings.Height > GameSettings.ImageInfo.Height) return;

		// 入力整数が約数でない場合はエラーを表示する。
		if (GameSettings.ImageInfo.Height % GameSettings.Height != 0) return;

		// 正常な入力だったのでエラーテキストを消す。
		error.text = "";
		heightFlag = true;

		// ヨコもタテも正常な値なら Start ボタンを有効化する。
		if (widthFlag && heightFlag)
		{
			buttonStart.SetActive(true);
		}
	}

	public void OnButtonStartClick()
	{
		SceneManager.LoadScene("MainGame");
	}

	#endregion

	#region private メソッド

	/// <summary>
	/// 画像読み込みバリデーション完了後に UI 状態を変更するために呼ぶメソッドです。
	/// </summary>
	/// <param name="valid">バリデーションの結果有効な入力であったか。</param>
	private void ChangeUIAfterImageValidation(bool valid)
	{
		if (valid)
		{
			image.sprite = Sprite.Create(GameSettings.ImageInfo.Texture, new Rect(0, 0, GameSettings.ImageInfo.Width, GameSettings.ImageInfo.Height), new Vector2(0.5f, 0.5f), 1);
			image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
			textImageSize.text = string.Format("{0} x {1}", GameSettings.ImageInfo.Width, GameSettings.ImageInfo.Height);
			inputFieldWidth.SetActive(true);
			inputFieldHeight.SetActive(true);
			return;
		}
		GameSettings.ImageInfo = null;
		image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
		textImageSize.text = "";
		inputFieldWidth.SetActive(false);
		inputFieldHeight.SetActive(false);
		buttonStart.SetActive(false);
		error.text = "Error: 画像ファイルの読み込みに失敗しました。";
	}

	#endregion
}
