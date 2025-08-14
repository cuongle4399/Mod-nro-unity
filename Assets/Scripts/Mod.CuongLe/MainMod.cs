using Assets.src.e;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using Color = UnityEngine.Color;
using Font = UnityEngine.Font;
using FontStyle = UnityEngine.FontStyle;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.IO;


namespace Mod.CuongLe
{
    public class MainMod : IActionListener, IChatable
    {
    	public static MainMod _Instance;

    	public static int int_0;

    	public static string account;

    	public static string password;

    	public static int int_1;

    	public static int int_2;

    	public static List<int> list_0;

    	public static List<int> list_1;

    	public static string string_2;

    	public static int runSpeed;

    	public static bool isAutoRevive;

    	public static bool isLockFocus;

    	public static int charIDLock;

    	public static string[] inputLockFocusCharID;

    	public static int zoneIdNRD;

    	public static int mapIdNRD;

    	public static bool isOpenMenuNPC;

    	public static bool isAutoEnterNRDMap;

    	public static string[] nameMapsNRD;

    	public static int int_4;

    	public static bool bool_1;

    	public static string[] inputHPPercentFusionDance;

    	public static string[] inputHPFusionDance;

    	public static int minumumHPPercentFusionDance;

    	public static int minimumHPFusionDance;

    	public static long long_0;

    	public static List<int> listCharIDs;

    	public static string[] inputCharID;

    	public static bool isAutoLockControl;

    	public static bool isAutoTeleport;

    	public static long long_1;

    	public static long long_2;

    	public static bool isAutoAttackBoss;

    	public static int HPLimit;

    	public static string[] inputHPLimit;

    	public static long long_3;

    	public static bool isAutoAttackOtherChars;

    	public static int limitHPChar;

    	public static long long_4;

    	public static string[] inputHPChar;

    	public static List<Boss> listBosses;

    	public static Image logoServerListScreen;

    	public static Image logoGameScreen;

    	public static List<Image> listBackgroundImages;

    	public static List<Color> listFlagColor;

    	public static int widthRect;

    	public static int heightRect;

    	public static List<Char> listCharsInMap;

    	public static string string_0;

    	public static bool isUsingSkill;

    	public static long lastTimeConnected;

    	public static bool isUsingCapsule;

    	public static string string_1;

    	public static int delay;

    	public static Image image_0;

    	public static int indexBackgroundImages;

    	public static long lastTimeChangeBackground;

    	public static string string_4;

    	public static string string_3;

    	public static int server;

    	public static string APIKey;

    	public static string APIServer;

    	public static bool isSlovingCapcha;

    	public static int int_3;

    	public static long long_5;

    	public static long long_6;

    	public static bool bool_0;

    	public static long long_7;

    	public static bool isAutoT77;

    	public static bool isAutoBomPicPoc;

    	private static string pathfile;

    	private static string resetLoadMapFile;

    	private static bool RunningGoMapQLTK;

    	private static bool thongBao;

    	public static int time_;

    	public static bool toiUuCPU;

    	public static long GoldCurrent;

    	public static bool infoTrainGold;

    	public static long GoldUpdate;

    	public static long GoldUpdateRealTime;

    	public static int LasterLogin;

    	public static bool loginAgain;

    	public static bool ConnectServer;

    	public static string VersionMod;

        private static int frameCount = 0;

        private static long lastTime = 1000; 

        private static int fps = 0;

    	private static int stopMap = 0;

    	private static bool checkSkill = false;

        private static string statusCapcha;

        public static int countCaptchaSolved = 0;

    	public static bool AutoCapCha;



        public static MainMod getInstance()
    	{
    		if (_Instance == null)
    		{
    			_Instance = new MainMod();
    		}
    		return _Instance;
    	}

    	public static void Update()
    	{
            if (GameScr.isAnalog == 0 && (Application.isMobilePlatform || IsEmulator()))
            {
                GameScr.isAnalog = 1;
            }
            Yardat.update();
            ModProCuongLe.Update();
    		if (thongBao && GameCanvas.gameTick % 1200 == 0)
    		{
                GameScr.gI().chatVip("Mod: Phím X mở menu, Phím V mở menu Pro, [ Shift + A, Shift + Z ], thay set đồ, Shift + U để dừng bất kỳ auto nào");
                thongBao = false;
    		}
    		//if (File.Exists(pathfile) && !RunningGoMapQLTK && GameCanvas.gameTick % 40 == 0)
    		//{
    		//	string[] array = File.ReadAllText(pathfile).Split('|');
    		//	try
    		//	{
    		//		if (array.Length > 1 && array[0] == "T" && int.TryParse(array[1], out var mapId))
    		//		{
    		//			RunningGoMapQLTK = true;
    		//			new Thread((ThreadStart)delegate
    		//			{
    		//				if (mapId == -99)
    		//				{
    		//					if (Char.myCharz().getGender().Equals("NM"))
    		//					{
    		//						mapId = 22;
    		//					}
    		//					else if (Char.myCharz().getGender().Equals("XD"))
    		//					{
    		//						mapId = 23;
    		//					}
    		//					else
    		//					{
    		//						mapId = 21;
    		//					}
    		//				}
    		//				AutoMap.StartRunToMapId(mapId);
    		//				while (TileMap.mapID != mapId)
    		//				{
    		//					Thread.Sleep(1000);
    		//				}
    		//				File.WriteAllText(pathfile, resetLoadMapFile);
    		//				RunningGoMapQLTK = false;
    		//			}).Start();
    		//		}
    		//	}
    		//	catch
    		//	{
    		//	}
    		//}
    		if ((!MobCapcha.isAttack || !MobCapcha.explode) && GameScr.gI().mobCapcha != null && AutoCapCha)
    		{
    			if (!isSlovingCapcha && GameCanvas.gameTick % 100 == 0)
    			{
    				isSlovingCapcha = true;
    				new Thread(SolveCapcha).Start();
    			}
    			return;
    		}
    		if (DoHoa.isShowCharsInMap)
    		{
    			listCharsInMap.Clear();
    			for (int num = 0; num < GameScr.vCharInMap.size(); num++)
    			{
    				Char obj2 = (Char)GameScr.vCharInMap.elementAt(num);
    				if (obj2.cName != null && obj2.cName != "" && !obj2.isPet && !obj2.isMiniPet && !obj2.cName.StartsWith("#") && !obj2.cName.StartsWith("$") && obj2.cName != "Trọng tài")
    				{
    					listCharsInMap.Add(obj2);
    				}
    			}
    		}
    		if (isAutoEnterNRDMap)
    		{
    			EnterNRDMap();
    		}
    		if (isAutoRevive)
    		{
    			Revive();
    		}
    		if (isLockFocus)
    		{
    			FocusTo(charIDLock);
    		}
    		AutoItem.Update();
    		AutoChat.Update();
    		AutoPean.Update();
    		AutoSkill.Update();
    		AutoTrain.Update();
    		AutoPick.Update();
    		AutoMap.Update();
    		AutoPoint.Update();
    		Char.myCharz().cspeed = runSpeed;
    	}


        public static int CalculateFPS()
        {
            frameCount++;
            long currentTime = mSystem.currentTimeMillis();

            long deltaTime = currentTime - lastTime;

            if (deltaTime >= 500) 
            {
                fps = (int)(frameCount * 1000.0 / deltaTime + 0.5); 
                frameCount = 0;
                lastTime = currentTime;
            }

            return fps;
        }

        public static void Paint(mGraphics g)
    	{

            DoHoa.Paint(g);
            paintListBosses(g);
    		//if (DoHoa.isShowCharsInMap)
    		//{
    		//	paintListCharsInMap(g);
    		//}
    		if (DoHoa.isShowMenuVIP)
    		{
    			UnifiedAutoMenu.Paint(g);
    			return;
            }
            int num2 = 8;
            int num = GameCanvas.h - 170;
            DoHoa.DrawFont.drawString(g, TileMap.mapNames[TileMap.mapID] + " [" + TileMap.mapID + "] Zone: " + TileMap.zoneID, 10, num, 0);
            num += num2;
            DoHoa.DrawFont.drawString(g, "x: " + Char.myCharz().cx + " y: " + Char.myCharz().cy, 10, num, 0);
            num += num2;

            // Vẽ FPS
            string fpsText = "FPS: " + CalculateFPS().ToString(); // Làm tròn FPS
            DoHoa.DrawFont.drawString(g, fpsText, GameScr.imgPanel.getWidth()/2 + GameScr.imgPanel.getWidth() *10 /100, GameScr.imgPanel.getHeight()- GameScr.imgPanel.getHeight()* 30/100, 0); 
            if (ModProCuongLe.DoBoss)
    		{
    			DoHoa.DrawFont.drawString(g, "Bắt đầu tìm khu boss từ " + TileMap.zoneID + "-> 14", 10, num, 0);
            
                num += num2;
    		}
            if (AutoSkill.isAutoSendAttack)
    		{
    			DoHoa.DrawFont.drawString(g, "Tự đánh: on", 10, num, 0);
    			num += num2;
    		}
    		if (isAutoRevive)
    		{
    			DoHoa.DrawFont.drawString(g, "Hồi sinh: on", 10, num, 0);
    			num += num2;
    		}
            if (isSlovingCapcha)
            {
                DoHoa.DrawFont.drawString(g, statusCapcha, 10, num, 0);
                num += num2;
                DoHoa.DrawFont.drawString(g, "Số lần giải Capcha thành công: "+ countCaptchaSolved.ToString(), 10, num, 0);
                num += num2;
            }
            if (ModProCuongLe.DoSatBossNapa)
    		{
                DoHoa.DrawFont.drawString(g, "Auto Farm Boss Nappa: on", 10, num, 0);
                num += num2;
            }
            if (ModProCuongLe.findBossMod)
    		{
                DoHoa.DrawFont.drawString(g, "Auto Map Trứng Mabu: on", 10, num, 0);
                num += num2;
            }
            if (ConnectServer)
    		{
    			DoHoa.DrawFont.drawString(g, "Đã kết nối", 10, num, 0);
    			num += num2;
    		}
    		if (AutoPick.isAutoPick)
    		{
    			DoHoa.DrawFont.drawString(g, "Auto nhặt: on", 10, num, 0);
    			num += num2;
    		}
            if (ModProCuongLe.aGimBoss)
            {
                DoHoa.DrawFont.drawString(g, "Auto gim Boss:  on", 10, num, 0);
                num += num2;
            }
            if (ModProCuongLe.AutoteleBoss)
            {
                DoHoa.DrawFont.drawString(g, "Auto tele Boss:  on", 10, num, 0);
                num += num2;
            }
        
            if (ModProCuongLe.tanCongBoss)
            {
                DoHoa.DrawFont.drawString(g, "Auto tan cong Boss:  on", 10, num, 0);
                num += num2;
            }
            if (AutoboMong.autoboMong)
    		{
    			DoHoa.DrawFont.drawString(g, "Đang tiến hành Bò Mộng (SHIFT + Y)", 10, num, 0);
    			num += num2;
    		}
    		if (isLockFocus)
    		{
    			DoHoa.DrawFont.drawString(g, "Khóa: " + charIDLock, 10, num, 0);
    			num += num2;
    		}
    		if (isAutoEnterNRDMap)
    		{
    			DoHoa.DrawFont.drawString(g, "Đang auto nrd: " + mapIdNRD + "sk" + zoneIdNRD, 15, num, 0);
    			num += num2;
    		}
    		if (ModProCuongLe.charw)
    		{
    			infoView(g, Char.myCharz(), 10, num);
    			num += num2 * 3;
    		}
    		if (ModProCuongLe.petw)
    		{
    			infoView(g, Char.myPetz(), 10, num);
    			num += num2 * 5;
    		}
    		if (infoTrainGold)
    		{
    			infoTrain(g, 10, num);
    			num += num2 * 3;
    		}
    		if (ModProCuongLe.hienThiDoKH)
    		{
    			DoHoa.DrawFont.drawString(g, ShowSetKH.InDanhSachDoKH(), 10, num, 0);
    			if (string.IsNullOrEmpty(ShowSetKH.InDanhSachDoKH()))
    			{
    				GameScr.info1.addInfo("Nick bạn deo co sét kích hoạt ok", 0);
    				ModProCuongLe.hienThiDoKH = false;
    			}
    			else
    			{
    				num += num2 * 6;
    			}
    		}
    		if (ModProCuongLe.banDo)
    		{
    			DoHoa.DrawFont.drawString(g, "Đang auto bán đồ khi full", 10, num, 0);
    			num += num2;
    		}
    		if (ModProCuongLe.catDoVIP)
    		{
    			DoHoa.DrawFont.drawString(g, "Đang auto cất đồ vào rương khi full", 10, num, 0);
    			num += num2;
    		}
    		if (AutoMap.isXmaping)
    		{
                int xButtonMap = GameCanvas.w / 2 - GameScr.imgLbtn.getWidth() / 2;
                int yButtonMap = GameCanvas.h / 2 - GameScr.imgLbtn.getHeight() / 2;
                g.drawImage((stopMap != 1) ? GameScr.imgLbtn : GameScr.imgLbtnFocus, xButtonMap, yButtonMap);
                int centerX = xButtonMap + GameScr.imgLbtn.getWidth() / 2;
                int centerY = yButtonMap + GameScr.imgLbtn.getHeight() / 2 -3;
                mFont.tahoma_7b_dark.drawString(g, "Stop Xmap", centerX, centerY, mFont.CENTER);
    			string s = "Đang tới: " + TileMap.mapNames[AutoMap.IdMapEnd];
    			if (AutoMap.xmapErrr)
    			{
    				s = "Bạn chưa thể tới: " + TileMap.mapNames[AutoMap.IdMapEnd];
                }
                DoHoa.DrawFont.drawString(g, s, centerX, yButtonMap - 12, mFont.CENTER);
    		}
    	}

    	public static void paintListCharsInMap(mGraphics g)
    	{
    		int num = (isMeInNRDMap() ? 35 : 95);
    		widthRect = 120;
    		heightRect = 7;
    		GUIStyle gUIStyle = new GUIStyle();
    		gUIStyle.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
    		gUIStyle.fontSize = 7 * mGraphics.zoomLevel;
    		gUIStyle.fontStyle = UnityEngine.FontStyle.Bold;
    		gUIStyle.alignment = TextAnchor.UpperLeft;
    		for (int i = 0; i < listCharsInMap.Count; i++)
    		{
    			Char obj = listCharsInMap[i];
    			g.setColor(2721889, 0.5f);
    			g.fillRect(GameCanvas.w - widthRect, num + 1, widthRect - 2, heightRect);
    			if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$") && obj.cName != "Trọng tài")
    			{
                    paintPKFlag(g, obj, num);
                    if (obj.isNRD)
    				{
    					paintCharInfo(g, obj);
    				}
    				string text = obj.cName + " [" + NinjaUtil.getMoneys(obj.cHP) + "]";
    				bool flag = isBoss(obj);
    				if (!flag)
    				{
    					text = obj.cName + " [" + NinjaUtil.getMoneys(obj.cHP) + " - " + obj.getGender() + "]";
    				}
    				GUIStyle gUIStyle2 = new GUIStyle(gUIStyle);
    				if (Char.myCharz().charFocus != null && Char.myCharz().charFocus.cName == obj.cName)
    				{
    					g.setColor(14155776);
    					g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, obj.cx - GameScr.cmx, obj.cy - GameScr.cmy);
    					gUIStyle2.normal.textColor = Color.red;
    				}
    				else if (flag)
    				{
    					g.setColor(16383818);
    					g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, obj.cx - GameScr.cmx, obj.cy - GameScr.cmy);
    					gUIStyle2.normal.textColor = Color.yellow;
    				}
    				else if (obj.cHPFull > 100000000 && obj.cHP > 0 && isMeInNRDMap() && !obj.isNRD)
    				{
    					gUIStyle2.normal.textColor = Color.magenta;
    				}
    				else
    				{
    					gUIStyle2.normal.textColor = Color.black;
    				}
    				g.drawString(i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, gUIStyle2);
    				num += heightRect + 1;
    			}
    		}
    	}

    	public static void paintCharInfo(mGraphics g, Char ch)
    	{
    		GUIStyle gUIStyle = new GUIStyle();
    		gUIStyle.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    		gUIStyle.fontSize = 6 * mGraphics.zoomLevel;
    		gUIStyle.fontStyle = FontStyle.Bold;
    		gUIStyle.normal.textColor = Color.black;
            int num = 72;
            int num2 = 6;
            g.drawString(ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "]", GameCanvas.w / 2, num, gUIStyle);
            num += num2;
            if (ch.isNRD)
    		{
    			mFont.tahoma_7b_yellow.drawString(g, "Còn: " + ch.timeNRD + " giây", GameCanvas.w / 2, num, 2);
    			num += num2;
    		}
    		if (ch.isFreez)
    		{
    			mFont.tahoma_7b_yellow.drawString(g, "Bị TDHS: " + ch.freezSeconds + " giây", GameCanvas.w / 2, num, 2);
    			num += num2;
    		}
    	}

    	public static void smethod_15(mGraphics g, int x, int y)
    	{
    	}

    	public static void paintListBosses(mGraphics g)
    	{
    		if (DoHoa.isHuntingBoss && !isMeInNRDMap())
    		{
    			int num = 42;
    			for (int i = 0; i < listBosses.Count; i++)
    			{
    				listBosses[i].Paint(g, GameCanvas.w - 2, num, mFont.RIGHT);
    				num += 10;
    			}
    		}
    	}

    	public void onChatFromMe(string text, string to)
    	{
            if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
    		{
    			ChatTextField.gI().isShow = false;
    			ResetChatTextField();
    		}
    		else if (ChatTextField.gI().strChat.Equals(inputLockFocusCharID[0]))
    		{
    			try
    			{
    				int num = (charIDLock = int.Parse(ChatTextField.gI().tfChat.getText()));
    				isLockFocus = true;
    				GameScr.info1.addInfo("Đã Thêm: " + num, 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại", 0);
    			}
    			ResetChatTextField();
    		}
    		else if (ChatTextField.gI().strChat.Equals(inputHPFusionDance[0]))
    		{
    			try
    			{
    				int num2 = (minimumHPFusionDance = int.Parse(ChatTextField.gI().tfChat.getText()));
    				GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + Res.formatNumber2(num2), 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
    			}
    			ResetChatTextField();
    		}
    		else if (ChatTextField.gI().strChat.Equals(inputCharID[0]))
    		{
    			try
    			{
    				int item = int.Parse(ChatTextField.gI().tfChat.getText());
    				listCharIDs.Add(item);
    				GameScr.info1.addInfo("Đã Thêm: " + item, 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
    			}
    			ResetChatTextField();
    		}
    		else if (ChatTextField.gI().strChat.Equals(inputHPLimit[0]))
    		{
    			try
    			{
    				int num3 = (HPLimit = int.Parse(ChatTextField.gI().tfChat.getText()));
    				GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num3) + " HP", 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
    			}
    			ResetChatTextField();
    		}
    		else if (ChatTextField.gI().strChat.Equals(inputHPChar[0]))
    		{
    			try
    			{
    				int num4 = (limitHPChar = int.Parse(ChatTextField.gI().tfChat.getText()));
    				GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num4) + " HP", 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
    			}
    			ResetChatTextField();
    		}
    		else
    		{
    			if (!ChatTextField.gI().strChat.Equals(inputHPPercentFusionDance[0]))
    			{
    				return;
    			}
    			try
    			{
    				int num5 = int.Parse(ChatTextField.gI().tfChat.getText());
    				if (num5 > 99)
    				{
    					num5 = 99;
    				}
    				minumumHPPercentFusionDance = num5;
    				GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + num5 + "%", 0);
    			}
    			catch
    			{
    				GameScr.info1.addInfo("%HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
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
    			AutoMap.ShowMenu();
    			break;
    		case 2:
    			AutoSkill.ShowMenu();
    			break;
    		case 3:
    			AutoPean.ShowMenu();
    			break;
    		case 4:
    			AutoPick.ShowMenu();
    			break;
    		case 5:
    			AutoTrain.ShowMenu();
    			break;
    		case 6:
    			AutoChat.ShowMenu();
    			break;
    		case 7:
    			AutoPoint.ShowMenu();
    			break;
    		case 8:
    			ShowMenuMore();
    			break;
    		case 9:
    			if (minumumHPPercentFusionDance > 0)
    			{
    				minumumHPPercentFusionDance = 0;
    				GameScr.info1.addInfo("Hợp thể khi HP dưới: 0% HP", 0);
    			}
    			else
    			{
    				ChatTextField.gI().strChat = inputHPPercentFusionDance[0];
    				ChatTextField.gI().tfChat.name = inputHPPercentFusionDance[1];
    				ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			}
    			break;
    		case 10:
    			if (minimumHPFusionDance > 0)
    			{
    				minimumHPFusionDance = 0;
    				GameScr.info1.addInfo("Hợp thể khi HP dưới: 0", 0);
    			}
    			else
    			{
    				ChatTextField.gI().strChat = inputHPFusionDance[0];
    				ChatTextField.gI().tfChat.name = inputHPFusionDance[1];
    				ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			}
    			break;
    		case 11:
    			smethod_2();
    			break;
    		case 12:
    			isAutoLockControl = !isAutoLockControl;
    			GameScr.info1.addInfo("Auto Khống Chế\n" + (isAutoLockControl ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			break;
    		case 13:
    			isAutoTeleport = !isAutoTeleport;
    			GameScr.info1.addInfo("Auto Teleport\n" + (isAutoTeleport ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			break;
    		case 14:
    			smethod_3();
    			break;
    		case 15:
    			ChatTextField.gI().strChat = inputCharID[0];
    			ChatTextField.gI().tfChat.name = inputCharID[1];
    			ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			break;
    		case 16:
    		{
    			int num2 = (int)p;
    			if (num2 != 0)
    			{
    				listCharIDs.Add(num2);
    				GameScr.info1.addInfo("Đã Thêm: " + num2, 0);
    			}
    			break;
    		}
    		case 17:
    		{
    			int num = (int)p;
    			if (num != 0)
    			{
    				listCharIDs.Remove(num);
    				GameScr.info1.addInfo("Đã Xóa: " + num, 0);
    			}
    			break;
    		}
    		case 18:
    			smethod_0();
    			break;
    		case 19:
    			isAutoAttackBoss = !isAutoAttackBoss;
    			GameScr.info1.addInfo("Tấn Công Boss\n" + (isAutoAttackBoss ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			break;
    		case 20:
    			ChatTextField.gI().strChat = inputHPLimit[0];
    			ChatTextField.gI().tfChat.name = inputHPLimit[1];
    			ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			break;
    		case 21:
    			smethod_1();
    			break;
    		case 22:
    			isAutoAttackOtherChars = !isAutoAttackOtherChars;
    			GameScr.info1.addInfo("Tàn Sát Người\n" + (isAutoAttackOtherChars ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			break;
    		case 23:
    			ChatTextField.gI().strChat = inputHPChar[0];
    			ChatTextField.gI().tfChat.name = inputHPChar[1];
    			ChatTextField.gI().startChat2(getInstance(), string.Empty);
    			break;
    		case 24:
    			MucTieu.ShowMenu();
    			break;
    		case 25:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 26:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 27:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 28:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 29:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 30:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 31:
    			GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
    			break;
    		case 32:
    			isAutoT77 = !isAutoT77;
    			GameScr.info1.addInfo("Auto T77\n" + (isAutoT77 ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
    			break;
    		case 33:
    			isAutoBomPicPoc = !isAutoBomPicPoc;
    			GameScr.info1.addInfo("Auto Bom\nPic Poc" + (isAutoBomPicPoc ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
    			break;
    		case 34:
    			ModProCuongLe.ShowMenu();
    			break;
    		case 100:
    			Application.OpenURL("https://www.facebook.com/cuongle1002/");
    			break;
    		case 103:
    			Application.OpenURL("https://electroheavenvn.github.io/DataNRO/TeaMobi/");
    			break;
    		}
    	}

    	public static bool UpdateKey(int unused)
    	{
    		if (GameCanvas.keyAsciiPress == Hotkeys.A)
    		{
    			AutoSkill.isAutoSendAttack = !AutoSkill.isAutoSendAttack;
    			GameScr.info1.addInfo("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.B)
    		{
    			Service.gI().friend(0, -1);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.C)
    		{
    			for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
    			{
    				Item item = Char.myCharz().arrItemBag[i];
    				if (item != null && (item.template.id == 194 || item.template.id == 193))
    				{
    					Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
    					break;
    				}
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.D)
    		{
    			AutoSkill.FreezeSelectedSkill();
    			return true;
    		}
            if (GameCanvas.keyAsciiPress == Hotkeys.E)
    		{
    			isAutoRevive = !isAutoRevive;
    			GameScr.info1.addInfo("Auto Hồi Sinh\n" + (isAutoRevive ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.F)
    		{
    			if (ModProCuongLe.ExistItemBag(921))
    			{
                    UseItem(921);
                    return true;
                }
                if (ModProCuongLe.ExistItemBag(454))
                {
                    UseItem(454);
                    return true;
                }

                return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.G)
    		{
    			if (Char.myCharz().charFocus == null)
    			{
    				GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!", 0);
    			}
    			else
    			{
    				Service.gI().giaodich(0, Char.myCharz().charFocus.charID, -1, -1);
    				GameScr.info1.addInfo("Đã Gửi Lời Mời Giao Dịch Đến: " + Char.myCharz().charFocus.cName, 0);
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.I)
    		{
    			isLockFocus = !isLockFocus;
    			if (!isLockFocus)
    			{
    				GameScr.info1.addInfo("Khoá Mục Tiêu\n[STATUS: OFF]", 0);
    			}
    			else if (Char.myCharz().charFocus == null)
    			{
    				GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!", 0);
    			}
    			else
    			{
    				charIDLock = Char.myCharz().charFocus.charID;
    				GameScr.info1.addInfo("Đã Khóa: " + Char.myCharz().charFocus.cName, 0);
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.J)
    		{
    			AutoMap.LoadMapLeft();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.K)
    		{
    			AutoMap.LoadMapCenter();
    			return true;
    		}
            if (GameCanvas.keyAsciiPress == Hotkeys.L)
    		{
    			AutoMap.LoadMapRight();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.M)
    		{
    			Service.gI().openUIZone();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.N)
    		{
    			if (isMeInNRDMap())
    			{
    				AutoPick.isAutoPick = !AutoPick.isAutoPick;
    				GameScr.info1.addInfo("Auto Nhặt\n" + (AutoPick.isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			}
    			else
    			{
    				AutoPick.ShowMenu();
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.O)
    		{
    			isAutoEnterNRDMap = !isAutoEnterNRDMap;
    			isOpenMenuNPC = true;
    			GameScr.info1.addInfo("Auto Vào NRD\n" + (isAutoEnterNRDMap ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
    			return true;
    		}
            if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_O)
            {
    			string text = "";
                for (int j = 0; j < Char.vItemTime.size(); j++)
                {
    				text+=((ItemTime)Char.vItemTime.elementAt(j)).idIcon.ToString() + '|';

                }
                ChatPopup.addChatPopupMultiLineGameline(text, 0,null,10);
                return true;
            }

            if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_C)
    		{
    			toiUuCPU = !toiUuCPU;
    			if (toiUuCPU)
    			{
    				GameScr.info1.addInfo("Tối ưu cpu", 0);
    				QualitySettings.vSyncCount = 0;
    				Application.targetFrameRate = 10;
    			}
    			else
    			{
    				GameScr.info1.addInfo("Đã tắt tối ưu cpu", 0);
    				QualitySettings.vSyncCount = 0;
    				Application.targetFrameRate = 90;
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.T)
    		{
    			UseItem(521);
    			return true;
    		}
            if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_T)
            {
    			string s = "";
    			 for (int j = 0; j < Char.vItemTime.size(); j++)
                    {
    					s += ((ItemTime)Char.vItemTime.elementAt(j)).idIcon.ToString()  + '|';
                    }
    			ChatPopup.addChatPopupMultiLineGameline(s, 0, null, 10);
                return true;
            }
            if (GameCanvas.keyAsciiPress == Hotkeys.X)
    		{
    			ShowMenu();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.V)
    		{
    			ModProCuongLe.ShowMenu();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.Z)
    		{
    			if (Char.myCharz().cFlag == 0)
    			{
    				Service.gI().getFlag(1, 8);
    				GameScr.info1.addInfo("Đã gửi yêu cầu bật cờ đen", 0);
    			}
    			else
    			{
    				Service.gI().getFlag(1, 0);
    				GameScr.info1.addInfo("Đã gửi yêu cầu tắt cờ", 0);
    			}
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.S) 

            {
                ModProCuongLe.aGimBoss = !ModProCuongLe.aGimBoss;
    			new Thread(ModProCuongLe.AutoFocusBoss).Start();
    			GameScr.info1.addInfo("Auto gim boss: " + (ModProCuongLe.aGimBoss ? "Bật" : "Tắt"), 0);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_A)
    		{
    			AutoItem.useSet(0);
    			GameScr.info1.addInfo("Đã mặc sét 1", 0);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_Z)
    		{
    			AutoItem.useSet(1);
    			GameScr.info1.addInfo("Đã mặc sét 2", 0);
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_P)
    		{
    			outConnect();
    			return true;
    		}
    		if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_U)
    		{
    			if (ModProCuongLe.findBossMod)
    			{
                    ModProCuongLe.findBossMod = false;
                }
    			if (ModProCuongLe.tanCongBoss)
    			{
    				ModProCuongLe.tanCongBoss = false;
    				ModProCuongLe.listBossTrongKhu.Clear();
    			}
    			if (ModProCuongLe.tieuDietNguoiBatCo)
    			{
    				ModProCuongLe.tieuDietNguoiBatCo = false;
    				ModProCuongLe.listNguoiCoDen.Clear();
    			}
    			if (ModProCuongLe.DoSatBossNapa) {
    				ModProCuongLe.DoSatBossNapa = false;
    			}
    			if (ModProCuongLe.aWhis)
    			{
    				ModProCuongLe.aWhis = false;
    			}
    			if (AutoboMong.autoboMong)
    			{
                    AutoboMong.autoboMong = false;
                    AutoboMong.trainVang = false;
                    AutoboMong.killCharing = false;
                    AutoboMong.trainning = false;
                    AutoTrain.isGoBack = false;
                    InfoMe.FinishBoMong = false;
                    AutoboMong.skipDone = false;
                    AutoMap.isEatChicken = true;
                    AutoPick.isAutoPick = false;
                    AutoPick.pickByList = 0;
                }
    			return true;
    		}
    		return true;
    	}

    	public static void ShowMenu()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Auto Map", getInstance(), 1, null));
    		myVector.addElement(new Command("Auto Pro Cuong Le", getInstance(), 34, null));
    		myVector.addElement(new Command("Auto Skill", getInstance(), 2, null));
    		myVector.addElement(new Command("Auto Pean", getInstance(), 3, null));
    		myVector.addElement(new Command("Auto Pick", getInstance(), 4, null));
    		myVector.addElement(new Command("Auto Train", getInstance(), 5, null));
            myVector.addElement(new Command("Mục Tiêu", getInstance(), 24, null));
            myVector.addElement(new Command("More", getInstance(), 8, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public static void ShowMenuMore()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Auto Point", getInstance(), 7, null));
    		myVector.addElement(new Command("Auto Chat", getInstance(), 6, null));
    		myVector.addElement(new Command("Facebook Cường Lê", getInstance(), 100, null));
    		myVector.addElement(new Command("Web xem id vật phẩm", getInstance(), 103, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public static void smethod_0()
    	{
    	}

    	public static void smethod_1()
    	{
    	}

    	public static void smethod_2()
    	{
    	}

    	public static void smethod_3()
    	{
    	}

    	public static void ResetChatTextField()
    	{
    		ChatTextField.gI().strChat = "Chat";
    		ChatTextField.gI().tfChat.name = "chat";
    		ChatTextField.gI().isShow = false;
    	}

    	public static void UseItem(int templateId)
    	{
    		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
    		{
    			Item item = Char.myCharz().arrItemBag[i];
    			if (item != null && item.template.id == templateId)
    			{
    				Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
    				break;
    			}
    		}
    	}

    	public static void TeleportTo(int x, int y)
    	{
    		Char.myCharz().cx = x;
    		Char.myCharz().cy = y;
    		Service.gI().charMove();
    		Char.myCharz().cx = x;
    		Char.myCharz().cy = y + 1;
    		Service.gI().charMove();
    		Char.myCharz().cx = x;
    		Char.myCharz().cy = y;
    		Service.gI().charMove();
    	}

    	public static int GetYGround(int x)
    	{
    		int num = 50;
    		int num2 = 0;
    		while (num2 < 30)
    		{
    			num2++;
    			num += 24;
    			if (TileMap.tileTypeAt(x, num, 2))
    			{
    				if (num % 24 != 0)
    				{
    					num -= num % 24;
    				}
    				break;
    			}
    		}
    		return num;
    	}

    	static MainMod()
    	{
    		account = "";
    		listFlagColor = new List<Color>();
    		listCharsInMap = new List<Char>();
    		string_0 = "https://www.facebook.com/cuongle1002/";
    		pathfile = "Data/LoadMap.ini";
    		resetLoadMapFile = "F|-1";
    		thongBao = true;
    		GoldCurrent = 0L;
    		GoldUpdate = 0L;
    		GoldUpdateRealTime = 0L;
    		VersionMod = "2.6";
    		DoHoa.isHuntingBoss = true;
    		listBosses = new List<Boss>();
    		listBackgroundImages = new List<Image>();
    		limitHPChar = -1;
    		inputHPChar = new string[2] { "Nhập HP Char:", "HP" };
    		inputHPLimit = new string[2] { "Nhập HP:", "HP" };
    		listCharIDs = new List<int>();
    		inputCharID = new string[2] { "Nhập charID:", "charID" };
    		inputHPPercentFusionDance = new string[2] { "Nhập %HP ", "%HP" };
    		inputHPFusionDance = new string[2] { "Nhập HP", "HP" };
    		nameMapsNRD = new string[7] { "Hành tinh M-2", "Hành tinh Polaris", "Hành tinh Cretaceous", "Hành tinh Monmaasu", "Hành tinh Rudeeze", "Hành tinh Gelbo", "Hành tinh Tigere" };
    		inputLockFocusCharID = new string[2] { "Nhập charID lock", "charID" };
    		list_0 = new List<int>();
    		list_1 = new List<int>();
    		string_2 = "2.0.6 - 06/04/2022 00:00:00";
    		runSpeed = 7;
    	}

    	public static void Revive()
    	{
    		if (Char.myCharz().luong + Char.myCharz().luongKhoa > 0 && Char.myCharz().meDead && Char.myCharz().cHP <= 0 && GameCanvas.gameTick % 20 == 0)
    		{
    			Service.gI().wakeUpFromDead();
    			Char.myCharz().meDead = false;
    			Char.myCharz().statusMe = 1;
    			Char.myCharz().cHP = Char.myCharz().cHPFull;
    			Char.myCharz().cMP = Char.myCharz().cMPFull;
    			Char obj = Char.myCharz();
    			Char obj2 = Char.myCharz();
    			Char.myCharz().cp3 = 0;
    			obj2.cp2 = 0;
    			obj.cp1 = 0;
    			ServerEffect.addServerEffect(109, Char.myCharz(), 2);
    			GameScr.gI().center = null;
    			GameScr.isHaveSelectSkill = true;
    		}
    	}

    	public static void FocusTo(int charId)
    	{
    		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
    		{
    			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
    			if (!obj.isMiniPet && !obj.isPet && obj.charID == charId)
    			{
    				Char.myCharz().mobFocus = null;
    				Char.myCharz().npcFocus = null;
    				Char.myCharz().itemFocus = null;
    				Char.myCharz().charFocus = obj;
    				break;
    			}
    		}
    	}

    	public static bool isMeInNRDMap()
    	{
    		if (TileMap.mapID >= 85)
    		{
    			return TileMap.mapID <= 91;
    		}
    		return false;
    	}

    	public static void smethod_4()
    	{
    	}

    	public static void smethod_5()
    	{
    	}

    	public static bool smethod_6(int int_0)
    	{
    		return list_0.Contains(int_0);
    	}

    	public static void TeleportToFocus()
    	{
    		if (Char.myCharz().charFocus != null)
    		{
    			TeleportTo(Char.myCharz().charFocus.cx, Char.myCharz().charFocus.cy);
    		}
    		else if (Char.myCharz().itemFocus != null)
    		{
    			TeleportTo(Char.myCharz().itemFocus.x, Char.myCharz().itemFocus.y);
    		}
    		else if (Char.myCharz().mobFocus != null)
    		{
    			TeleportTo(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
    		}
    		else if (Char.myCharz().npcFocus != null)
    		{
    			TeleportTo(Char.myCharz().npcFocus.cx, Char.myCharz().npcFocus.cy - 3);
    		}
    		else
    		{
    			GameScr.info1.addInfo("Không Có Mục Tiêu!", 0);
    		}
    	}

    	public static bool isBoss(Char ch)
    	{
    		if (ch.cName != null && ch.cName != "" && !ch.isPet && !ch.isMiniPet && char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && ch.cName != "Trọng tài" && !ch.cName.StartsWith("#"))
    		{
    			return !ch.cName.StartsWith("$");
    		}
    		return false;
    	}

    	public static void EnterNRDMap()
    	{
    		if (isOpenMenuNPC && (TileMap.mapID == 24 || TileMap.mapID == 25 || TileMap.mapID == 26) && GameCanvas.gameTick % 20 == 0)
    		{
    			Service.gI().openMenu(29);
    			Service.gI().confirmMenu(29, 1);
    			if (GameCanvas.panel.mapNames != null && GameCanvas.panel.mapNames.Length > 6 && GameCanvas.panel.mapNames[mapIdNRD - 1] == nameMapsNRD[mapIdNRD - 1])
    			{
    				Service.gI().requestMapSelect(mapIdNRD - 1);
    				isOpenMenuNPC = false;
    			}
    		}
    		if (isMeInNRDMap() && !Char.isLoadingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 20 == 0)
    		{
    			Service.gI().requestChangeZone(zoneIdNRD, -1);
    			isAutoEnterNRDMap = false;
    			isOpenMenuNPC = true;
    		}
    	}

    	public static void smethod_7()
    	{
    	}

    	public static void smethod_8()
    	{
    	}

    	public static int MyHPPercent()
    	{
    		return (int)(Char.myCharz().cHP * 100 / Char.myCharz().cHPFull);
    	}
    	public static void updateTouch()
    	{
            if (AutoMap.isXmaping && GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - GameScr.imgLbtn.getWidth() - 20, GameCanvas.h / 2 - GameScr.imgLbtn.getHeight()- 20, GameScr.imgLbtn.getWidth() + 60, GameScr.imgLbtn.getHeight()+ 60) && GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
            {
    			stopMap = 1;
                AutoMap.FinishXmap();
                SoundMn.gI().buttonClick();
                GameCanvas.clearAllPointerEvent();
    			if (AutoTrain.isGoBack)
    			{
                    AutoTrain.isGoBack = false;

                }
    			stopMap = 0;
            }
            if (DoHoa.isShowCharsInMap)
            {
                MainMod.controlTeleChar();
            }
            if (GameCanvas.isPointerHoldIn(GameScr.imgPanel.getWidth() + 5, 3, DoHoa.imgSetting.getWidth() + 5, DoHoa.imgSetting.getHeight())
                && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                MainMod.ShowMenu();
                GameCanvas.clearAllPointerEvent();
                return;
            }
            // Khi click nút mở menu VIP
            if (GameCanvas.isPointerHoldIn(GameScr.imgPanel.getWidth() + 5 + DoHoa.imgSetting.getWidth() + 5, 3, DoHoa.imgBoxOpenItem.getWidth() + 5, DoHoa.imgBoxOpenItem.getHeight())
                && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                DoHoa.isShowMenuVIP = !DoHoa.isShowMenuVIP;
                GameCanvas.clearAllPointerEvent();
                return;
            }

            // Nếu menu đang bật
            if (DoHoa.isShowMenuVIP)
            {
                int panelX = UnifiedAutoMenu.PanelX;
                int panelY = UnifiedAutoMenu.PanelY;
                int panelW = UnifiedAutoMenu.PanelWidth;
                int panelH = UnifiedAutoMenu.PanelHeight;

                // Nếu click bên trong menu thì xử lý click menu
                if (GameCanvas.isPointerHoldIn(panelX, panelY, panelW, panelH))
                {
                    UnifiedAutoMenu.HandleClick();
                   //GameCanvas.clearAllPointerEvent();
                }
                else if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    // Click ngoài menu => tắt menu
                    DoHoa.isShowMenuVIP = false;
                    GameCanvas.clearAllPointerEvent();
                }
            }


        }
        public static bool isMyHPLowerThan(int percent)
    	{
    		if (Char.myCharz().cHP > 0)
    		{
    			return MyHPPercent() < percent;
    		}
    		return false;
    	}


    	public static string GetFlagName(int flagId)
    	{
    		if (flagId == -1 || flagId == 0)
    		{
    			return "";
    		}
    		string text = "";
    		switch (flagId)
    		{
    		case 1:
    			text = "Cờ xanh";
    			break;
    		case 2:
    			text = "Cờ đỏ";
    			break;
    		case 3:
    			text = "Cờ tím";
    			break;
    		case 4:
    			text = "Cờ vàng";
    			break;
    		case 5:
    			text = "Cờ lục";
    			break;
    		case 6:
    			text = "Cờ hồng";
    			break;
    		case 7:
    			text = "Cờ cam";
    			break;
    		case 8:
    			text = "Cờ đen";
    			break;
    		case 9:
    			text = "Cờ Kaio";
    			break;
    		case 10:
    			text = "Cờ Mabu";
    			break;
    		case 11:
    			text = "Cờ xanh dương";
    			break;
    		}
    		if (!text.Equals(""))
    		{
    			return "(" + text + ") ";
    		}
    		return text;
    	}

    	public static void LoadData()
    	{
    		AutoVutDo.loadData();
    		AutoboMong.loadData();
    		DoHoa.loadData();
    		LoadFlagColor();
    		if (mGraphics.zoomLevel == 2)
    		{
    			try
    			{
    				logoGameScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoCuongLe));
    				logoServerListScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoCuongLe));
    			}
    			catch
    			{
    			}
    		}
    		if (mGraphics.zoomLevel == 1)
    		{
    			try
    			{
    				logoServerListScreen = Image.__createImage(Convert.FromBase64String(Logopixel.Logo));
    				logoGameScreen = Image.__createImage(Convert.FromBase64String(Logopixel.Logo));
    			}
    			catch
    			{
    			}
    		}
    		try
    		{
    			APIKey = File.ReadAllText("Data\\keyAPI.ini");
    			APIServer = File.ReadAllText("Data\\serverAPI.ini");
    		}
    		catch
    		{
    		}
    		new Thread(GetLoginDataFromAccountManager).Start();
    	}

    	public static void LoadFlagColor()
    	{
    		listFlagColor.Add(Color.black);
    		listFlagColor.Add(new Color(0f, 0.99609375f, 0.99609375f));
    		listFlagColor.Add(Color.red);
    		listFlagColor.Add(new Color(0.54296875f, 0f, 0.54296875f));
    		listFlagColor.Add(Color.yellow);
    		listFlagColor.Add(Color.green);
    		listFlagColor.Add(new Color(0.99609375f, 0.51171875f, 125f / 128f));
    		listFlagColor.Add(new Color(0.80078125f, 51f / 128f, 0f));
    		listFlagColor.Add(Color.black);
    		listFlagColor.Add(Color.blue);
    		listFlagColor.Add(Color.red);
    		listFlagColor.Add(Color.blue);
    	}

    	public static void RefreshListCharsInMap()
    	{
    		if (listCharsInMap.Count <= 2)
    		{
    			return;
    		}
    		List<Char> list = new List<Char>();
    		while (listCharsInMap.Count != 0)
    		{
    			Char obj = listCharsInMap[0];
    			list.Add(obj);
    			string nameWithoutClanTag = obj.GetNameWithoutClanTag();
    			listCharsInMap.RemoveAt(0);
    			for (int i = 0; i < listCharsInMap.Count; i++)
    			{
    				Char obj2 = listCharsInMap[i];
    				if (nameWithoutClanTag == obj2.GetNameWithoutClanTag())
    				{
    					list.Add(obj2);
    					listCharsInMap.RemoveAt(i);
    					i--;
    				}
    			}
    		}
    		listCharsInMap.Clear();
    		listCharsInMap = list;
    	}


    	public static bool isColdImmune(Item item)
    	{
    		int id = item.template.id;
    		if (id != 450 && id != 630 && id != 631 && id != 632 && id != 878 && id != 879)
    		{
    			if (id >= 386)
    			{
    				return id <= 394;
    			}
    			return false;
    		}
    		return true;
    	}

    	public static void UseCapsule()
    	{
    		isUsingCapsule = true;
    		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
    		{
    			Item item = Char.myCharz().arrItemBag[i];
    			if (item != null && (item.template.id == 194 || item.template.id == 193))
    			{
    				Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
    				break;
    			}
    		}
    		Thread.Sleep(500);
    		Service.gI().requestMapSelect(0);
    		Thread.Sleep(1000);
    		isUsingCapsule = false;
    	}

    	public static void UseSkill(int skillIndex)
    	{
    		if (!isUsingSkill)
    		{
    			isUsingSkill = true;
    			if (Char.myCharz().myskill != GameScr.keySkill[skillIndex])
    			{
    				GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], isShortcut: true);
    				Thread.Sleep(200);
    				GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], isShortcut: true);
    				isUsingSkill = false;
    			}
    			else
    			{
    				GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], isShortcut: true);
    				isUsingSkill = false;
    			}
    		}
    	}

    	public static void GetLoginDataFromAccountManager()
    	{
    		try
    		{
    			string[] array = Environment.GetCommandLineArgs()[1].Split('|');
    			account = array[1];
    			server = int.Parse(array[2]);
    			password = DecryptString(array[3], "ud");
    			new Thread(Login).Start();
    		}
    		catch
    		{
    			account = "";
    		}
    	}

    	public static void Login()
    	{
    		Thread.Sleep(1000);
    		while (true)
    		{
    			try
    			{
    				if (string.IsNullOrEmpty(Char.myCharz().cName))
    				{
    					Thread.Sleep(100);
    					while (!ServerListScreen.loadScreen)
    					{
    						Thread.Sleep(10);
    					}
    					Thread.Sleep(500);
    					Rms.saveRMSString("acc", "Cuong Le");
    					Rms.saveRMSString("pass", "fuckyou");
    					Thread.Sleep(500);
    					Rms.saveRMSInt("svselect", server - 1);
    					ServerListScreen.ipSelect = server - 1;
    					if (server <= 20)
    					{
    						GameCanvas.serverScreen.selectServer();
    						while (!ServerListScreen.loadScreen)
    						{
    							Thread.Sleep(10);
    						}
    						while (!Session_ME.gI().isConnected())
    						{
    							Thread.Sleep(100);
    						}
    						Thread.Sleep(100);
    						while (!ServerListScreen.loadScreen)
    						{
    							Thread.Sleep(10);
    						}
    					}
    					Thread.Sleep(1000);
    					GameCanvas.serverScreen.perform(3, null);
    					Thread.Sleep(1000);
    					GameCanvas.gameTick = 0;
    					loginAgain = true;
    					for (int num = 36; num >= 6; num--)
    					{
    						LasterLogin = num;
    						Thread.Sleep(1000);
    					}
    				}
    			}
    			catch
    			{
    			}
    			for (int num2 = 5; num2 >= 0; num2--)
    			{
    				LasterLogin = num2;
    				Thread.Sleep(1000);
    			}
    		}
    	}

        public static void SolveCapcha()
        {
            isSlovingCapcha = true;
            statusCapcha = "🔄 Đang giải Captcha...";
            Thread.Sleep(1000);

            try
            {
                // Thiết lập TLS
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                string apiKeyPath = Path.Combine("Data", "keyAPI.ini");
                if (!File.Exists(apiKeyPath))
                {
                    statusCapcha = "⚠ Không tìm thấy file keyAPI.ini!";
                    return;
                }

                string apiKey = File.ReadAllText(apiKeyPath);
                if (string.IsNullOrEmpty(apiKey))
                {
                    statusCapcha = "⚠ API key trống hoặc sai định dạng!";
                    return;
                }

                if (GameScr.imgCapcha == null || GameScr.imgCapcha.texture == null)
                {
                    statusCapcha = "⚠ Không có ảnh Captcha!";
                    return;
                }

                // 📷 Mã hóa trực tiếp ảnh Captcha sang base64
                statusCapcha = "📷 Đang mã hóa ảnh Captcha...";
                string base64Image = Convert.ToBase64String(GameScr.imgCapcha.texture.EncodeToPNG());

                // 🌐 Gửi API
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("User-Agent", "Mozilla/5.0");

                    NameValueCollection data = new NameValueCollection
                    {
                        ["image"] = base64Image
                    };

                    string apiUrl = $"http://api.phamgiang.net/captcha/nro?token={apiKey}";

                    statusCapcha = "📡 Đang gửi yêu cầu giải Captcha...";
                    byte[] responseBytes = webClient.UploadValues(apiUrl, "POST", data);
                    string response = Encoding.UTF8.GetString(responseBytes);

                    // 🧠 Phân tích phản hồi
                    Match match = Regex.Match(response, "\"captcha\"\\s*:\\s*\"(\\d+)\"");
                    Match statusMatch = Regex.Match(response, "\"status\"\\s*:\\s*(\\d+)");

                    if (match.Success && statusMatch.Success && statusMatch.Groups[1].Value == "0")
                    {
                        string capchaValue = match.Groups[1].Value;

                        if (capchaValue.Length >= 4 && capchaValue.Length <= 7)
                        {
                            statusCapcha = "✅ Nhập Captcha: " + capchaValue;

                            foreach (char ch in capchaValue)
                            {
                                Service.gI().mobCapcha(ch);
                                Thread.Sleep(Res.random(500, 700));
                            }
                            countCaptchaSolved++;
                            statusCapcha = "✅ Giải Captcha thành công!";
                        }
                        else
                        {
                            statusCapcha = "⚠ Captcha không hợp lệ: " + capchaValue;
                        }
                    }
                    else
                    {
                        statusCapcha = "❌ API lỗi hoặc Captcha sai: " + response;
                    }
                }
            }
            catch (WebException webEx)
            {
                using (var reader = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    statusCapcha = "❌ Lỗi mạng/API: " + error;
                }
            }
            catch (Exception ex)
            {
                statusCapcha = "❌ Lỗi: " + ex.Message;
            }
            finally
            {
                if (GameScr.gI().mobCapcha != null)
                {
                    Thread.Sleep(3000);
                }

                isSlovingCapcha = false;
            }
        }



        public static bool isMeHasEnoughMP(Skill skillToUse)
    	{
    		if (skillToUse.template.manaUseType == 2)
    		{
    			return true;
    		}
    		if (skillToUse.template.manaUseType != 1)
    		{
    			return Char.myCharz().cMP >= skillToUse.manaUse;
    		}
    		return Char.myCharz().cMP >= skillToUse.manaUse * Char.myCharz().cMPFull / 100;
    	}

    	public static void smethod_14()
    	{
    	}

    	public static void smethod_16()
    	{
    	}

    	public static string DecryptString(string str, string key)
    	{
    		byte[] array = Convert.FromBase64String(str);
    		byte[] key2 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
    		byte[] bytes = new TripleDESCryptoServiceProvider
    		{
    			Key = key2,
    			Mode = CipherMode.ECB,
    			Padding = PaddingMode.PKCS7
    		}.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
    		return Encoding.UTF8.GetString(bytes);
    	}

    	public static void checkInputSHIFT(ref int num)
    	{
    		if (num >= 97 && num <= 122)
    		{
    			num -= 32;
    		}
    	}
    

        public static void infoView(mGraphics g, Char c, int xStart, int yStart)
    	{
    		if (c.charID == Char.myCharz().charID)
    		{
    			DoHoa.DrawFont.drawString(g, $"HP: {c.cHP}/{c.cHPFull}", xStart, yStart, 0);
    			DoHoa.DrawFont.drawString(g, $"MP: {c.cMP}/{c.cMPFull}", xStart, yStart + 8, 0);
    			DoHoa.DrawFont.drawString(g, $"SĐ: {c.cDamFull}", xStart, yStart + 16, 0);
    			return;
    		}
    		DoHoa.DrawFont.drawString(g, "Tên: " + c.cName, xStart, yStart, 0);
    		DoHoa.DrawFont.drawString(g, $"HP: {c.cHP}/{c.cHPFull}", xStart, yStart + 8, 0);
    		DoHoa.DrawFont.drawString(g, $"MP: {c.cMP}/{c.cMPFull}", xStart, yStart + 16, 0);
    		DoHoa.DrawFont.drawString(g, $"SĐ: {c.cDamFull}", xStart, yStart + 24, 0);
    		DoHoa.DrawFont.drawString(g, "SM: " + NinjaUtil.getMoneys(c.cPower), xStart, yStart + 32, 0);
    		DoHoa.DrawFont.drawString(g, "TN: " + NinjaUtil.getMoneys(c.cTiemNang), xStart, yStart + 40, 0);
    	}
        public static void paintPKFlag(mGraphics g, Char @char, int num)
        {
            if (@char.cFlag != 0 && @char.cFlag != -1)
            {
                SmallImage.drawFlagSquare(g, @char.cFlag, GameCanvas.w - widthRect - 8, num + 1);
            }
        }
        public static void outConnect()
    	{
    		if (Controller.isConnectionFail)
    		{
    			Controller.isConnectionFail = false;
    		}
    		GameCanvas.isResume = true;
    		Session_ME.gI().clearSendingMessage();
    		Session_ME2.gI().clearSendingMessage();
    		if (Controller.isLoadingData)
    		{
    			GameCanvas.instance.resetToLoginScrz();
    			GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
    			Controller.isDisconnected = false;
    			return;
    		}
    		Char.isLoadingMap = false;
    		if (Controller.isMain)
    		{
    			ServerListScreen.testConnect = 0;
    		}
    		GameCanvas.instance.resetToLoginScrz();
    		if (Main.typeClient == 6)
    		{
    			if (GameCanvas.currentScreen != GameCanvas.serverScreen && GameCanvas.currentScreen != GameCanvas.loginScr)
    			{
    				GameCanvas.startOKDlg(mResources.maychutathoacmatsong);
    			}
    		}
    		else
    		{
    			GameCanvas.startOKDlg(mResources.maychutathoacmatsong);
    		}
    		mSystem.endKey();
    	}

    	public static void infoTrain(mGraphics g, int xStart, int yStart)
    	{
    		DoHoa.DrawFont.drawString(g, "Vàng : " + Res.FormatNumberVIP(Char.myCharz().xu), xStart, yStart, 0);
    		DoHoa.DrawFont.drawString(g, "Số vàng up được : " + Res.FormatNumberVIP(Char.myCharz().xu - GoldCurrent), xStart, yStart + 8, 0);
    		if (Char.myCharz().xu - GoldUpdate > 0 && GameCanvas.gameTick % 30 == 0)
    		{
    			GoldUpdateRealTime = Char.myCharz().xu - GoldUpdate;
    			GoldUpdate = Char.myCharz().xu;
    		}
    		DoHoa.DrawFont.drawString(g, "Vàng của 1 hit: " + Res.FormatNumberVIP(GoldUpdateRealTime), xStart, yStart + 16, 0);
    	}

    	public static void useHopThe()
    	{
    		UseItem(454);
    	}

        public static void controlTeleChar()
        {
            int num = (isMeInNRDMap() ? 35 : 95);
            widthRect = 120;
            heightRect = 7;
            for (int i = 0; i < listCharsInMap.Count; i++)
            {
                if (listCharsInMap[i] != null)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - widthRect, num + 1, widthRect * 3/4, heightRect) && GameCanvas.isPointerClick)
                    {
    					if(Char.myCharz().charFocus == listCharsInMap[i])
    					{
                            AutoMap.TeleportTo(listCharsInMap[i].cx, listCharsInMap[i].cy);
                        }
    					else
    					{
                            Char.myCharz().focusManualTo(listCharsInMap[i]);
                        }
                        SoundMn.gI().buttonClick();
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                    }
                    num += heightRect + 1;
                }
            }
        }
        static bool IsEmulator()
        {
            string model = SystemInfo.deviceModel.ToLower();
            string name = SystemInfo.deviceName.ToLower();
            string manufacturer = SystemInfo.deviceName.ToLower();
            string os = SystemInfo.operatingSystem.ToLower();
            string cpu = SystemInfo.processorType.ToLower();

            string[] emulatorIndicators = new string[]
            {
            "nox", "genymotion", "emulator", "sdk", "google_sdk",
            "x86", "virtual", "test-keys", "bluestacks", "bs"
            };

            foreach (var keyword in emulatorIndicators)
            {
                if (model.Contains(keyword) || name.Contains(keyword) || os.Contains(keyword) || cpu.Contains(keyword))
                {
                    return true;
                }
            }

            // Một số trình giả lập có device model trống hoặc quá đơn giản
            if (string.IsNullOrEmpty(model) || model.Length < 4)
            {
                return true;
            }

            return false;
        }
    }
}
