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

        private static bool Throwing;

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
            myVector.addElement(new Command("Auto vứt: " + (autoVut ? "Bật" : "Tắt"), getInstance(), 5, null));
            myVector.addElement(new Command("Thêm id vật phẩm", getInstance(), 1, null));
            myVector.addElement(new Command("Xóa tất cả danh sách", getInstance(), 2, null));
            myVector.addElement(new Command("Xóa id vật phẩm", getInstance(), 3, null));
            myVector.addElement(new Command("Xem danh sách vật phẩm", getInstance(), 4, null));
            myVector.addElement(new Command("Lưu danh sách", getInstance(), 6, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    ChatTextField.gI().strChat = titleInput[0];
                    ChatTextField.gI().tfChat.name = "idItem hoặc _ giữa các idItem";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 2:
                    listVutDo.Clear();
                    GameScr.info1.addInfo("Đã xóa toàn bộ danh sách vật phẩm", 0);
                    break;
                case 3:
                    ChatTextField.gI().strChat = titleInput[1];
                    ChatTextField.gI().tfChat.name = "idItem hoặc _ giữa các idItem";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 4:
                    {
                        if (listVutDo.Count == 0)
                        {
                            ChatPopup.addChatPopupMultiLineGameline("Danh sách vật phẩm trống!", 0, null, 10);
                            break;
                        }
                        string text = "";
                        for (int j = 0; j < listVutDo.Count; j++)
                        {
                            string itemName = GetNameItem((short)listVutDo[j]);
                            text += listVutDo[j] + ": " + (string.IsNullOrEmpty(itemName) ? "Unknown" : itemName);
                            if (j < listVutDo.Count - 1)
                            {
                                text += "\n";
                            }
                        }
                        ChatPopup.addChatPopupMultiLineGameline(text, 0, null, 10);
                        break;
                    }
                case 5:
                    autoVut = !autoVut;
                    GameScr.info1.addInfo("Auto vứt item: " + (autoVut ? "Bật" : "Tắt"), 0);
                    if (autoVut)
                    {
                        new Thread(new ThreadStart(vutListItem)).Start();
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
                case 7:
                    {
                        AddListVutDo((int)p);
                        break;
                    }
                case 8:
                    {
                        DeleteListVutDo((int)p);
                        break;
                    }
            }
        }

        private static void ResetChatTextField()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
            ChatTextField.gI().isShow = false;
        }

        static AutoVutDo()
        {
            listVutDo = new List<int>();
            titleInput = new string[2] { "Nhập id Item cần thêm", "Nhập id Item cần xóa" };
        }

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
            {
                ChatTextField.gI().isShow = false;
                ResetChatTextField();
            }

            else if (ChatTextField.gI().strChat.Equals(titleInput[0]))
            {
                try
                {
                    string[] ids = text.Split('_');
                    List<string> success = new List<string>();
                    List<string> failed = new List<string>();
                    foreach (string id in ids)
                    {
                        if (string.IsNullOrEmpty(id.Trim()))
                            continue;

                        int num;
                        if (int.TryParse(id.Trim(), out num) && num >= 0)
                        {
                            string itemName = GetNameItem((short)num);
                            string displayName = num + " (" + (string.IsNullOrEmpty(itemName) ? "Unknown" : itemName) + ")";
                            if (!listVutDo.Contains(num))
                            {
                                listVutDo.Add(num);
                                success.Add(displayName);
                            }
                            else
                            {
                                failed.Add(displayName + ": đã tồn tại");
                            }
                        }
                        else
                        {
                            failed.Add(id + ": không hợp lệ");
                        }
                    }
                    string message = "";
                    if (success.Count > 0)
                    {
                        message += "Đã thêm: " + string.Join(", ", success.ToArray());
                    }
                    if (failed.Count > 0)
                    {
                        if (message.Length > 0) message += ". ";
                        message += "Thất bại: " + string.Join(", ", failed.ToArray());
                    }
                    if (message.Length == 0)
                    {
                        message = "Không thêm được id Item nào";
                    }
                    GameScr.info1.addInfo(message, 0);
                }
                catch
                {
                    GameScr.info1.addInfo("Vui lòng nhập đúng định dạng: số hoặc _ giữa các số", 0);
                    
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(titleInput[1]))
            {
                try
                {
                    string[] ids = text.Split('_');
                    List<string> success = new List<string>();
                    List<string> failed = new List<string>();
                    foreach (string id in ids)
                    {
                        if (string.IsNullOrEmpty(id.Trim()))
                            continue;

                        int num;
                        if (int.TryParse(id.Trim(), out num) && num >= 0)
                        {
                            string itemName = GetNameItem((short)num);
                            string displayName = num + " (" + (string.IsNullOrEmpty(itemName) ? "Unknown" : itemName) + ")";
                            if (listVutDo.Contains(num))
                            {
                                listVutDo.Remove(num);
                                success.Add(displayName);
                            }
                            else
                            {
                                failed.Add(displayName + ": không tồn tại");
                            }
                        }
                        else
                        {
                            failed.Add(id + ": không hợp lệ");
                        }
                    }
                    string message = "";
                    if (success.Count > 0)
                    {
                        message += "Đã xóa: " + string.Join(", ", success.ToArray());
                    }
                    if (failed.Count > 0)
                    {
                        if (message.Length > 0) message += ". ";
                        message += "Thất bại: " + string.Join(", ", failed.ToArray());
                    }
                    if (message.Length == 0)
                    {
                        message = "Không xóa được id Item nào";
                    }
                    GameScr.info1.addInfo(message, 0);
                }
                catch
                {
                    GameScr.info1.addInfo("Vui lòng nhập đúng định dạng: số hoặc _ giữa các số", 0);
                }
                ResetChatTextField();
            }
            else
            {
                Service.gI().chat(text);
                ChatTextField.gI().isShow = false;
            }
        }

        public void onCancelChat()
        {
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
                    if (!autoVut)
                        break;
                    vutItem(listVutDo[i]);
                    while (Char.myCharz().isWaitMonkey)
                    {
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(3000);
            }
            GameScr.info1.addInfo("Auto vứt đã dừng", 0);
        }

        public static string GetNameItem(short idItem)
        {
            return ItemTemplates.get(idItem).name;
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
                    int result;
                    if (int.TryParse(array[i].Trim(), out result) && result >= 0)
                    {
                        listVutDo.Add(result);
                    }
                }
                GameScr.info1.addInfo(listVutDo.Count > 0 ? "Tải danh sách vật phẩm thành công" : "Danh sách vật phẩm trống", 0);
            }
            catch
            {
                GameScr.info1.addInfo("Lỗi tải danh sách vật phẩm", 0);
            }
        }
        private static void AddListVutDo(int idItem)
        {
            listVutDo.Add(idItem);
            GameScr.info1.addInfo("Đã thêm " + GetNameItem((short)idItem) + " vào danh sách vứt. Vui lòng mở auto", 0);
        }
        private static void DeleteListVutDo(int idItem)
        {
            listVutDo.Remove(idItem);
            GameScr.info1.addInfo("Đã xóa " + GetNameItem((short)idItem) + " trong danh sách vứt", 0);
        }
    }
}