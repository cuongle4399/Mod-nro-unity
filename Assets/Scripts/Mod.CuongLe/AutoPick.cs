using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod.CuongLe
{
    public class AutoPick : IActionListener, IChatable
    {
        private static AutoPick _Instance;

        public static bool isAutoPick;
        public static long lastTimePickedItem;
        private static int maximumPickDistance = 50;
        private static bool isTeleportToItem;
        private static bool isPickAll;
        public static int pickByList;
        private static List<int> listItemAutoPick = new List<int>();

        private static readonly string[] inputMaximumPickDistance = { "Nhập khoảng cách nhặt", "khoảng cách (>50)" };
        private static readonly string[] inputItemID = { "Nhập ID của item", "ID" };

        public static AutoPick getInstance()
        {
            return _Instance ?? (_Instance = new AutoPick());
        }

        public static void Update()
        {
            if (!isAutoPick || (GameScr.isAutoPlay && !(GameScr.canAutoPlay || AutoTrain.isAutoTrain)))
                return;

            long now = mSystem.currentTimeMillis();

            for (int i = 0; i < GameScr.vItemMap.size(); i++)
            {
                ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
                if (item == null || item.template == null) continue;

                bool isOwnerOrFree = item.playerId == Char.myCharz().charID || item.playerId == -1;
                int dx = Math.abs(Char.myCharz().cx - item.x);
                int dy = Math.abs(Char.myCharz().cy - item.y);

                if (isNRDMap(TileMap.mapID))
                {
                    if (isOwnerOrFree && dx <= 60 && now - lastTimePickedItem > 550 && isNRD(item))
                    {
                        Service.gI().pickItem(item.itemMapID);
                        lastTimePickedItem = now;
                        return;
                    }
                }
                else
                {
                    if (isPickIt(item) && dx <= maximumPickDistance && dy <= maximumPickDistance && now - lastTimePickedItem > 550)
                    {
                        if (isTeleportToItem && !Char.isLockKey)
                        {
                            TeleportTo(item.x, item.y);
                        }

                        Service.gI().pickItem(item.itemMapID);
                        lastTimePickedItem = now;
                        return;
                    }
                }
            }
        }

        public void onChatFromMe(string text, string to)
        {
            string input = ChatTextField.gI().tfChat.getText();
            if (string.IsNullOrEmpty(input))
            {
                ChatTextField.gI().isShow = false;
                return;
            }

            if (ChatTextField.gI().strChat.Equals(inputMaximumPickDistance[0]))
            {
                if (int.TryParse(input, out int value))
                {
                    maximumPickDistance = value;
                    GameScr.info1.addInfo("Khoảng Cách Nhặt: " + value, 0);
                }
                else
                {
                    GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputItemID[0]))
            {
                if (int.TryParse(input, out int itemId))
                {
                    listItemAutoPick.Add(itemId);
                    GameScr.info1.addInfo("Đã Thêm Item " + itemId, 0);
                }
                else
                {
                    GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
                }
                ResetChatTextField();
            }
        }

        public void onCancelChat()
        {
            if (GameScr.isPaintMessage)
            {
                GameScr.isPaintMessage = false;
                ChatTextField.gI().center = null;
            }
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    isAutoPick = !isAutoPick;
                    pickByList = 0;
                    GameScr.info1.addInfo("Auto Nhặt\n" + (isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    break;
                case 2:
                    isPickAll = !isPickAll;
                    GameScr.info1.addInfo("Nhặt Tất Cả\n" + (isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    break;
                case 3:
                    isAutoPick = !isAutoPick;
                    pickByList = 1;
                    GameScr.info1.addInfo("Nhặt Theo Danh Sách\n" + (isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    break;
                case 4:
                    isTeleportToItem = !isTeleportToItem;
                    GameScr.info1.addInfo("Dịch Đến Item\n" + (isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    break;
                case 5:
                    ChatTextField.gI().strChat = inputMaximumPickDistance[0];
                    ChatTextField.gI().tfChat.name = inputMaximumPickDistance[1];
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 6:
                    if (listItemAutoPick.Count == 0)
                    {
                        GameScr.info1.addInfo("Danh Sách Trống!", 0);
                    }
                    else
                    {
                        GameScr.info1.addInfo(string.Join(" ", listItemAutoPick.ConvertAll<string>(delegate (int i) { return i.ToString(); }).ToArray()), 0);

                    }
                    break;
                case 7:
                    listItemAutoPick.Clear();
                    GameScr.info1.addInfo("Đã Clear Danh Sách Nhặt!", 0);
                    break;
                case 8:
                    ChatTextField.gI().strChat = inputItemID[0];
                    ChatTextField.gI().tfChat.name = inputItemID[1];
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 9:
                    if (Char.myCharz().itemFocus != null)
                    {
                        listItemAutoPick.Add(Char.myCharz().itemFocus.template.id);
                        GameScr.info1.addInfo("Đã thêm " + Char.myCharz().itemFocus.template.name + " [" + Char.myCharz().itemFocus.template.id + "]", 0);
                    }
                    break;
            }
        }

        public static void ShowMenu()
        {
            MyVector menu = new MyVector();
            menu.addElement(new Command("Auto Nhặt\n" + ((!isAutoPick || pickByList != 0) ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 1, null));
            menu.addElement(new Command("Nhặt Tất Cả\n" + (isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 2, null));
            menu.addElement(new Command("Nhặt Theo Danh Sách\n" + ((!isAutoPick || pickByList != 1) ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 3, null));
            menu.addElement(new Command("Dịch Đến Item\n" + (isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
            menu.addElement(new Command("Khoảng Cách Nhặt\n[" + maximumPickDistance + "]", getInstance(), 5, null));
            menu.addElement(new Command("Xem Danh Sách Nhặt", getInstance(), 6, null));
            menu.addElement(new Command("Clear Danh Sách Nhặt", getInstance(), 7, null));
            menu.addElement(new Command("Thêm ItemID", getInstance(), 8, null));

            if (Char.myCharz().itemFocus != null)
            {
                menu.addElement(new Command("Thêm: " + Char.myCharz().itemFocus.template.name + " [" + Char.myCharz().itemFocus.template.id + "]", getInstance(), 9, null));
            }

            GameCanvas.menu.startAt(menu, 3);
        }

        private static void ResetChatTextField()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().isShow = false;
        }

        public static void FocusToNearestItem()
        {
            if (Char.myCharz().itemFocus != null) return;

            ItemMap nearest = null;
            int minDist = int.MaxValue;

            for (int i = 0; i < GameScr.vItemMap.size(); i++)
            {
                ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
                if (item == null || !isPickIt(item)) continue;

                int dx = Math.abs(Char.myCharz().cx - item.x);
                int dy = Math.abs(Char.myCharz().cy - item.y);

                if (dx <= maximumPickDistance && dy <= maximumPickDistance)
                {
                    int dist = dx * dx + dy * dy;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = item;
                    }
                }
            }

            if (nearest != null)
                Char.myCharz().itemFocus = nearest;
        }

        public static void PickIt()
        {
            if (Char.myCharz().itemFocus == null) return;

            long now = mSystem.currentTimeMillis();
            if (now - lastTimePickedItem < 550) return;

            ItemMap item = Char.myCharz().itemFocus;

            if (isTeleportToItem && !Char.isLockKey)
            {
                TeleportTo(item.x, item.y);
            }

            int dx = Math.abs(Char.myCharz().cx - item.x);
            int dy = Math.abs(Char.myCharz().cy - item.y);

            if (dx <= 40 && dy <= 40)
            {
                Service.gI().pickItem(item.itemMapID);
                lastTimePickedItem = now;
                Char.myCharz().itemFocus = null;
            }
            else
            {
                Char.myCharz().currentMovePoint = new MovePoint(item.x, item.y);
                Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
            }
        }

        private static void TeleportTo(int x, int y)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        private static bool isPickIt(ItemMap item)
        {
            if (item == null || item.template == null) return false;

            if (isPickAll) return true;

            bool isOwnerOrFree = item.playerId == Char.myCharz().charID || item.playerId == -1;

            if (pickByList == 0)
                return isOwnerOrFree;

            return pickByList == 1 && listItemAutoPick.Contains(item.template.id) && isOwnerOrFree;
        }

        private static bool isNRDMap(int mapID)
        {
            return mapID >= 85 && mapID <= 91;
        }

        private static bool isNRD(ItemMap item)
        {
            return item.template.id >= 372 && item.template.id <= 378;
        }
    }
}
