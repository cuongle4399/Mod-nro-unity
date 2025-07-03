using System.Collections.Generic;
using System.IO;

namespace Mod.CuongLe
{
    public class AutoChat : IActionListener, IChatable
    {
        private static AutoChat _Instance;

        private static bool isAutoChatPublic;
        private static long delayAutoChatPublic;
        private static long lastTimeChatPublic;
        private static string autoChatPublicContent;

        private static bool isAutoChatGlobal;
        private static long delayAutoChatGlobal;
        private static long lastTimeChatGlobal;
        private static string autoChatGlobalContent;
        private static int countChatGlobal;

        private static int gems;
        private static bool isAutoSpamChatGlobal;
        private static int spammedChatGlobalTimes;
        private static int timesSpamChatGlobal;

        public static bool isAutoInbox;
        public static List<int> listCharInbox;
        private static string autoInboxContent;
        private static long lastTimeInbox;
        private static int countChatInbox;

        private static bool isSaveData;

        private static string[] inputDelayChatPublic = new string[2] { "Nhập delay chat:", "Mili giây (>=2000ms)" };
        private static string[] inputDelayChatGlobal = new string[2] { "Nhập delay chat thế giới:", "Mili giây (>=2000ms)" };
        private static string[] inputSpamChatGlobal = new string[2] { "Nhập số lần spam", "Số lần" };
        private static string[] inputContentChatPublic = new string[2] { "Nhập nội dung chat:", "Nội dung" };
        private static string[] inputContentChatGlobal = new string[2] { "Nhập nội dung chat thế giới:", "Nội dung" };
        private static string[] inputContentChatInbox = new string[2] { "Nhập nội dung inbox:", "Nội dung" };

        private static string tempChatPublicContent;
        private static string tempChatGlobalContent;
        private static string tempChatInboxContent;

        public static AutoChat getInstance()
        {
            if (_Instance == null)
            {
                _Instance = new AutoChat();
            }
            return _Instance;
        }

        public static void Update()
        {
            if (isAutoChatPublic)
            {
                ChatPublic();
            }
            if (isAutoChatGlobal)
            {
                ChatGlobal();
            }
            if (isAutoSpamChatGlobal)
            {
                SpamChatGlobal();
            }
            if (isAutoInbox)
            {
                ChatInbox();
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
            {
                ChatTextField.gI().isShow = false;
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputContentChatPublic[0]))
            {
                tempChatPublicContent = text;
                ChatTextField.gI().strChat = inputDelayChatPublic[0];
                ChatTextField.gI().tfChat.name = inputDelayChatPublic[1];
                ChatTextField.gI().tfChat.setText("");
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
            }
            else if (ChatTextField.gI().strChat.Equals(inputDelayChatPublic[0]))
            {
                int num = 0;
                try
                {
                    num = int.Parse(ChatTextField.gI().tfChat.getText());
                    if (num >= 2000)
                    {
                        autoChatPublicContent = tempChatPublicContent;
                        delayAutoChatPublic = num;
                        isAutoChatPublic = true;
                        GameScr.info1.addInfo("Auto Chat: " + NinjaUtil.getMoneys(delayAutoChatPublic) + " mili giây", 0);
                        if (isSaveData)
                        {
                            Rms.saveRMSInt("AutoChatIsAutoChatPublic", 1);
                            Rms.saveRMSString("AutoChatDelayChatPublic", delayAutoChatPublic.ToString());
                        }
                    }
                    else
                    {
                        GameCanvas.startOKDlg("Delay phải >= 2000ms!");
                    }
                }
                catch
                {
                    GameCanvas.startOKDlg("Delay không hợp lệ!");
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputContentChatGlobal[0]))
            {
                tempChatGlobalContent = text;
                ChatTextField.gI().strChat = inputDelayChatGlobal[0];
                ChatTextField.gI().tfChat.name = inputDelayChatGlobal[1];
                ChatTextField.gI().tfChat.setText("");
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
            }
            else if (ChatTextField.gI().strChat.Equals(inputDelayChatGlobal[0]))
            {
                int num2 = 0;
                try
                {
                    num2 = int.Parse(ChatTextField.gI().tfChat.getText());
                    if (num2 >= 2000)
                    {
                        autoChatGlobalContent = tempChatGlobalContent;
                        delayAutoChatGlobal = num2;
                        isAutoChatGlobal = true;
                        isAutoSpamChatGlobal = false;
                        GameScr.info1.addInfo("Auto Chat Thế Giới: " + NinjaUtil.getMoneys(delayAutoChatGlobal) + " mili giây", 0);
                        if (isSaveData)
                        {
                            Rms.saveRMSInt("AutoChatIsAutoChatGlobal", 1);
                            Rms.saveRMSString("AutoChatDelayChatGlobal", delayAutoChatGlobal.ToString());
                        }
                    }
                    else
                    {
                        GameCanvas.startOKDlg("Delay phải >= 2000ms!");
                    }
                }
                catch
                {
                    GameCanvas.startOKDlg("Delay không hợp lệ!");
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputSpamChatGlobal[0]))
            {
                int num3 = 0;
                try
                {
                    num3 = int.Parse(ChatTextField.gI().tfChat.getText());
                    if (num3 > 0)
                    {
                        autoChatGlobalContent = tempChatGlobalContent;
                        timesSpamChatGlobal = num3;
                        spammedChatGlobalTimes = 0;
                        gems = Char.myCharz().luong + Char.myCharz().luongKhoa;
                        isAutoChatGlobal = false;
                        isAutoSpamChatGlobal = true;
                        GameScr.info1.addInfo("Spam Chat Thế Giới: " + timesSpamChatGlobal + " lần", 0);
                    }
                    if (num3 <= 0)
                    {
                        GameCanvas.startOKDlg("Số lần không hợp lệ!");
                    }
                }
                catch
                {
                    GameCanvas.startOKDlg("Số lần không hợp lệ!");
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputContentChatInbox[0]))
            {
                autoInboxContent = text;
                isAutoInbox = true;
                GameScr.info1.addInfo("Auto Inbox\n[STATUS: ON]", 0);
                if (isSaveData)
                {
                    Rms.saveRMSInt("AutoChatIsAutoChatInbox", 1);
                }
                ResetChatTextField();
            }
        }

        public void onCancelChat()
        {
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    if (!isAutoChatPublic)
                    {
                        ChatTextField.gI().strChat = inputContentChatPublic[0];
                        ChatTextField.gI().tfChat.name = inputContentChatPublic[1];
                        ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    }
                    if (isAutoChatPublic)
                    {
                        isAutoChatPublic = false;
                        GameScr.info1.addInfo("Auto Chat\n[STATUS: OFF]", 0);
                        if (isSaveData)
                        {
                            Rms.saveRMSInt("AutoChatIsAutoChatPublic", 0);
                        }
                    }
                    break;
                case 2:
                    if (!isAutoChatGlobal)
                    {
                        ChatTextField.gI().strChat = inputContentChatGlobal[0];
                        ChatTextField.gI().tfChat.name = inputContentChatGlobal[1];
                        ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    }
                    if (isAutoChatGlobal)
                    {
                        isAutoChatGlobal = false;
                        GameScr.info1.addInfo("Auto Chat Thế Giới\n[STATUS: OFF]", 0);
                        if (isSaveData)
                        {
                            Rms.saveRMSInt("AutoChatIsAutoChatGlobal", 0);
                        }
                    }
                    break;
                case 3:
                    if (!isAutoSpamChatGlobal)
                    {
                        ChatTextField.gI().strChat = inputContentChatGlobal[0];
                        ChatTextField.gI().tfChat.name = inputContentChatGlobal[1];
                        ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    }
                    if (isAutoSpamChatGlobal)
                    {
                        isAutoSpamChatGlobal = false;
                        GameScr.info1.addInfo("Auto Spam Chat Thế Giới\n[STATUS: OFF]", 0);
                    }
                    break;
                case 4:
                    if (isAutoInbox)
                    {
                        isAutoInbox = false;
                        GameScr.info1.addInfo("Auto Inbox\n[STATUS: OFF]", 0);
                    }
                    else
                    {
                        ChatTextField.gI().strChat = inputContentChatInbox[0];
                        ChatTextField.gI().tfChat.name = inputContentChatInbox[1];
                        ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    }
                    if (isSaveData)
                    {
                        Rms.saveRMSInt("AutoChatIsAutoChatInbox", isAutoInbox ? 1 : 0);
                    }
                    break;
                case 5:
                    isSaveData = !isSaveData;
                    GameScr.info1.addInfo("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    Rms.saveRMSInt("AutoChatIsSaveRms", isSaveData ? 1 : 0);
                    if (isSaveData)
                    {
                        SaveData();
                    }
                    break;
            }
        }

        public static void ShowMenu()
        {
            LoadSavedData();
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Auto Chat\n" + (isAutoChatPublic ? ("[" + NinjaUtil.getMoneys(delayAutoChatPublic) + " mili giây]") : "[STATUS: OFF]"), getInstance(), 1, null));
            myVector.addElement(new Command("Auto Chat Thế Giới\n" + (isAutoChatGlobal ? ("[" + NinjaUtil.getMoneys(delayAutoChatGlobal) + " mili giây]") : "[STATUS: OFF]"), getInstance(), 2, null));
            myVector.addElement(new Command("Auto Spam Chat Thế Giới\n" + (isAutoSpamChatGlobal ? ("[" + spammedChatGlobalTimes + "/" + timesSpamChatGlobal + "]") : "[STATUS: OFF]"), getInstance(), 3, null));
            myVector.addElement(new Command("Auto Inbox\n" + (isAutoInbox ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
            myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 5, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        private static void ResetChatTextField()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().isShow = false;
        }

        private static void LoadSavedData()
        {
            isSaveData = Rms.loadRMSInt("AutoChatIsSaveRms") == 1;
            if (isSaveData)
            {
                isAutoChatPublic = Rms.loadRMSInt("AutoChatIsAutoChatPublic") == 1;
                isAutoChatGlobal = Rms.loadRMSInt("AutoChatIsAutoChatGlobal") == 1;
                isAutoInbox = Rms.loadRMSInt("AutoChatIsAutoChatInbox") == 1;
                if (isAutoChatPublic)
                {
                    delayAutoChatPublic = long.Parse(Rms.loadRMSString("AutoChatDelayChatPublic"));
                    if (delayAutoChatPublic < 2000)
                    {
                        delayAutoChatPublic = 2000L;
                    }
                }
                if (isAutoChatGlobal)
                {
                    delayAutoChatGlobal = long.Parse(Rms.loadRMSString("AutoChatDelayChatGlobal"));
                    if (delayAutoChatGlobal < 2000)
                    {
                        delayAutoChatGlobal = 2000L;
                    }
                }
            }
        }

        private static void SaveData()
        {
            Rms.saveRMSInt("AutoChatIsAutoChatPublic", isAutoChatPublic ? 1 : 0);
            Rms.saveRMSInt("AutoChatIsAutoChatGlobal", isAutoChatGlobal ? 1 : 0);
            Rms.saveRMSInt("AutoChatIsAutoChatInbox", isAutoInbox ? 1 : 0);
            if (isAutoChatPublic)
            {
                Rms.saveRMSString("AutoChatDelayChatPublic", delayAutoChatPublic.ToString());
            }
            if (isAutoChatGlobal)
            {
                Rms.saveRMSString("AutoChatDelayChatGlobal", delayAutoChatGlobal.ToString());
            }
        }

        private static void ChatPublic()
        {
            if (delayAutoChatPublic < 2000)
            {
                delayAutoChatPublic = 2000L;
            }
            if (autoChatPublicContent == null || autoChatPublicContent.Equals(""))
            {
                autoChatPublicContent = "Cường đẹp trai!";
            }
            if (mSystem.currentTimeMillis() - lastTimeChatPublic > delayAutoChatPublic)
            {
                lastTimeChatPublic = mSystem.currentTimeMillis();
                Service.gI().chat(autoChatPublicContent);
            }
        }

        private static void ChatGlobal()
        {
            int num = Char.myCharz().luong + Char.myCharz().luongKhoa;
            if (num < 5)
            {
                isAutoChatGlobal = false;
                GameScr.info1.addInfo("Bạn không đủ ngọc để chat!", 0);
                return;
            }
            if (delayAutoChatGlobal < 2000)
            {
                delayAutoChatGlobal = 2000L;
            }
            if (autoChatGlobalContent == null || autoChatGlobalContent.Equals(""))
            {
                autoChatGlobalContent = "Cường đẹp trai!";
            }
            if (gems == num && mSystem.currentTimeMillis() - lastTimeChatGlobal > 2000)
            {
                lastTimeChatGlobal = mSystem.currentTimeMillis() - delayAutoChatGlobal - 500;
            }
            if (mSystem.currentTimeMillis() - lastTimeChatGlobal > delayAutoChatGlobal)
            {
                lastTimeChatGlobal = mSystem.currentTimeMillis();
                gems = num;
                Service.gI().chatGlobal(autoChatGlobalContent);
            }
        }

        private static void SpamChatGlobal()
        {
            int num = Char.myCharz().luong + Char.myCharz().luongKhoa;
            if (num < 5 || timesSpamChatGlobal <= 0 || spammedChatGlobalTimes >= timesSpamChatGlobal)
            {
                isAutoSpamChatGlobal = false;
            }
            if (autoChatGlobalContent == null || autoChatGlobalContent.Equals(""))
            {
                autoChatGlobalContent = "Cường đẹp trai!";
            }
            if (gems != num)
            {
                gems = num;
                spammedChatGlobalTimes++;
            }
            if (mSystem.currentTimeMillis() - lastTimeChatGlobal >= 150)
            {
                lastTimeChatGlobal = mSystem.currentTimeMillis();
                countChatGlobal++;
                gems = num;
                Service.gI().chatGlobal(autoChatGlobalContent);
            }
        }

        private static void ChatInbox()
        {
            if (listCharInbox == null)
            {
                listCharInbox = new List<int>();
            }
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (isInboxable(obj) && !listCharInbox.Contains(obj.charID))
                {
                    listCharInbox.Add(obj.charID);
                }
            }
            if (autoInboxContent == null || autoInboxContent.Equals(""))
            {
                autoInboxContent = "Cường đẹp trai!";
            }
            if (listCharInbox.Count > 0 && mSystem.currentTimeMillis() - lastTimeInbox > 2000)
            {
                lastTimeInbox = mSystem.currentTimeMillis();
                countChatInbox++;
                Service.gI().chatPlayer(autoInboxContent, listCharInbox[0]);
                listCharInbox.RemoveAt(0);
            }
        }

        private static bool isInboxable(Char ch)
        {
            if (ch != null && ch.cName != null && !ch.cName.Equals("") && !char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && !ch.isPet && !ch.isMiniPet && !ch.cName.StartsWith("#"))
            {
                return !ch.cName.StartsWith("$");
            }
            return false;
        }

        static AutoChat()
        {
            listCharInbox = new List<int>();
            inputDelayChatPublic = new string[2] { "Nhập delay chat:", "Mili giây (>=2000ms)" };
            inputDelayChatGlobal = new string[2] { "Nhập delay chat thế giới:", "Mili giây (>=2000ms)" };
            inputSpamChatGlobal = new string[2] { "Nhập số lần spam", "Số lần" };
            LoadSavedData();
        }
    }
}