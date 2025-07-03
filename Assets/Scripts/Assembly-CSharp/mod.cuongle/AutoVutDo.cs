using System.Collections.Generic;
using System.Threading;

namespace Mod.CuongLe
{
    public class AutoVutDo : IActionListener, IChatable
    {
    	private static AutoVutDo _Instance;

    	public static List<int> listVutDo;

    	private static string[] titleInput;

    	private static bool autoVut;

    	public static AutoVutDo getInstance()
    	{
    		if (_Instance == null)
    		{
    			_Instance = new AutoVutDo();
    		}
    		return _Instance;
    	}

    	public static void ShowMenu()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Auto vứt " + (autoVut ? "Bật" : "Tắt"), getInstance(), 5, null));
    		myVector.addElement(new Command("Thêm id vật phẩm", getInstance(), 1, null));
    		myVector.addElement(new Command("Xóa tất cả danh sách vật phẩm", getInstance(), 2, null));
    		myVector.addElement(new Command("Xóa 1 id vật phẩm", getInstance(), 3, null));
    		myVector.addElement(new Command("Xem danh sách vật phẩm muốn vứt", getInstance(), 4, null));
    		myVector.addElement(new Command("Lưu cài đặt danh sách", getInstance(), 6, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public void perform(int idAction, object p)
    	{
    		switch (idAction)
    		{
    		case 1:
    			ChatTextField.gI().strChat = titleInput[0];
    			ChatTextField.gI().tfChat.name = "cuong dep zai";
    			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
    			ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			break;
    		case 2:
    			listVutDo.Clear();
    			GameScr.info1.addInfo("Đã xóa tất cả item cần vứt", 0);
    			break;
    		case 3:
    			ChatTextField.gI().strChat = titleInput[1];
    			ChatTextField.gI().tfChat.name = "cuong dep zai";
    			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
    			ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			break;
    		case 4:
    		{
    			if (listVutDo.Count == 0)
    			{
    				ChatPopup.addChatPopupMultiLineGameline("Danh Sách Trống!", 0, null, 10);
    				break;
    			}
    			string text = "";
    			for (int j = 0; j < listVutDo.Count; j++)
    			{
    				if (j == listVutDo.Count - 1)
    				{
    					text += listVutDo[j];
    					break;
    				}
    				text = text + listVutDo[j] + ",";
    			}
    			ChatPopup.addChatPopupMultiLineGameline(text, 0, null, 10);
    			break;
    		}
    		case 5:
    			autoVut = !autoVut;
    			if (autoVut)
    			{
    				GameScr.info1.addInfo("Auto vứt item đã bật", 0);
    				new Thread(vutListItem).Start();
    			}
    			break;
    		case 6:
    		{
    			string[] array = new string[listVutDo.Count];
    			for (int i = 0; i < listVutDo.Count; i++)
    			{
    				array[i] = listVutDo[i].ToString();
    			}
    			string data = string.Join(",", array);
    			Rms.saveRMSString("listVutDo", data);
    			GameScr.info1.addInfo("Đã lưu danh sách vật phẩm", 0);
    			break;
    		}
    		}
    	}

    	private static void ResetChatTextField()
    	{
    		ChatTextField.gI().strChat = "Chat";
    		ChatTextField.gI().tfChat.name = "chat";
    		ChatTextField.gI().isShow = false;
    	}

    	static AutoVutDo()
    	{
    		listVutDo = new List<int>();
    		titleInput = new string[2] { "Nhập id Item cần thêm", "Nhập id Item cần xóa" };
    	}

    	public void onChatFromMe(string text, string to)
    	{
    		if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
    		{
    			if (ChatTextField.gI().strChat.Equals(titleInput[0]))
    			{
    				try
    				{
    					int num = int.Parse(ChatTextField.gI().tfChat.getText());
    					if (listVutDo.Contains(num))
    					{
    						GameScr.info1.addInfo("Id Item này đã tồn tại", 0);
    						ResetChatTextField();
    					}
    					else if (num >= 0)
    					{
    						listVutDo.Add(num);
    						GameScr.info1.addInfo("Đã thêm thành công id Item " + num, 0);
    						ResetChatTextField();
    					}
    					else
    					{
    						GameScr.info1.addInfo("Vui lòng nhập đúng id Item", 0);
    						ResetChatTextField();
    					}
    					return;
    				}
    				catch
    				{
    					GameScr.info1.addInfo("Tao bảo nhập id Item m nhập cái gì thế ?????", 0);
    					ResetChatTextField();
    					return;
    				}
    			}
    			if (!ChatTextField.gI().strChat.Equals(titleInput[1]))
    			{
    				return;
    			}
    			try
    			{
    				int num2 = int.Parse(ChatTextField.gI().tfChat.getText());
    				if (listVutDo.Contains(num2) && num2 > 0)
    				{
    					listVutDo.Remove(num2);
    					GameScr.info1.addInfo("Đã xóa id Item " + num2, 0);
    					ResetChatTextField();
    				}
    				else
    				{
    					GameScr.info1.addInfo("Item này không tồn tại trong danh sách", 0);
    					ResetChatTextField();
    				}
    				return;
    			}
    			catch
    			{
    				GameScr.info1.addInfo("Tao bảo nhập id Item m nhập cái gì thế ?????", 0);
    				ResetChatTextField();
    				return;
    			}
    		}
    		ChatTextField.gI().isShow = false;
    	}

    	public void onCancelChat()
    	{
    		if (GameScr.isPaintMessage)
    		{
    			GameScr.isPaintMessage = false;
    			ChatTextField.gI().center = null;
    		}
    	}

    	public static void vutItem(int IDitem)
    	{
    		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
    		{
    			if (!autoVut)
    			{
    				break;
    			}
    			Item item = Char.myCharz().arrItemBag[i];
    			if (item != null && item.template.id == IDitem)
    			{
    				while (Char.myCharz().isWaitMonkey)
    				{
    					Thread.Sleep(1000);
    				}
    				Service.gI().useItem(2, 1, (sbyte)i, -1);
    				Thread.Sleep(500);
    			}
    		}
    	}

    	private static void vutListItem()
    	{
    		while (autoVut)
    		{
    			for (int i = 0; i < listVutDo.Count; i++)
    			{
    				vutItem(listVutDo[i]);
    				while (Char.myCharz().isWaitMonkey)
    				{
    					Thread.Sleep(1000);
    				}
    			}
    			Thread.Sleep(3000);
    		}
    		GameScr.info1.addInfo("Auto vứt đã dừng!", 0);
    	}

    	public static void loadData()
    	{
    		string text = Rms.loadRMSString("listVutDo");
    		if (string.IsNullOrEmpty(text))
    		{
    			return;
    		}
    		try
    		{
    			listVutDo.Clear();
    			string[] array = text.Split(',');
    			for (int i = 0; i < array.Length; i++)
    			{
    				if (int.TryParse(array[i], out var result) && result >= 0)
    				{
    					listVutDo.Add(result);
    				}
    			}
    		}
    		catch
    		{
    			GameScr.info1.addInfo("Lỗi khi tải danh sách vật phẩm", 0);
    		}
    	}
    }
}
