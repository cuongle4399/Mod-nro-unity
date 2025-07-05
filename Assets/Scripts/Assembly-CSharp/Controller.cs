using System;
using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
using UnityEngine;

public class Controller : IMessageHandler
{
	protected static Controller me;

	protected static Controller me2;

	public Message messWait;

	public static bool isLoadingData = false;

	public static bool isConnectOK;

	public static bool isConnectionFail;

	public static bool isDisconnected;

	public static bool isMain;

	private float demCount;

	private int move;

	private int total;

	public static bool isStopReadMessage;

	public static bool isGet_CLIENT_INFO = false;

	public static MyHashTable frameHT_NEWBOSS = new MyHashTable();

	public const sbyte PHUBAN_TYPE_CHIENTRUONGNAMEK = 0;

	public const sbyte PHUBAN_START = 0;

	public const sbyte PHUBAN_UPDATE_POINT = 1;

	public const sbyte PHUBAN_END = 2;

	public const sbyte PHUBAN_LIFE = 4;

	public const sbyte PHUBAN_INFO = 5;

	public static bool isEXTRA_LINK = false;

	public static Controller gI()
	{
		if (me == null)
		{
			me = new Controller();
		}
		return me;
	}

	public static Controller gI2()
	{
		if (me2 == null)
		{
			me2 = new Controller();
		}
		return me2;
	}

	public void onConnectOK(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectOK();
	}

	public void onConnectionFail(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onConnectionFail();
	}

	public void onDisconnected(bool isMain1)
	{
		isMain = isMain1;
		mSystem.onDisconnected();
	}

	public void requestItemPlayer(Message msg)
	{
		try
		{
			int num = msg.reader().readUnsignedByte();
			Item item = GameScr.currentCharViewInfo.arrItemBody[num];
			item.saleCoinLock = msg.reader().readInt();
			item.sys = msg.reader().readByte();
			item.options = new MyVector();
			try
			{
				while (true)
				{
					ItemOption itemOption = readItemOption(msg);
					if (itemOption != null)
					{
						item.options.addElement(itemOption);
					}
				}
			}
			catch (Exception ex)
			{
				Cout.println("Loi tairequestItemPlayer 1" + ex.ToString());
			}
		}
		catch (Exception ex2)
		{
			Cout.println("Loi tairequestItemPlayer 2" + ex2.ToString());
		}
	}

	public void onMessage(Message msg)
	{
		GameCanvas.debugSession.removeAllElements();
		GameCanvas.debug("SA1", 2);
		try
		{
			if (msg.command != -74)
			{
				Res.outz("=========> [READ] cmd= " + msg.command);
			}
			Char obj = null;
			Mob mob = null;
			MyVector myVector = new MyVector();
			int num = 0;
			GameCanvas.timeLoading = 15;
			Controller2.readMessage(msg);
			switch (msg.command)
			{
			case 12:
				read_cmdExtraBig(msg);
				break;
			case 0:
				readLogin(msg);
				break;
			case 24:
				read_cmdExtra(msg);
				break;
			case 20:
				phuban_Info(msg);
				break;
			case 66:
				readGetImgByName(msg);
				break;
			case 65:
			{
				sbyte b68 = msg.reader().readSByte();
				string text7 = msg.reader().readUTF();
				short num157 = msg.reader().readShort();
				if (ItemTime.isExistMessage(b68))
				{
					if (num157 != 0)
					{
						ItemTime.getMessageById(b68).initTimeText(b68, text7, num157);
					}
					else
					{
						GameScr.textTime.removeElement(ItemTime.getMessageById(b68));
					}
				}
				else
				{
					ItemTime itemTime = new ItemTime();
					itemTime.initTimeText(b68, text7, num157);
					GameScr.textTime.addElement(itemTime);
				}
				break;
			}
			case 112:
			{
				sbyte b59 = msg.reader().readByte();
				Res.outz("spec type= " + b59);
				if (b59 == 0)
				{
					Panel.spearcialImage = msg.reader().readShort();
					Panel.specialInfo = msg.reader().readUTF();
				}
				else
				{
					if (b59 != 1)
					{
						break;
					}
					sbyte b60 = msg.reader().readByte();
					Char.myCharz().infoSpeacialSkill = new string[b60][];
					Char.myCharz().imgSpeacialSkill = new short[b60][];
					GameCanvas.panel.speacialTabName = new string[b60][];
					for (int num131 = 0; num131 < b60; num131++)
					{
						GameCanvas.panel.speacialTabName[num131] = new string[2];
						string[] array10 = Res.split(msg.reader().readUTF(), "\n", 0);
						if (array10.Length == 2)
						{
							GameCanvas.panel.speacialTabName[num131] = array10;
						}
						if (array10.Length == 1)
						{
							GameCanvas.panel.speacialTabName[num131][0] = array10[0];
							GameCanvas.panel.speacialTabName[num131][1] = string.Empty;
						}
						int num132 = msg.reader().readByte();
						Char.myCharz().infoSpeacialSkill[num131] = new string[num132];
						Char.myCharz().imgSpeacialSkill[num131] = new short[num132];
						for (int num133 = 0; num133 < num132; num133++)
						{
							Char.myCharz().imgSpeacialSkill[num131][num133] = msg.reader().readShort();
							Char.myCharz().infoSpeacialSkill[num131][num133] = msg.reader().readUTF();
						}
					}
					GameCanvas.panel.tabName[25] = GameCanvas.panel.speacialTabName;
					GameCanvas.panel.setTypeSpeacialSkill();
					GameCanvas.panel.show();
				}
				break;
			}
			case -98:
			{
				sbyte b69 = msg.reader().readByte();
				GameCanvas.menu.showMenu = false;
				if (b69 == 0)
				{
					GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
				}
				break;
			}
			case -97:
				Char.myCharz().cNangdong = msg.reader().readInt();
				break;
			case -96:
			{
				sbyte typeTop = msg.reader().readByte();
				GameCanvas.panel.vTop.removeAllElements();
				string topName = msg.reader().readUTF();
				sbyte b49 = msg.reader().readByte();
				for (int num114 = 0; num114 < b49; num114++)
				{
					int rank = msg.reader().readInt();
					int pId = msg.reader().readInt();
					short headID = msg.reader().readShort();
					short headICON = msg.reader().readShort();
					short body = msg.reader().readShort();
					short leg = msg.reader().readShort();
					string name = msg.reader().readUTF();
					string info3 = msg.reader().readUTF();
					TopInfo topInfo = new TopInfo();
					topInfo.rank = rank;
					topInfo.headID = headID;
					topInfo.headICON = headICON;
					topInfo.body = body;
					topInfo.leg = leg;
					topInfo.name = name;
					topInfo.info = info3;
					topInfo.info2 = msg.reader().readUTF();
					topInfo.pId = pId;
					GameCanvas.panel.vTop.addElement(topInfo);
				}
				GameCanvas.panel.topName = topName;
				GameCanvas.panel.setTypeTop(typeTop);
				GameCanvas.panel.show();
				break;
			}
			case -94:
				while (msg.reader().available() > 0)
				{
					short num97 = msg.reader().readShort();
					int num98 = msg.reader().readInt();
					for (int num99 = 0; num99 < Char.myCharz().vSkill.size(); num99++)
					{
						Skill skill = (Skill)Char.myCharz().vSkill.elementAt(num99);
						if (skill != null && skill.skillId == num97)
						{
							if (num98 < skill.coolDown)
							{
								skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num98);
							}
							Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num98 + "curr cool down= " + skill.coolDown);
						}
					}
				}
				break;
			case -95:
			{
				sbyte b6 = msg.reader().readByte();
				Res.outz("type= " + b6);
				if (b6 == 0)
				{
					int num9 = msg.reader().readInt();
					short templateId = msg.reader().readShort();
					long num10 = msg.reader().readLong();
					SoundMn.gI().explode_1();
					if (num9 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe = new Mob(num9, false, false, false, false, false, templateId, 1, num10, 0, num10, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
						Char.myCharz().mobMe.isMobMe = true;
						EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
						Char.myCharz().tMobMeBorn = 30;
						GameScr.vMob.addElement(Char.myCharz().mobMe);
					}
					else
					{
						obj = GameScr.findCharInMap(num9);
						if (obj != null)
						{
							Mob mob2 = new Mob(num9, false, false, false, false, false, templateId, 1, num10, 0, num10, (short)obj.cx, (short)obj.cy, 4, 0);
							mob2.isMobMe = true;
							obj.mobMe = mob2;
							GameScr.vMob.addElement(obj.mobMe);
						}
						else
						{
							Mob mob3 = GameScr.findMobInMap(num9);
							if (mob3 == null)
							{
								mob3 = new Mob(num9, false, false, false, false, false, templateId, 1, num10, 0, num10, -100, -100, 4, 0);
								mob3.isMobMe = true;
								GameScr.vMob.addElement(mob3);
							}
						}
					}
				}
				if (b6 == 1)
				{
					int num11 = msg.reader().readInt();
					int mobId = msg.reader().readByte();
					Res.outz("mod attack id= " + num11);
					if (num11 == Char.myCharz().charID)
					{
						if (GameScr.findMobInMap(mobId) != null)
						{
							Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
					else
					{
						obj = GameScr.findCharInMap(num11);
						if (obj != null && GameScr.findMobInMap(mobId) != null)
						{
							obj.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
						}
					}
				}
				if (b6 == 2)
				{
					int num12 = msg.reader().readInt();
					int num13 = msg.reader().readInt();
					long num14 = msg.reader().readLong();
					long cHPNew = msg.reader().readLong();
					if (num12 == Char.myCharz().charID)
					{
						Res.outz("mob dame= " + num14);
						obj = GameScr.findCharInMap(num13);
						if (obj != null)
						{
							obj.cHPNew = cHPNew;
							if (Char.myCharz().mobMe.isBusyAttackSomeOne)
							{
								obj.doInjure(num14, 0L, false, true);
							}
							else
							{
								Char.myCharz().mobMe.dame = num14;
								Char.myCharz().mobMe.setAttack(obj);
							}
						}
					}
					else
					{
						mob = GameScr.findMobInMap(num12);
						if (mob != null)
						{
							if (num13 == Char.myCharz().charID)
							{
								Char.myCharz().cHPNew = cHPNew;
								if (mob.isBusyAttackSomeOne)
								{
									Char.myCharz().doInjure(num14, 0L, false, true);
								}
								else
								{
									mob.dame = num14;
									mob.setAttack(Char.myCharz());
								}
							}
							else
							{
								obj = GameScr.findCharInMap(num13);
								if (obj != null)
								{
									obj.cHPNew = cHPNew;
									if (mob.isBusyAttackSomeOne)
									{
										obj.doInjure(num14, 0L, false, true);
									}
									else
									{
										mob.dame = num14;
										mob.setAttack(obj);
									}
								}
							}
						}
					}
				}
				if (b6 == 3)
				{
					int num15 = msg.reader().readInt();
					int mobId2 = msg.reader().readInt();
					long hp = msg.reader().readLong();
					long num16 = msg.reader().readLong();
					obj = null;
					obj = ((Char.myCharz().charID != num15) ? GameScr.findCharInMap(num15) : Char.myCharz());
					if (obj != null)
					{
						mob = GameScr.findMobInMap(mobId2);
						if (obj.mobMe != null)
						{
							obj.mobMe.attackOtherMob(mob);
						}
						if (mob != null)
						{
							mob.hp = hp;
							mob.updateHp_bar();
							if (num16 == 0)
							{
								mob.x = mob.xFirst;
								mob.y = mob.yFirst;
								GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
							}
							else
							{
								GameScr.startFlyText("-" + num16, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
							}
						}
					}
				}
				if (b6 == 4)
				{
				}
				if (b6 == 5)
				{
					int num17 = msg.reader().readInt();
					sbyte b7 = msg.reader().readByte();
					int mobId3 = msg.reader().readInt();
					long num18 = msg.reader().readLong();
					long hp2 = msg.reader().readLong();
					obj = null;
					obj = ((num17 != Char.myCharz().charID) ? GameScr.findCharInMap(num17) : Char.myCharz());
					if (obj == null)
					{
						return;
					}
					if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
					{
						obj.setSkillPaint(GameScr.sks[b7], 0);
					}
					else
					{
						obj.setSkillPaint(GameScr.sks[b7], 1);
					}
					Mob mob4 = GameScr.findMobInMap(mobId3);
					if (obj.cx <= mob4.x)
					{
						obj.cdir = 1;
					}
					else
					{
						obj.cdir = -1;
					}
					obj.mobFocus = mob4;
					mob4.hp = hp2;
					mob4.updateHp_bar();
					GameCanvas.debug("SA83v2", 2);
					if (num18 == 0)
					{
						mob4.x = mob4.xFirst;
						mob4.y = mob4.yFirst;
						GameScr.startFlyText(mResources.miss, mob4.x, mob4.y - mob4.h, 0, -2, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num18, mob4.x, mob4.y - mob4.h, 0, -2, mFont.ORANGE);
					}
				}
				if (b6 == 6)
				{
					int num19 = msg.reader().readInt();
					if (num19 == Char.myCharz().charID)
					{
						Char.myCharz().mobMe.startDie();
					}
					else
					{
						obj = GameScr.findCharInMap(num19);
						if (obj != null)
						{
							obj.mobMe.startDie();
						}
					}
				}
				if (b6 != 7)
				{
					break;
				}
				int num20 = msg.reader().readInt();
				if (num20 == Char.myCharz().charID)
				{
					Char.myCharz().mobMe = null;
					for (int i = 0; i < GameScr.vMob.size(); i++)
					{
						if (((Mob)GameScr.vMob.elementAt(i)).mobId == num20)
						{
							GameScr.vMob.removeElementAt(i);
						}
					}
					break;
				}
				obj = GameScr.findCharInMap(num20);
				for (int j = 0; j < GameScr.vMob.size(); j++)
				{
					if (((Mob)GameScr.vMob.elementAt(j)).mobId == num20)
					{
						GameScr.vMob.removeElementAt(j);
					}
				}
				if (obj != null)
				{
					obj.mobMe = null;
				}
				break;
			}
			case -92:
				Main.typeClient = msg.reader().readByte();
				if (Rms.loadRMSString("ResVersion") == null)
				{
					Rms.clearAll();
				}
				Rms.saveRMSInt("clienttype", Main.typeClient);
				Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
				if (Rms.loadRMSString("ResVersion") == null)
				{
					GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
				}
				break;
			case -91:
			{
				sbyte b42 = msg.reader().readByte();
				GameCanvas.panel.mapNames = new string[b42];
				GameCanvas.panel.planetNames = new string[b42];
				for (int num96 = 0; num96 < b42; num96++)
				{
					GameCanvas.panel.mapNames[num96] = msg.reader().readUTF();
					GameCanvas.panel.planetNames[num96] = msg.reader().readUTF();
				}
				GameCanvas.panel.setTypeMapTrans();
				GameCanvas.panel.show();
				break;
			}
			case -90:
			{
				sbyte b70 = msg.reader().readByte();
				int num161 = msg.reader().readInt();
				Res.outz("===> UPDATE_BODY:    type = " + b70);
				obj = ((Char.myCharz().charID != num161) ? GameScr.findCharInMap(num161) : Char.myCharz());
				if (b70 != -1)
				{
					short num162 = msg.reader().readShort();
					short num163 = msg.reader().readShort();
					short num164 = msg.reader().readShort();
					sbyte isMonkey = msg.reader().readByte();
					if (obj != null)
					{
						if (obj.charID == num161)
						{
							obj.isMask = true;
							obj.isMonkey = isMonkey;
							if (obj.isMonkey != 0)
							{
								obj.isWaitMonkey = false;
								obj.isLockMove = false;
							}
						}
						else if (obj != null)
						{
							obj.isMask = true;
							obj.isMonkey = isMonkey;
						}
						if (num162 != -1)
						{
							obj.head = num162;
						}
						if (num163 != -1)
						{
							obj.body = num163;
						}
						if (num164 != -1)
						{
							obj.leg = num164;
						}
					}
				}
				if (b70 == -1 && obj != null)
				{
					obj.isMask = false;
					obj.isMonkey = 0;
				}
				if (obj == null)
				{
					break;
				}
				for (int num165 = 0; num165 < 54; num165++)
				{
					obj.removeEffChar(0, 201 + num165);
				}
				if (obj.bag >= 201 && obj.bag < 255)
				{
					Effect effect2 = new Effect(obj.bag, obj, 2, -1, 10, 1);
					effect2.typeEff = 5;
					obj.addEffChar(effect2);
				}
				if (obj.bag == 30 && obj.me)
				{
					GameScr.isPickNgocRong = true;
				}
				if (!obj.me)
				{
					break;
				}
				GameScr.isudungCapsun4 = false;
				GameScr.isudungCapsun3 = false;
				for (int num166 = 0; num166 < Char.myCharz().arrItemBag.Length; num166++)
				{
					Item item4 = Char.myCharz().arrItemBag[num166];
					if (item4 == null)
					{
						continue;
					}
					if (item4.template.id == 194)
					{
						GameScr.isudungCapsun4 = item4.quantity > 0;
						if (GameScr.isudungCapsun4)
						{
							break;
						}
					}
					else if (item4.template.id == 193)
					{
						GameScr.isudungCapsun3 = item4.quantity > 0;
					}
				}
				break;
			}
			case -88:
				GameCanvas.endDlg();
				GameCanvas.serverScreen.switchToMe();
				break;
			case -87:
			{
				Res.outz("GET UPDATE_DATA " + msg.reader().available() + " bytes");
				msg.reader().mark(500000);
				createData(msg.reader(), true);
				msg.reader().reset();
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				sbyte[] data2 = new sbyte[1] { GameScr.vcData };
				Rms.saveRMS("NRdataVersion", data2);
				LoginScr.isUpdateData = false;
				GameScr.gI().readOk();
				break;
			}
			case -86:
			{
				sbyte b43 = msg.reader().readByte();
				Res.outz("server gui ve giao dich action = " + b43);
				if (b43 == 0)
				{
					int playerID = msg.reader().readInt();
					GameScr.gI().giaodich(playerID);
				}
				if (b43 == 1)
				{
					int num106 = msg.reader().readInt();
					Char obj6 = GameScr.findCharInMap(num106);
					if (obj6 == null)
					{
						return;
					}
					GameCanvas.panel.setTypeGiaoDich(obj6);
					GameCanvas.panel.show();
					Service.gI().getPlayerMenu(num106);
				}
				if (b43 == 2)
				{
					sbyte b44 = msg.reader().readByte();
					for (int num107 = 0; num107 < GameCanvas.panel.vMyGD.size(); num107++)
					{
						Item item2 = (Item)GameCanvas.panel.vMyGD.elementAt(num107);
						if (item2.indexUI == b44)
						{
							GameCanvas.panel.vMyGD.removeElement(item2);
							break;
						}
					}
				}
				if (b43 == 5)
				{
				}
				if (b43 == 6)
				{
					GameCanvas.panel.isFriendLock = true;
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.isFriendLock = true;
					}
					GameCanvas.panel.vFriendGD.removeAllElements();
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.vFriendGD.removeAllElements();
					}
					int friendMoneyGD = msg.reader().readInt();
					sbyte b45 = msg.reader().readByte();
					Res.outz("item size = " + b45);
					for (int num108 = 0; num108 < b45; num108++)
					{
						Item item3 = new Item();
						item3.template = ItemTemplates.get(msg.reader().readShort());
						item3.quantity = msg.reader().readInt();
						int num109 = msg.reader().readUnsignedByte();
						if (num109 != 0)
						{
							item3.itemOption = new ItemOption[num109];
							for (int num110 = 0; num110 < item3.itemOption.Length; num110++)
							{
								ItemOption itemOption5 = readItemOption(msg);
								if (itemOption5 != null)
								{
									item3.itemOption[num110] = itemOption5;
									item3.compare = GameCanvas.panel.getCompare(item3);
								}
							}
						}
						if (GameCanvas.panel2 != null)
						{
							GameCanvas.panel2.vFriendGD.addElement(item3);
						}
						else
						{
							GameCanvas.panel.vFriendGD.addElement(item3);
						}
					}
					if (GameCanvas.panel2 != null)
					{
						GameCanvas.panel2.setTabGiaoDich(false);
						GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
					}
					else
					{
						GameCanvas.panel.friendMoneyGD = friendMoneyGD;
						if (GameCanvas.panel.currentTabIndex == 2)
						{
							GameCanvas.panel.setTabGiaoDich(false);
						}
					}
				}
				if (b43 == 7)
				{
					InfoDlg.hide();
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.hide();
					}
				}
				break;
			}
			case -85:
			{
				Res.outz("CAP CHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				sbyte b33 = msg.reader().readByte();
				if (b33 == 0)
				{
					int num67 = msg.reader().readUnsignedShort();
					Res.outz("lent =" + num67);
					sbyte[] data3 = new sbyte[num67];
					msg.reader().read(ref data3, 0, num67);
					GameScr.imgCapcha = Image.createImage(data3, 0, num67);
					GameScr.gI().keyInput = "-----";
					GameScr.gI().strCapcha = msg.reader().readUTF();
					GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
					GameScr.gI().mobCapcha = new Mob();
					GameScr.gI().right = null;
				}
				if (b33 == 1)
				{
					MobCapcha.isAttack = true;
				}
				if (b33 == 2)
				{
					MobCapcha.explode = true;
					GameScr.gI().right = GameScr.gI().cmdFocus;
				}
				break;
			}
			case -112:
			{
				sbyte b47 = msg.reader().readByte();
				if (b47 == 0)
				{
					sbyte mobIndex = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex).clearBody();
				}
				if (b47 == 1)
				{
					sbyte mobIndex2 = msg.reader().readByte();
					GameScr.findMobInMap(mobIndex2).setBody(msg.reader().readShort());
				}
				break;
			}
			case -84:
			{
				int index = msg.reader().readUnsignedByte();
				Mob mob5 = null;
				try
				{
					mob5 = (Mob)GameScr.vMob.elementAt(index);
				}
				catch (Exception)
				{
				}
				if (mob5 != null)
				{
					mob5.maxHp = msg.reader().readLong();
				}
				break;
			}
			case -83:
			{
				sbyte b39 = msg.reader().readByte();
				if (b39 == 0)
				{
					int num87 = msg.reader().readShort();
					int bgRID = msg.reader().readShort();
					int num88 = msg.reader().readUnsignedByte();
					int num89 = msg.reader().readInt();
					string text2 = msg.reader().readUTF();
					int num90 = msg.reader().readShort();
					int num91 = msg.reader().readShort();
					sbyte b40 = msg.reader().readByte();
					if (b40 == 1)
					{
						GameScr.gI().isRongNamek = true;
					}
					else
					{
						GameScr.gI().isRongNamek = false;
					}
					GameScr.gI().xR = num90;
					GameScr.gI().yR = num91;
					Res.outz("xR= " + num90 + " yR= " + num91 + " +++++++++++++++++++++++++++++++++++++++");
					if (Char.myCharz().charID == num89)
					{
						GameCanvas.panel.hideNow();
						GameScr.gI().activeRongThanEff(true);
					}
					else if (TileMap.mapID == num87 && TileMap.zoneID == num88)
					{
						GameScr.gI().activeRongThanEff(false);
					}
					else if (mGraphics.zoomLevel > 1)
					{
						GameScr.gI().doiMauTroi();
					}
					GameScr.gI().mapRID = num87;
					GameScr.gI().bgRID = bgRID;
					GameScr.gI().zoneRID = num88;
				}
				if (b39 == 1)
				{
					Res.outz("map RID = " + GameScr.gI().mapRID + " zone RID= " + GameScr.gI().zoneRID);
					Res.outz("map ID = " + TileMap.mapID + " zone ID= " + TileMap.zoneID);
					if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
					{
						GameScr.gI().hideRongThanEff();
					}
					else
					{
						GameScr.gI().isRongThanXuatHien = false;
						if (GameScr.gI().isRongNamek)
						{
							GameScr.gI().isRongNamek = false;
						}
					}
				}
				if (b39 != 2)
				{
				}
				break;
			}
			case -82:
			{
				sbyte b29 = msg.reader().readByte();
				TileMap.tileIndex = new int[b29][][];
				TileMap.tileType = new int[b29][];
				for (int num64 = 0; num64 < b29; num64++)
				{
					sbyte b30 = msg.reader().readByte();
					TileMap.tileType[num64] = new int[b30];
					TileMap.tileIndex[num64] = new int[b30][];
					for (int num65 = 0; num65 < b30; num65++)
					{
						TileMap.tileType[num64][num65] = msg.reader().readInt();
						sbyte b31 = msg.reader().readByte();
						TileMap.tileIndex[num64][num65] = new int[b31];
						for (int num66 = 0; num66 < b31; num66++)
						{
							TileMap.tileIndex[num64][num65][num66] = msg.reader().readByte();
						}
					}
				}
				break;
			}
			case -81:
			{
				sbyte b12 = msg.reader().readByte();
				if (b12 == 0)
				{
					string src = msg.reader().readUTF();
					string src2 = msg.reader().readUTF();
					GameCanvas.panel.setTypeCombine();
					GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
					GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
					GameCanvas.panel.show();
				}
				if (b12 == 1)
				{
					GameCanvas.panel.vItemCombine.removeAllElements();
					sbyte b13 = msg.reader().readByte();
					for (int num28 = 0; num28 < b13; num28++)
					{
						sbyte b14 = msg.reader().readByte();
						for (int num29 = 0; num29 < Char.myCharz().arrItemBag.Length; num29++)
						{
							Item item = Char.myCharz().arrItemBag[num29];
							if (item != null && item.indexUI == b14)
							{
								item.isSelect = true;
								GameCanvas.panel.vItemCombine.addElement(item);
							}
						}
					}
					if (GameCanvas.panel.isShow)
					{
						GameCanvas.panel.setTabCombine();
					}
				}
				if (b12 == 2)
				{
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b12 == 3)
				{
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(0);
				}
				if (b12 == 4)
				{
					short iconID = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(1);
				}
				if (b12 == 5)
				{
					short iconID2 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID2;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(2);
				}
				if (b12 == 6)
				{
					short iconID3 = msg.reader().readShort();
					short iconID4 = msg.reader().readShort();
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(3);
					GameCanvas.panel.iconID1 = iconID3;
					GameCanvas.panel.iconID3 = iconID4;
				}
				if (b12 == 7)
				{
					short iconID5 = msg.reader().readShort();
					GameCanvas.panel.iconID3 = iconID5;
					GameCanvas.panel.combineSuccess = 0;
					GameCanvas.panel.setCombineEff(4);
				}
				if (b12 == 8)
				{
					GameCanvas.panel.iconID3 = -1;
					GameCanvas.panel.combineSuccess = 1;
					GameCanvas.panel.setCombineEff(4);
				}
				short num30 = 21;
				int num31 = 0;
				int num32 = 0;
				try
				{
					num30 = msg.reader().readShort();
					num31 = msg.reader().readShort();
					num32 = msg.reader().readShort();
					GameCanvas.panel.xS = num31 - GameScr.cmx;
					GameCanvas.panel.yS = num32 - GameScr.cmy;
				}
				catch (Exception)
				{
				}
				for (int num33 = 0; num33 < GameScr.vNpc.size(); num33++)
				{
					Npc npc = (Npc)GameScr.vNpc.elementAt(num33);
					if (npc.template.npcTemplateId == num30)
					{
						GameCanvas.panel.xS = npc.cx - GameScr.cmx;
						GameCanvas.panel.yS = npc.cy - GameScr.cmy;
						GameCanvas.panel.idNPC = num30;
						break;
					}
				}
				break;
			}
			case -80:
			{
				sbyte b50 = msg.reader().readByte();
				InfoDlg.hide();
				if (b50 == 0)
				{
					GameCanvas.panel.vFriend.removeAllElements();
					int num117 = msg.reader().readUnsignedByte();
					for (int num118 = 0; num118 < num117; num118++)
					{
						Char obj8 = new Char();
						obj8.charID = msg.reader().readInt();
						obj8.head = msg.reader().readShort();
						obj8.headICON = msg.reader().readShort();
						obj8.body = msg.reader().readShort();
						obj8.leg = msg.reader().readShort();
						obj8.bag = msg.reader().readShort();
						obj8.cName = msg.reader().readUTF();
						bool isOnline = msg.reader().readBoolean();
						InfoItem infoItem = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
						infoItem.charInfo = obj8;
						infoItem.isOnline = isOnline;
						GameCanvas.panel.vFriend.addElement(infoItem);
					}
					GameCanvas.panel.setTypeFriend();
					GameCanvas.panel.show();
				}
				if (b50 == 3)
				{
					MyVector vFriend = GameCanvas.panel.vFriend;
					int num119 = msg.reader().readInt();
					Res.outz("online offline id=" + num119);
					for (int num120 = 0; num120 < vFriend.size(); num120++)
					{
						InfoItem infoItem2 = (InfoItem)vFriend.elementAt(num120);
						if (infoItem2.charInfo != null && infoItem2.charInfo.charID == num119)
						{
							Res.outz("online= " + infoItem2.isOnline);
							infoItem2.isOnline = msg.reader().readBoolean();
							break;
						}
					}
				}
				if (b50 != 2)
				{
					break;
				}
				MyVector vFriend2 = GameCanvas.panel.vFriend;
				int num121 = msg.reader().readInt();
				for (int num122 = 0; num122 < vFriend2.size(); num122++)
				{
					InfoItem infoItem3 = (InfoItem)vFriend2.elementAt(num122);
					if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num121)
					{
						vFriend2.removeElement(infoItem3);
						break;
					}
				}
				if (GameCanvas.panel.isShow)
				{
					GameCanvas.panel.setTabFriend();
				}
				break;
			}
			case -99:
			{
				InfoDlg.hide();
				sbyte b61 = msg.reader().readByte();
				if (b61 == 0)
				{
					GameCanvas.panel.vEnemy.removeAllElements();
					int num137 = msg.reader().readUnsignedByte();
					for (int num138 = 0; num138 < num137; num138++)
					{
						Char obj10 = new Char();
						obj10.charID = msg.reader().readInt();
						obj10.head = msg.reader().readShort();
						obj10.headICON = msg.reader().readShort();
						obj10.body = msg.reader().readShort();
						obj10.leg = msg.reader().readShort();
						obj10.bag = msg.reader().readShort();
						obj10.cName = msg.reader().readUTF();
						InfoItem infoItem4 = new InfoItem(msg.reader().readUTF());
						bool flag10 = msg.reader().readBoolean();
						infoItem4.charInfo = obj10;
						infoItem4.isOnline = flag10;
						Res.outz("isonline = " + flag10);
						GameCanvas.panel.vEnemy.addElement(infoItem4);
					}
					GameCanvas.panel.setTypeEnemy();
					GameCanvas.panel.show();
				}
				break;
			}
			case -79:
			{
				InfoDlg.hide();
				int num104 = msg.reader().readInt();
				Char charMenu = GameCanvas.panel.charMenu;
				if (charMenu == null)
				{
					return;
				}
				charMenu.cPower = msg.reader().readLong();
				charMenu.currStrLevel = msg.reader().readUTF();
				break;
			}
			case -93:
			{
				short num153 = msg.reader().readShort();
				BgItem.newSmallVersion = new sbyte[num153];
				for (int num154 = 0; num154 < num153; num154++)
				{
					BgItem.newSmallVersion[num154] = msg.reader().readByte();
				}
				break;
			}
			case -77:
			{
				short num115 = msg.reader().readShort();
				SmallImage.newSmallVersion = new sbyte[num115];
				SmallImage.maxSmall = num115;
				SmallImage.imgNew = new Small[num115];
				for (int num116 = 0; num116 < num115; num116++)
				{
					SmallImage.newSmallVersion[num116] = msg.reader().readByte();
				}
				break;
			}
			case -76:
			{
				sbyte b15 = msg.reader().readByte();
				if (b15 == 0)
				{
					sbyte b16 = msg.reader().readByte();
					if (b16 <= 0)
					{
						return;
					}
					Char.myCharz().arrArchive = new Archivement[b16];
					for (int num34 = 0; num34 < b16; num34++)
					{
						Char.myCharz().arrArchive[num34] = new Archivement();
						Char.myCharz().arrArchive[num34].info1 = num34 + 1 + ". " + msg.reader().readUTF();
						Char.myCharz().arrArchive[num34].info2 = msg.reader().readUTF();
						Char.myCharz().arrArchive[num34].money = msg.reader().readShort();
						Char.myCharz().arrArchive[num34].isFinish = msg.reader().readBoolean();
						Char.myCharz().arrArchive[num34].isRecieve = msg.reader().readBoolean();
					}
					GameCanvas.panel.setTypeArchivement();
					GameCanvas.panel.show();
				}
				else if (b15 == 1)
				{
					int num35 = msg.reader().readUnsignedByte();
					if (Char.myCharz().arrArchive[num35] != null)
					{
						Char.myCharz().arrArchive[num35].isRecieve = true;
					}
				}
				break;
			}
			case -74:
			{
				if (ServerListScreen.stopDownload)
				{
					return;
				}
				if (!GameCanvas.isGetResourceFromServer())
				{
					Service.gI().getResource(3, null);
					SmallImage.loadBigRMS();
					SplashScr.imgLogo = null;
					if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
					{
						LoginScr.isContinueToLogin = true;
					}
					GameCanvas.loginScr = new LoginScr();
					GameCanvas.loginScr.switchToMe();
					return;
				}
				bool flag6 = true;
				Res.outz("1>>GET_IMAGE_SOURCE = " + msg.reader().available());
				sbyte b41 = msg.reader().readByte();
				Res.outz("2>GET_IMAGE_SOURCE = " + b41);
				if (b41 == 0)
				{
					int num92 = msg.reader().readInt();
					Res.outz("3>GET_IMAGE_SOURCE serverVersion = " + num92);
					string text3 = Rms.loadRMSString("ResVersion");
					int num93 = ((text3 == null || !(text3 != string.Empty)) ? (-1) : int.Parse(text3));
					Res.outz("4>>>GET_IMAGE_SOURCE: version>> " + text3 + " <> " + num93 + "!=" + num92);
					if (num93 == -1 || num93 != num92)
					{
						GameCanvas.serverScreen.show2();
					}
					else
					{
						SmallImage.loadBigRMS();
						SplashScr.imgLogo = null;
						ServerListScreen.loadScreen = true;
						Res.outz(">>>vo ne: " + GameCanvas.currentScreen);
						if (GameCanvas.currentScreen != GameCanvas.loginScr)
						{
							if (GameCanvas.serverScreen == null)
							{
								GameCanvas.serverScreen = new ServerListScreen();
							}
							GameCanvas.serverScreen.switchToMe();
						}
						else
						{
							if (GameCanvas.loginScr == null)
							{
								GameCanvas.loginScr = new LoginScr();
							}
							GameCanvas.loginScr.doLogin();
						}
					}
				}
				if (b41 == 1)
				{
					ServerListScreen.strWait = mResources.downloading_data;
					short nBig = msg.reader().readShort();
					ServerListScreen.nBig = nBig;
					Service.gI().getResource(2, null);
				}
				if (b41 == 2)
				{
					try
					{
						isLoadingData = true;
						GameCanvas.endDlg();
						ServerListScreen.demPercent++;
						ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
						string text4 = msg.reader().readUTF();
						Res.outz(">>>vo serverPath: " + text4);
						string[] array8 = Res.split(text4, "/", 0);
						string filename = "x" + mGraphics.zoomLevel + array8[array8.Length - 1];
						int num94 = msg.reader().readInt();
						sbyte[] data4 = new sbyte[num94];
						msg.reader().read(ref data4, 0, num94);
						Rms.saveRMS(filename, data4);
					}
					catch (Exception)
					{
						GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
					}
				}
				if (b41 == 3 && flag6)
				{
					isLoadingData = false;
					int num95 = msg.reader().readInt();
					Res.outz(">>>GET_IMAGE_SOURCE: lastVersion>> " + num95);
					Rms.saveRMSString("ResVersion", num95 + string.Empty);
					Service.gI().getResource(3, null);
					GameCanvas.endDlg();
					SplashScr.imgLogo = null;
					SmallImage.loadBigRMS();
					mSystem.gcc();
					ServerListScreen.bigOk = true;
					ServerListScreen.loadScreen = true;
					GameScr.gI().loadGameScr();
					GameScr.isLoadAllData = false;
					Service.gI().updateData();
					if (GameCanvas.currentScreen != GameCanvas.loginScr)
					{
						GameCanvas.serverScreen.switchToMe();
					}
				}
				break;
			}
			case -43:
			{
				sbyte itemAction = msg.reader().readByte();
				sbyte b18 = msg.reader().readByte();
				sbyte index2 = msg.reader().readByte();
				string info = msg.reader().readUTF();
				GameCanvas.panel.itemRequest(itemAction, info, b18, index2);
				break;
			}
			case -59:
			{
				sbyte typePK = msg.reader().readByte();
				GameScr.gI().player_vs_player(msg.reader().readInt(), msg.reader().readInt(), msg.reader().readUTF(), typePK);
				break;
			}
			case -62:
			{
				int num40 = msg.reader().readUnsignedByte();
				sbyte b21 = msg.reader().readByte();
				if (b21 <= 0)
				{
					break;
				}
				ClanImage clanImage = ClanImage.getClanImage((short)num40);
				if (clanImage == null)
				{
					break;
				}
				clanImage.idImage = new short[b21];
				for (int num41 = 0; num41 < b21; num41++)
				{
					clanImage.idImage[num41] = msg.reader().readShort();
					if (clanImage.idImage[num41] > 0)
					{
						SmallImage.vKeys.addElement(clanImage.idImage[num41] + string.Empty);
					}
				}
				break;
			}
			case -65:
			{
				InfoDlg.hide();
				int num86 = msg.reader().readInt();
				sbyte b38 = msg.reader().readByte();
				if (b38 == 0)
				{
					break;
				}
				if (Char.myCharz().charID == num86)
				{
					//isStopReadMessage = true;
					//GameScr.lockTick = 500;
					GameScr.gI().center = null;
					if (b38 == 0 || b38 == 1 || b38 == 3)
					{
						Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, true, (b38 != 1) ? b38 : Char.myCharz().cgender);
						//Teleport.addTeleport(p);
					}
					//if (b38 == 2)
					//{
					//	GameScr.lockTick = 50;
					//	Char.myCharz().hide();
					//}
				}
				else
				{
					Char obj5 = GameScr.findCharInMap(num86);
					if ((b38 == 0 || b38 == 1 || b38 == 3) && obj5 != null)
					{
						obj5.isUsePlane = true;
						Teleport teleport = new Teleport(obj5.cx, obj5.cy, obj5.head, obj5.cdir, 0, false, (b38 != 1) ? b38 : obj5.cgender);
						teleport.id = num86;
						teleport.Char = obj5;
						Teleport.addTeleport(teleport);
					}
					if (b38 == 2)
					{
						obj5.hide();
					}
				}
				break;
			}
			case -64:
			{
				int num47 = msg.reader().readInt();
				int num48 = msg.reader().readShort();
				obj = null;
				obj = ((num47 != Char.myCharz().charID) ? GameScr.findCharInMap(num47) : Char.myCharz());
				if (obj == null)
				{
					return;
				}
				obj.bag = num48;
				for (int num49 = 0; num49 < 54; num49++)
				{
					obj.removeEffChar(0, 201 + num49);
				}
				if (obj.bag >= 201 && obj.bag < 255)
				{
					Effect effect = new Effect(obj.bag, obj, 2, -1, 10, 1);
					effect.typeEff = 5;
					obj.addEffChar(effect);
				}
				Res.outz("cmd:-64 UPDATE BAG PLAER = " + ((obj != null) ? obj.cName : string.Empty) + num47 + " BAG ID= " + num48);
				if (num48 == 30 && obj.me)
				{
					GameScr.isPickNgocRong = true;
				}
				break;
			}
			case -63:
			{
				Res.outz("GET BAG");
				int num50 = msg.reader().readShort();
				sbyte b25 = msg.reader().readByte();
				ClanImage clanImage2 = new ClanImage();
				clanImage2.ID = num50;
				if (b25 > 0)
				{
					clanImage2.idImage = new short[b25];
					for (int num51 = 0; num51 < b25; num51++)
					{
						clanImage2.idImage[num51] = msg.reader().readShort();
						Res.outz("ID=  " + num50 + " frame= " + clanImage2.idImage[num51]);
					}
					ClanImage.idImages.put(num50 + string.Empty, clanImage2);
				}
				break;
			}
			case -57:
			{
				string strInvite = msg.reader().readUTF();
				int clanID = msg.reader().readInt();
				int code = msg.reader().readInt();
				GameScr.gI().clanInvite(strInvite, clanID, code);
				break;
			}
			case -51:
				InfoDlg.hide();
				readClanMsg(msg, 0);
				if (GameCanvas.panel.isMessage && GameCanvas.panel.type == 5)
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			case -53:
			{
				InfoDlg.hide();
				bool flag7 = false;
				int num100 = msg.reader().readInt();
				Res.outz("clanId= " + num100);
				if (num100 == -1)
				{
					flag7 = true;
					Char.myCharz().clan = null;
					ClanMessage.vMessage.removeAllElements();
					if (GameCanvas.panel.member != null)
					{
						GameCanvas.panel.member.removeAllElements();
					}
					if (GameCanvas.panel.myMember != null)
					{
						GameCanvas.panel.myMember.removeAllElements();
					}
					if (GameCanvas.currentScreen == GameScr.gI())
					{
						GameCanvas.panel.setTabClans();
					}
					return;
				}
				GameCanvas.panel.tabIcon = null;
				if (Char.myCharz().clan == null)
				{
					Char.myCharz().clan = new Clan();
				}
				Char.myCharz().clan.ID = num100;
				Char.myCharz().clan.name = msg.reader().readUTF();
				Char.myCharz().clan.slogan = msg.reader().readUTF();
				Char.myCharz().clan.imgID = msg.reader().readShort();
				Char.myCharz().clan.powerPoint = msg.reader().readUTF();
				Char.myCharz().clan.leaderName = msg.reader().readUTF();
				Char.myCharz().clan.currMember = msg.reader().readUnsignedByte();
				Char.myCharz().clan.maxMember = msg.reader().readUnsignedByte();
				Char.myCharz().role = msg.reader().readByte();
				Char.myCharz().clan.clanPoint = msg.reader().readInt();
				Char.myCharz().clan.level = msg.reader().readByte();
				GameCanvas.panel.myMember = new MyVector();
				for (int num101 = 0; num101 < Char.myCharz().clan.currMember; num101++)
				{
					Member member5 = new Member();
					member5.ID = msg.reader().readInt();
					member5.head = msg.reader().readShort();
					member5.headICON = msg.reader().readShort();
					member5.leg = msg.reader().readShort();
					member5.body = msg.reader().readShort();
					member5.name = msg.reader().readUTF();
					member5.role = msg.reader().readByte();
					member5.powerPoint = msg.reader().readUTF();
					member5.donate = msg.reader().readInt();
					member5.receive_donate = msg.reader().readInt();
					member5.clanPoint = msg.reader().readInt();
					member5.curClanPoint = msg.reader().readInt();
					member5.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.myMember.addElement(member5);
				}
				int num102 = msg.reader().readUnsignedByte();
				for (int num103 = 0; num103 < num102; num103++)
				{
					readClanMsg(msg, -1);
				}
				if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
				{
					GameCanvas.panel.setTabClans();
				}
				if (flag7)
				{
					GameCanvas.panel.setTabClans();
				}
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -537 MY CLAN INFO");
				break;
			}
			case -52:
			{
				sbyte b24 = msg.reader().readByte();
				if (b24 == 0)
				{
					Member member2 = new Member();
					member2.ID = msg.reader().readInt();
					member2.head = msg.reader().readShort();
					member2.headICON = msg.reader().readShort();
					member2.leg = msg.reader().readShort();
					member2.body = msg.reader().readShort();
					member2.name = msg.reader().readUTF();
					member2.role = msg.reader().readByte();
					member2.powerPoint = msg.reader().readUTF();
					member2.donate = msg.reader().readInt();
					member2.receive_donate = msg.reader().readInt();
					member2.clanPoint = msg.reader().readInt();
					member2.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					if (GameCanvas.panel.myMember == null)
					{
						GameCanvas.panel.myMember = new MyVector();
					}
					GameCanvas.panel.myMember.addElement(member2);
					GameCanvas.panel.initTabClans();
				}
				if (b24 == 1)
				{
					GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
					GameCanvas.panel.currentListLength--;
					GameCanvas.panel.initTabClans();
				}
				if (b24 == 2)
				{
					Member member3 = new Member();
					member3.ID = msg.reader().readInt();
					member3.head = msg.reader().readShort();
					member3.headICON = msg.reader().readShort();
					member3.leg = msg.reader().readShort();
					member3.body = msg.reader().readShort();
					member3.name = msg.reader().readUTF();
					member3.role = msg.reader().readByte();
					member3.powerPoint = msg.reader().readUTF();
					member3.donate = msg.reader().readInt();
					member3.receive_donate = msg.reader().readInt();
					member3.clanPoint = msg.reader().readInt();
					member3.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					for (int num46 = 0; num46 < GameCanvas.panel.myMember.size(); num46++)
					{
						Member member4 = (Member)GameCanvas.panel.myMember.elementAt(num46);
						if (member4.ID == member3.ID)
						{
							if (Char.myCharz().charID == member3.ID)
							{
								Char.myCharz().role = member3.role;
							}
							Member o = member3;
							GameCanvas.panel.myMember.removeElement(member4);
							GameCanvas.panel.myMember.insertElementAt(o, num46);
							return;
						}
					}
				}
				Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -52  MY CLAN UPDSTE");
				break;
			}
			case -50:
			{
				InfoDlg.hide();
				GameCanvas.panel.member = new MyVector();
				sbyte b17 = msg.reader().readByte();
				for (int num37 = 0; num37 < b17; num37++)
				{
					Member member = new Member();
					member.ID = msg.reader().readInt();
					member.head = msg.reader().readShort();
					member.headICON = msg.reader().readShort();
					member.leg = msg.reader().readShort();
					member.body = msg.reader().readShort();
					member.name = msg.reader().readUTF();
					member.role = msg.reader().readByte();
					member.powerPoint = msg.reader().readUTF();
					member.donate = msg.reader().readInt();
					member.receive_donate = msg.reader().readInt();
					member.clanPoint = msg.reader().readInt();
					member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
					GameCanvas.panel.member.addElement(member);
				}
				GameCanvas.panel.isViewMember = true;
				GameCanvas.panel.isSearchClan = false;
				GameCanvas.panel.isMessage = false;
				GameCanvas.panel.currentListLength = GameCanvas.panel.member.size() + 2;
				GameCanvas.panel.initTabClans();
				break;
			}
			case -47:
			{
				InfoDlg.hide();
				sbyte b8 = msg.reader().readByte();
				Res.outz("clan = " + b8);
				if (b8 == 0)
				{
					GameCanvas.panel.clanReport = mResources.cannot_find_clan;
					GameCanvas.panel.clans = null;
				}
				else
				{
					GameCanvas.panel.clans = new Clan[b8];
					Res.outz("clan search lent= " + GameCanvas.panel.clans.Length);
					for (int k = 0; k < GameCanvas.panel.clans.Length; k++)
					{
						GameCanvas.panel.clans[k] = new Clan();
						GameCanvas.panel.clans[k].ID = msg.reader().readInt();
						GameCanvas.panel.clans[k].name = msg.reader().readUTF();
						GameCanvas.panel.clans[k].slogan = msg.reader().readUTF();
						GameCanvas.panel.clans[k].imgID = msg.reader().readShort();
						GameCanvas.panel.clans[k].powerPoint = msg.reader().readUTF();
						GameCanvas.panel.clans[k].leaderName = msg.reader().readUTF();
						GameCanvas.panel.clans[k].currMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[k].maxMember = msg.reader().readUnsignedByte();
						GameCanvas.panel.clans[k].date = msg.reader().readInt();
					}
				}
				GameCanvas.panel.isSearchClan = true;
				GameCanvas.panel.isViewMember = false;
				GameCanvas.panel.isMessage = false;
				if (GameCanvas.panel.isSearchClan)
				{
					GameCanvas.panel.initTabClans();
				}
				break;
			}
			case -46:
			{
				InfoDlg.hide();
				sbyte b63 = msg.reader().readByte();
				if (b63 == 1 || b63 == 3)
				{
					GameCanvas.endDlg();
					ClanImage.vClanImage.removeAllElements();
					int num143 = msg.reader().readShort();
					for (int num144 = 0; num144 < num143; num144++)
					{
						ClanImage clanImage3 = new ClanImage();
						clanImage3.ID = msg.reader().readShort();
						clanImage3.name = msg.reader().readUTF();
						clanImage3.xu = msg.reader().readInt();
						clanImage3.luong = msg.reader().readInt();
						if (!ClanImage.isExistClanImage(clanImage3.ID))
						{
							ClanImage.addClanImage(clanImage3);
							continue;
						}
						ClanImage.getClanImage((short)clanImage3.ID).name = clanImage3.name;
						ClanImage.getClanImage((short)clanImage3.ID).xu = clanImage3.xu;
						ClanImage.getClanImage((short)clanImage3.ID).luong = clanImage3.luong;
					}
					if (Char.myCharz().clan != null)
					{
						GameCanvas.panel.changeIcon();
					}
				}
				if (b63 == 4)
				{
					Char.myCharz().clan.imgID = msg.reader().readShort();
					Char.myCharz().clan.slogan = msg.reader().readUTF();
				}
				break;
			}
			case -61:
			{
				int num135 = msg.reader().readInt();
				if (num135 != Char.myCharz().charID)
				{
					if (GameScr.findCharInMap(num135) != null)
					{
						GameScr.findCharInMap(num135).clanID = msg.reader().readInt();
						if (GameScr.findCharInMap(num135).clanID == -2)
						{
							GameScr.findCharInMap(num135).isCopy = true;
						}
					}
				}
				else if (Char.myCharz().clan != null)
				{
					Char.myCharz().clan.ID = msg.reader().readInt();
				}
				break;
			}
			case -42:
				Char.myCharz().cHPGoc = msg.readInt3Byte();
				Char.myCharz().cMPGoc = msg.readInt3Byte();
				Char.myCharz().cDamGoc = msg.reader().readInt();
				Char.myCharz().cHPFull = msg.reader().readLong();
				Char.myCharz().cMPFull = msg.reader().readLong();
				Char.myCharz().cHP = msg.reader().readLong();
				Char.myCharz().cMP = msg.reader().readLong();
				Char.myCharz().cspeed = msg.reader().readByte();
				Char.myCharz().hpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().mpFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().damFrom1000TiemNang = msg.reader().readByte();
				Char.myCharz().cDamFull = msg.reader().readLong();
				Char.myCharz().cDefull = msg.reader().readLong();
				Char.myCharz().cCriticalFull = msg.reader().readByte();
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().expForOneAdd = msg.reader().readShort();
				Char.myCharz().cDefGoc = msg.reader().readInt();
				Char.myCharz().cCriticalGoc = msg.reader().readByte();
				Char.myCharz().cGiamST = msg.reader().readByte();
				Char.myCharz().cCritDameFull = msg.reader().readShort();
				InfoDlg.hide();
				break;
			case 1:
			{
				bool flag9 = msg.reader().readBool();
				Res.outz("isRes= " + flag9);
				if (!flag9)
				{
					GameCanvas.startOKDlg(msg.reader().readUTF());
					break;
				}
				GameCanvas.loginScr.isLogin2 = false;
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				GameCanvas.endDlg();
				GameCanvas.loginScr.doLogin();
				break;
			}
			case 2:
				Char.isLoadingMap = false;
				LoginScr.isLoggingIn = false;
				if (!GameScr.isLoadAllData)
				{
					GameScr.gI().initSelectChar();
				}
				BgItem.clearHashTable();
				GameCanvas.endDlg();
				CreateCharScr.isCreateChar = true;
				CreateCharScr.gI().switchToMe();
				break;
			case -107:
			{
				sbyte b27 = msg.reader().readByte();
				if (b27 == 0)
				{
					Char.myCharz().havePet = false;
				}
				if (b27 == 1)
				{
					Char.myCharz().havePet = true;
				}
				if (b27 != 2)
				{
					break;
				}
				InfoDlg.hide();
				Char.myPetz().head = msg.reader().readShort();
				Char.myPetz().setDefaultPart();
				int num53 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num53);
				Char.myPetz().arrItemBody = new Item[num53];
				for (int num54 = 0; num54 < num53; num54++)
				{
					short num55 = msg.reader().readShort();
					Res.outz("template id= " + num55);
					if (num55 == -1)
					{
						continue;
					}
					Res.outz("1");
					Char.myPetz().arrItemBody[num54] = new Item();
					Char.myPetz().arrItemBody[num54].template = ItemTemplates.get(num55);
					int num56 = Char.myPetz().arrItemBody[num54].template.type;
					Char.myPetz().arrItemBody[num54].quantity = msg.reader().readInt();
					Res.outz("3");
					Char.myPetz().arrItemBody[num54].info = msg.reader().readUTF();
					Char.myPetz().arrItemBody[num54].content = msg.reader().readUTF();
					int num57 = msg.reader().readUnsignedByte();
					Res.outz("option size= " + num57);
					if (num57 != 0)
					{
						Char.myPetz().arrItemBody[num54].itemOption = new ItemOption[num57];
						for (int num58 = 0; num58 < Char.myPetz().arrItemBody[num54].itemOption.Length; num58++)
						{
							ItemOption itemOption2 = readItemOption(msg);
							if (itemOption2 != null)
							{
								Char.myPetz().arrItemBody[num54].itemOption[num58] = itemOption2;
							}
						}
					}
					switch (num56)
					{
					case 0:
						Char.myPetz().body = Char.myPetz().arrItemBody[num54].template.part;
						break;
					case 1:
						Char.myPetz().leg = Char.myPetz().arrItemBody[num54].template.part;
						break;
					}
				}
				Char.myPetz().cHP = msg.reader().readLong();
				Char.myPetz().cHPFull = msg.reader().readLong();
				Char.myPetz().cMP = msg.reader().readLong();
				Char.myPetz().cMPFull = msg.reader().readLong();
				Char.myPetz().cDamFull = msg.reader().readLong();
				Char.myPetz().cName = msg.reader().readUTF();
				Char.myPetz().currStrLevel = msg.reader().readUTF();
				Char.myPetz().cPower = msg.reader().readLong();
				Char.myPetz().cTiemNang = msg.reader().readLong();
				Char.myPetz().petStatus = msg.reader().readByte();
				Char.myPetz().cStamina = msg.reader().readShort();
				Char.myPetz().cMaxStamina = msg.reader().readShort();
				Char.myPetz().cCriticalFull = msg.reader().readByte();
				Char.myPetz().cDefull = msg.reader().readLong();
				Char.myPetz().arrPetSkill = new Skill[msg.reader().readByte()];
				Res.outz("SKILLENT = " + Char.myPetz().arrPetSkill);
				for (int num59 = 0; num59 < Char.myPetz().arrPetSkill.Length; num59++)
				{
					short num60 = msg.reader().readShort();
					if (num60 != -1)
					{
						Char.myPetz().arrPetSkill[num59] = Skills.get(num60);
						continue;
					}
					Char.myPetz().arrPetSkill[num59] = new Skill();
					Char.myPetz().arrPetSkill[num59].template = null;
					Char.myPetz().arrPetSkill[num59].moreInfo = msg.reader().readUTF();
				}
				Char.myPetz().cGiamST = msg.reader().readByte();
				Char.myPetz().cCritDameFull = msg.reader().readShort();
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					GameCanvas.panel2 = new Panel();
					GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
					GameCanvas.panel2.setTypeBodyOnly();
					GameCanvas.panel2.show();
					GameCanvas.panel.setTypePetMain();
					GameCanvas.panel.show();
				}
				else
				{
					GameCanvas.panel.tabName[21] = mResources.petMainTab;
					GameCanvas.panel.setTypePetMain();
					GameCanvas.panel.show();
				}
				break;
			}
			case -37:
			{
				sbyte b37 = msg.reader().readByte();
				Res.outz("cAction= " + b37);
				if (b37 != 0)
				{
					break;
				}
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().setDefaultPart();
				int num80 = msg.reader().readUnsignedByte();
				Res.outz("num body = " + num80);
				Char.myCharz().arrItemBody = new Item[num80];
				for (int num81 = 0; num81 < num80; num81++)
				{
					short num82 = msg.reader().readShort();
					if (num82 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBody[num81] = new Item();
					Char.myCharz().arrItemBody[num81].template = ItemTemplates.get(num82);
					int num83 = Char.myCharz().arrItemBody[num81].template.type;
					Char.myCharz().arrItemBody[num81].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBody[num81].info = msg.reader().readUTF();
					Char.myCharz().arrItemBody[num81].content = msg.reader().readUTF();
					int num84 = msg.reader().readUnsignedByte();
					if (num84 != 0)
					{
						Char.myCharz().arrItemBody[num81].itemOption = new ItemOption[num84];
						for (int num85 = 0; num85 < Char.myCharz().arrItemBody[num81].itemOption.Length; num85++)
						{
							ItemOption itemOption4 = readItemOption(msg);
							if (itemOption4 != null)
							{
								Char.myCharz().arrItemBody[num81].itemOption[num85] = itemOption4;
							}
						}
					}
					switch (num83)
					{
					case 0:
						Char.myCharz().body = Char.myCharz().arrItemBody[num81].template.part;
						break;
					case 1:
						Char.myCharz().leg = Char.myCharz().arrItemBody[num81].template.part;
						break;
					}
				}
				break;
			}
			case -36:
			{
				sbyte b9 = msg.reader().readByte();
				Res.outz("cAction= " + b9);
				GameScr.isudungCapsun4 = false;
				GameScr.isudungCapsun3 = false;
				if (b9 == 0)
				{
					int num21 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBag = new Item[num21];
					GameScr.hpPotion = 0;
					Res.outz("numC=" + num21);
					for (int l = 0; l < num21; l++)
					{
						short num22 = msg.reader().readShort();
						if (num22 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBag[l] = new Item();
						Char.myCharz().arrItemBag[l].template = ItemTemplates.get(num22);
						Char.myCharz().arrItemBag[l].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBag[l].info = msg.reader().readUTF();
						Char.myCharz().arrItemBag[l].content = msg.reader().readUTF();
						Char.myCharz().arrItemBag[l].indexUI = l;
						int num23 = msg.reader().readUnsignedByte();
						if (num23 != 0)
						{
							Char.myCharz().arrItemBag[l].itemOption = new ItemOption[num23];
							for (int m = 0; m < Char.myCharz().arrItemBag[l].itemOption.Length; m++)
							{
								ItemOption itemOption = readItemOption(msg);
								if (itemOption != null)
								{
									Char.myCharz().arrItemBag[l].itemOption[m] = itemOption;
								}
							}
							Char.myCharz().arrItemBag[l].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[l]);
						}
						if (Char.myCharz().arrItemBag[l].template.type == 11)
						{
						}
						if (Char.myCharz().arrItemBag[l].template.type == 6)
						{
							GameScr.hpPotion += Char.myCharz().arrItemBag[l].quantity;
						}
						if (Char.myCharz().arrItemBag[l].template.id == 194)
						{
							GameScr.isudungCapsun4 = Char.myCharz().arrItemBag[l].quantity > 0;
						}
						else if (Char.myCharz().arrItemBag[l].template.id == 193 && !GameScr.isudungCapsun4)
						{
							GameScr.isudungCapsun3 = Char.myCharz().arrItemBag[l].quantity > 0;
						}
					}
				}
				if (b9 == 2)
				{
					sbyte b10 = msg.reader().readByte();
					int num24 = msg.reader().readInt();
					int quantity = Char.myCharz().arrItemBag[b10].quantity;
					int id = Char.myCharz().arrItemBag[b10].template.id;
					Char.myCharz().arrItemBag[b10].quantity = num24;
					if (Char.myCharz().arrItemBag[b10].quantity < quantity && Char.myCharz().arrItemBag[b10].template.type == 6)
					{
						GameScr.hpPotion -= quantity - Char.myCharz().arrItemBag[b10].quantity;
					}
					if (Char.myCharz().arrItemBag[b10].quantity == 0)
					{
						Char.myCharz().arrItemBag[b10] = null;
					}
					switch (id)
					{
					case 194:
						GameScr.isudungCapsun4 = num24 > 0;
						break;
					case 193:
						GameScr.isudungCapsun3 = num24 > 0;
						break;
					}
				}
				break;
			}
			case -35:
			{
				sbyte b64 = msg.reader().readByte();
				Res.outz("cAction= " + b64);
				if (b64 == 0)
				{
					int num148 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemBox = new Item[num148];
					GameCanvas.panel.hasUse = 0;
					for (int num149 = 0; num149 < num148; num149++)
					{
						short num150 = msg.reader().readShort();
						if (num150 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemBox[num149] = new Item();
						Char.myCharz().arrItemBox[num149].template = ItemTemplates.get(num150);
						Char.myCharz().arrItemBox[num149].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBox[num149].info = msg.reader().readUTF();
						Char.myCharz().arrItemBox[num149].content = msg.reader().readUTF();
						int num151 = msg.reader().readUnsignedByte();
						if (num151 != 0)
						{
							Char.myCharz().arrItemBox[num149].itemOption = new ItemOption[num151];
							for (int num152 = 0; num152 < Char.myCharz().arrItemBox[num149].itemOption.Length; num152++)
							{
								ItemOption itemOption6 = readItemOption(msg);
								if (itemOption6 != null)
								{
									Char.myCharz().arrItemBox[num149].itemOption[num152] = itemOption6;
								}
							}
						}
						GameCanvas.panel.hasUse++;
					}
				}
				if (b64 == 1)
				{
					bool isBoxClan = false;
					try
					{
						sbyte b65 = msg.reader().readByte();
						if (b65 == 1)
						{
							isBoxClan = true;
						}
					}
					catch (Exception)
					{
					}
					GameCanvas.panel.setTypeBox();
					GameCanvas.panel.isBoxClan = isBoxClan;
					GameCanvas.panel.show();
				}
				if (b64 == 2)
				{
					sbyte b66 = msg.reader().readByte();
					int quantity2 = msg.reader().readInt();
					Char.myCharz().arrItemBox[b66].quantity = quantity2;
					if (Char.myCharz().arrItemBox[b66].quantity == 0)
					{
						Char.myCharz().arrItemBox[b66] = null;
					}
				}
				break;
			}
			case -45:
			{
				sbyte b51 = msg.reader().readByte();
				int num123 = msg.reader().readInt();
				short num124 = msg.reader().readShort();
				Res.outz(">.SKILL_NOT_FOCUS      skillNotFocusID: " + num124 + " skill type= " + b51 + "   player use= " + num123);
				if (b51 == 20)
				{
					sbyte b52 = msg.reader().readByte();
					sbyte dir = msg.reader().readByte();
					short timeGong = msg.reader().readShort();
					bool isFly = ((msg.reader().readByte() != 0) ? true : false);
					sbyte typePaint = msg.reader().readByte();
					sbyte typeItem = -1;
					try
					{
						typeItem = msg.reader().readByte();
					}
					catch (Exception)
					{
					}
					Res.outz(">.SKILL_NOT_FOCUS  skill typeFrame= " + b52);
					obj = ((Char.myCharz().charID != num123) ? GameScr.findCharInMap(num123) : Char.myCharz());
					obj.SetSkillPaint_NEW(num124, isFly, b52, typePaint, dir, timeGong, typeItem);
				}
				if (b51 == 21)
				{
					Point point = new Point();
					point.x = msg.reader().readShort();
					point.y = msg.reader().readShort();
					short timeDame = msg.reader().readShort();
					short rangeDame = msg.reader().readShort();
					sbyte typePaint2 = 0;
					sbyte typeItem2 = -1;
					Point[] array9 = null;
					obj = ((Char.myCharz().charID != num123) ? GameScr.findCharInMap(num123) : Char.myCharz());
					try
					{
						typePaint2 = msg.reader().readByte();
						sbyte b53 = msg.reader().readByte();
						if (b53 > 0)
						{
							array9 = new Point[b53];
							for (int num125 = 0; num125 < array9.Length; num125++)
							{
								array9[num125] = new Point();
								array9[num125].type = msg.reader().readByte();
								if (array9[num125].type == 0)
								{
									array9[num125].id = msg.reader().readByte();
								}
								else
								{
									array9[num125].id = msg.reader().readInt();
								}
							}
						}
					}
					catch (Exception)
					{
					}
					try
					{
						typeItem2 = msg.reader().readByte();
					}
					catch (Exception)
					{
					}
					Res.outz(">.SKILL_NOT_FOCUS  skill targetDame= " + point.x + ":" + point.y + "    c:" + obj.cx + ":" + obj.cy + "   cdir:" + obj.cdir);
					obj.SetSkillPaint_STT(1, num124, point, timeDame, rangeDame, typePaint2, array9, typeItem2);
				}
				if (b51 == 0)
				{
					Res.outz("id use= " + num123);
					if (Char.myCharz().charID != num123)
					{
						obj = GameScr.findCharInMap(num123);
						if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
						{
							obj.setSkillPaint(GameScr.sks[num124], 0);
						}
						else
						{
							obj.setSkillPaint(GameScr.sks[num124], 1);
							obj.delayFall = 20;
						}
					}
					else
					{
						Char.myCharz().saveLoadPreviousSkill();
						Res.outz("LOAD LAST SKILL");
					}
					sbyte b54 = msg.reader().readByte();
					Res.outz("npc size= " + b54);
					for (int num126 = 0; num126 < b54; num126++)
					{
						sbyte b55 = msg.reader().readByte();
						sbyte b56 = msg.reader().readByte();
						Res.outz("index= " + b55);
						if (num124 >= 42 && num124 <= 48)
						{
							((Mob)GameScr.vMob.elementAt(b55)).isFreez = true;
							((Mob)GameScr.vMob.elementAt(b55)).seconds = b56;
							((Mob)GameScr.vMob.elementAt(b55)).last = (((Mob)GameScr.vMob.elementAt(b55)).cur = mSystem.currentTimeMillis());
						}
					}
					sbyte b57 = msg.reader().readByte();
					for (int num127 = 0; num127 < b57; num127++)
					{
						int num128 = msg.reader().readInt();
						sbyte b58 = msg.reader().readByte();
						Res.outz("player ID= " + num128 + " my ID= " + Char.myCharz().charID);
						if (num124 < 42 || num124 > 48)
						{
							continue;
						}
						if (num128 == Char.myCharz().charID)
						{
							if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
							{
								GameScr.gI().isFreez = true;
								Char.myCharz().isFreez = true;
								Char.myCharz().freezSeconds = b58;
								Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
								Char.myCharz().isLockMove = true;
							}
						}
						else
						{
							obj = GameScr.findCharInMap(num128);
							if (obj != null && !obj.isFlyAndCharge && !obj.isStandAndCharge)
							{
								obj.isFreez = true;
								obj.seconds = b58;
								obj.freezSeconds = b58;
								obj.lastFreez = (GameScr.findCharInMap(num128).currFreez = mSystem.currentTimeMillis());
							}
						}
					}
				}
				if (b51 == 1 && num123 != Char.myCharz().charID)
				{
					try
					{
						GameScr.findCharInMap(num123).isCharge = true;
					}
					catch (Exception)
					{
					}
				}
				if (b51 == 3)
				{
					if (num123 == Char.myCharz().charID)
					{
						Char.myCharz().isCharge = false;
						SoundMn.gI().taitaoPause();
						Char.myCharz().saveLoadPreviousSkill();
					}
					else
					{
						GameScr.findCharInMap(num123).isCharge = false;
					}
				}
				if (b51 == 4)
				{
					if (num123 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort() - 1000;
						Char.myCharz().last = mSystem.currentTimeMillis();
						Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
					}
					else if (GameScr.findCharInMap(num123) != null)
					{
						Char obj9 = GameScr.findCharInMap(num123);
						switch (obj9.cgender)
						{
						case 0:
							if (TileMap.mapID != 170)
							{
								obj.useChargeSkill(false);
								break;
							}
							if (num124 >= 77 && num124 <= 83)
							{
								obj.useChargeSkill(true);
							}
							if (num124 >= 70 && num124 <= 76)
							{
								obj.useChargeSkill(false);
							}
							break;
						case 1:
						{
							if (TileMap.mapID != 170)
							{
								obj.useChargeSkill(true);
								break;
							}
							bool isGround2 = true;
							if (num124 >= 70 && num124 <= 76)
							{
								isGround2 = false;
							}
							if (num124 >= 77 && num124 <= 83)
							{
								isGround2 = true;
							}
							obj.useChargeSkill(isGround2);
							break;
						}
						default:
							if (TileMap.mapID == 170)
							{
								bool isGround = true;
								if (num124 >= 70 && num124 <= 76)
								{
									isGround = false;
								}
								if (num124 >= 77 && num124 <= 83)
								{
									isGround = true;
								}
								obj.useChargeSkill(isGround);
							}
							break;
						}
						obj.skillTemplateId = num124;
						if (num124 >= 70 && num124 <= 76)
						{
							obj.isUseSkillAfterCharge = true;
						}
						obj.seconds = msg.reader().readShort();
						obj.last = mSystem.currentTimeMillis();
					}
				}
				if (b51 == 5)
				{
					if (num123 == Char.myCharz().charID)
					{
						Char.myCharz().stopUseChargeSkill();
					}
					else if (GameScr.findCharInMap(num123) != null)
					{
						GameScr.findCharInMap(num123).stopUseChargeSkill();
					}
				}
				if (b51 == 6)
				{
					if (num123 == Char.myCharz().charID)
					{
						Char.myCharz().setAutoSkillPaint(GameScr.sks[num124], 0);
					}
					else if (GameScr.findCharInMap(num123) != null)
					{
						GameScr.findCharInMap(num123).setAutoSkillPaint(GameScr.sks[num124], 0);
						SoundMn.gI().gong();
					}
				}
				if (b51 == 7)
				{
					if (num123 == Char.myCharz().charID)
					{
						Char.myCharz().seconds = msg.reader().readShort();
						Res.outz("second = " + Char.myCharz().seconds);
						Char.myCharz().last = mSystem.currentTimeMillis();
					}
					else if (GameScr.findCharInMap(num123) != null)
					{
						GameScr.findCharInMap(num123).useChargeSkill(true);
						GameScr.findCharInMap(num123).seconds = msg.reader().readShort();
						GameScr.findCharInMap(num123).last = mSystem.currentTimeMillis();
						SoundMn.gI().gong();
					}
				}
				if (b51 == 8 && num123 != Char.myCharz().charID && GameScr.findCharInMap(num123) != null)
				{
					GameScr.findCharInMap(num123).setAutoSkillPaint(GameScr.sks[num124], 0);
				}
				break;
			}
			case -44:
			{
				bool flag5 = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag5 = true;
				}
				sbyte b34 = msg.reader().readByte();
				int num68 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop = new Item[num68][];
				GameCanvas.panel.shopTabName = new string[num68 + ((!flag5) ? 1 : 0)][];
				for (int num69 = 0; num69 < GameCanvas.panel.shopTabName.Length; num69++)
				{
					GameCanvas.panel.shopTabName[num69] = new string[2];
				}
				if (b34 == 2)
				{
					GameCanvas.panel.maxPageShop = new int[num68];
					GameCanvas.panel.currPageShop = new int[num68];
				}
				if (!flag5)
				{
					GameCanvas.panel.shopTabName[num68] = mResources.inventory;
				}
				for (int num70 = 0; num70 < num68; num70++)
				{
					string[] array4 = Res.split(msg.reader().readUTF(), "\n", 0);
					if (b34 == 2)
					{
						GameCanvas.panel.maxPageShop[num70] = msg.reader().readUnsignedByte();
					}
					if (array4.Length == 2)
					{
						GameCanvas.panel.shopTabName[num70] = array4;
					}
					if (array4.Length == 1)
					{
						GameCanvas.panel.shopTabName[num70][0] = array4[0];
						GameCanvas.panel.shopTabName[num70][1] = string.Empty;
					}
					int num71 = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[num70] = new Item[num71];
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					if (b34 == 1)
					{
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
					}
					for (int num72 = 0; num72 < num71; num72++)
					{
						short num73 = msg.reader().readShort();
						if (num73 == -1)
						{
							continue;
						}
						Char.myCharz().arrItemShop[num70][num72] = new Item();
						Char.myCharz().arrItemShop[num70][num72].template = ItemTemplates.get(num73);
						if (b34 == 8)
						{
							Char.myCharz().arrItemShop[num70][num72].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].quantity = msg.reader().readInt();
						}
						else if (b34 == 4)
						{
							Char.myCharz().arrItemShop[num70][num72].reason = msg.reader().readUTF();
						}
						else if (b34 == 0)
						{
							Char.myCharz().arrItemShop[num70][num72].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].buyGold = msg.reader().readInt();
						}
						else if (b34 == 1)
						{
							Char.myCharz().arrItemShop[num70][num72].powerRequire = msg.reader().readLong();
						}
						else if (b34 == 2)
						{
							Char.myCharz().arrItemShop[num70][num72].itemId = msg.reader().readShort();
							Char.myCharz().arrItemShop[num70][num72].buyCoin = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].buyGold = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].buyType = msg.reader().readByte();
							Char.myCharz().arrItemShop[num70][num72].quantity = msg.reader().readInt();
							Char.myCharz().arrItemShop[num70][num72].isMe = msg.reader().readByte();
						}
						else if (b34 == 3)
						{
							Char.myCharz().arrItemShop[num70][num72].isBuySpec = true;
							Char.myCharz().arrItemShop[num70][num72].iconSpec = msg.reader().readShort();
							Char.myCharz().arrItemShop[num70][num72].buySpec = msg.reader().readInt();
						}
						int num74 = msg.reader().readUnsignedByte();
						if (num74 != 0)
						{
							Char.myCharz().arrItemShop[num70][num72].itemOption = new ItemOption[num74];
							for (int num75 = 0; num75 < Char.myCharz().arrItemShop[num70][num72].itemOption.Length; num75++)
							{
								ItemOption itemOption3 = readItemOption(msg);
								if (itemOption3 != null)
								{
									Char.myCharz().arrItemShop[num70][num72].itemOption[num75] = itemOption3;
									Char.myCharz().arrItemShop[num70][num72].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num70][num72]);
								}
							}
						}
						sbyte b35 = msg.reader().readByte();
						Char.myCharz().arrItemShop[num70][num72].newItem = ((b35 != 0) ? true : false);
						sbyte b36 = msg.reader().readByte();
						if (b36 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[num70][num72].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
						if (b34 == 2 && GameMidlet.intVERSION >= 237)
						{
							Char.myCharz().arrItemShop[num70][num72].nameNguoiKyGui = msg.reader().readUTF();
							Res.err("nguoi ki gui  " + Char.myCharz().arrItemShop[num70][num72].nameNguoiKyGui);
						}
					}
				}
				if (flag5)
				{
					if (b34 != 2)
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
						GameCanvas.panel2.setTypeBodyOnly();
						GameCanvas.panel2.show();
					}
					else
					{
						GameCanvas.panel2 = new Panel();
						GameCanvas.panel2.setTypeKiGuiOnly();
						GameCanvas.panel2.show();
					}
				}
				GameCanvas.panel.tabName[1] = GameCanvas.panel.shopTabName;
				if (b34 == 2)
				{
					string[][] array5 = GameCanvas.panel.tabName[1];
					if (flag5)
					{
						GameCanvas.panel.tabName[1] = new string[4][]
						{
							array5[0],
							array5[1],
							array5[2],
							array5[3]
						};
					}
					else
					{
						GameCanvas.panel.tabName[1] = new string[5][]
						{
							array5[0],
							array5[1],
							array5[2],
							array5[3],
							array5[4]
						};
					}
				}
				GameCanvas.panel.setTypeShop(b34);
				GameCanvas.panel.show();
				break;
			}
			case -41:
			{
				sbyte b26 = msg.reader().readByte();
				Char.myCharz().strLevel = new string[b26];
				for (int num52 = 0; num52 < b26; num52++)
				{
					string text = msg.reader().readUTF();
					Char.myCharz().strLevel[num52] = text;
				}
				Res.outz("---   xong  level caption cmd : " + msg.command);
				break;
			}
			case -34:
			{
				sbyte b19 = msg.reader().readByte();
				Res.outz("act= " + b19);
				if (b19 == 0 && GameScr.gI().magicTree != null)
				{
					Res.outz("toi duoc day");
					MagicTree magicTree = GameScr.gI().magicTree;
					magicTree.id = msg.reader().readShort();
					magicTree.name = msg.reader().readUTF();
					magicTree.name = Res.changeString(magicTree.name);
					magicTree.x = msg.reader().readShort();
					magicTree.y = msg.reader().readShort();
					magicTree.level = msg.reader().readByte();
					magicTree.currPeas = msg.reader().readShort();
					magicTree.maxPeas = msg.reader().readShort();
					Res.outz("curr Peas= " + magicTree.currPeas);
					magicTree.strInfo = msg.reader().readUTF();
					magicTree.seconds = msg.reader().readInt();
					magicTree.timeToRecieve = magicTree.seconds;
					sbyte b20 = msg.reader().readByte();
					magicTree.peaPostionX = new int[b20];
					magicTree.peaPostionY = new int[b20];
					for (int num39 = 0; num39 < b20; num39++)
					{
						magicTree.peaPostionX[num39] = msg.reader().readByte();
						magicTree.peaPostionY[num39] = msg.reader().readByte();
					}
					magicTree.isUpdate = msg.reader().readBool();
					magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
				}
				if (b19 == 1)
				{
					myVector = new MyVector();
					try
					{
						while (msg.reader().available() > 0)
						{
							string caption = msg.reader().readUTF();
							myVector.addElement(new Command(caption, GameCanvas.instance, 888392, null));
						}
					}
					catch (Exception ex7)
					{
						Cout.println("Loi MAGIC_TREE " + ex7.ToString());
					}
					GameCanvas.menu.startAt(myVector, 3);
				}
				if (b19 == 2)
				{
					GameScr.gI().magicTree.remainPeas = msg.reader().readShort();
					GameScr.gI().magicTree.seconds = msg.reader().readInt();
					GameScr.gI().magicTree.last = (GameScr.gI().magicTree.cur = mSystem.currentTimeMillis());
					GameScr.gI().magicTree.isUpdateTree = true;
					GameScr.gI().magicTree.isPeasEffect = true;
				}
				break;
			}
			case 11:
			{
				GameCanvas.debug("SA9", 2);
				int num25 = msg.reader().readShort();
				sbyte b11 = msg.reader().readByte();
				if (b11 != 0)
				{
					Mob.arrMobTemplate[num25].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b11);
				}
				else
				{
					Mob.arrMobTemplate[num25].data.readData(NinjaUtil.readByteArray(msg));
				}
				for (int n = 0; n < GameScr.vMob.size(); n++)
				{
					mob = (Mob)GameScr.vMob.elementAt(n);
					if (mob.templateId == num25)
					{
						mob.w = Mob.arrMobTemplate[num25].data.width;
						mob.h = Mob.arrMobTemplate[num25].data.height;
					}
				}
				sbyte[] array2 = NinjaUtil.readByteArray(msg);
				Image img = Image.createImage(array2, 0, array2.Length);
				Mob.arrMobTemplate[num25].data.img = img;
				int num26 = msg.reader().readByte();
				Mob.arrMobTemplate[num25].data.typeData = num26;
				if (num26 == 1 || num26 == 2)
				{
					readFrameBoss(msg, num25);
				}
				break;
			}
			case -69:
				Char.myCharz().cMaxStamina = msg.reader().readShort();
				break;
			case -68:
				Char.myCharz().cStamina = msg.reader().readShort();
				break;
			case -67:
			{
				demCount += 1f;
				int num156 = msg.reader().readInt();
				Res.outz("RECIEVE  hinh small: " + num156);
				sbyte[] array17 = null;
				try
				{
					array17 = NinjaUtil.readByteArray(msg);
					Res.outz(">SIZE CHECK= " + array17.Length);
					if (num156 == 3896)
					{
					}
					SmallImage.imgNew[num156].img = createImage(array17);
				}
				catch (Exception)
				{
					array17 = null;
					SmallImage.imgNew[num156].img = Image.createRGBImage(new int[1], 1, 1, true);
				}
				if (array17 != null && mGraphics.zoomLevel > 1)
				{
					Rms.saveRMS(mGraphics.zoomLevel + "Small" + num156, array17);
				}
				break;
			}
			case -66:
			{
				short id3 = msg.reader().readShort();
				sbyte[] data5 = NinjaUtil.readByteArray(msg);
				EffectData effDataById = Effect.getEffDataById(id3);
				sbyte b67 = msg.reader().readSByte();
				if (b67 == 0)
				{
					effDataById.readData(data5);
				}
				else
				{
					effDataById.readDataNewBoss(data5, b67);
				}
				sbyte[] array15 = NinjaUtil.readByteArray(msg);
				effDataById.img = Image.createImage(array15, 0, array15.Length);
				break;
			}
			case -32:
			{
				short num139 = msg.reader().readShort();
				int num140 = msg.reader().readInt();
				sbyte[] array11 = null;
				Image image = null;
				try
				{
					array11 = new sbyte[num140];
					for (int num141 = 0; num141 < num140; num141++)
					{
						array11[num141] = msg.reader().readByte();
					}
					image = Image.createImage(array11, 0, num140);
					BgItem.imgNew.put(num139 + string.Empty, image);
				}
				catch (Exception)
				{
					array11 = null;
					BgItem.imgNew.put(num139 + string.Empty, Image.createRGBImage(new int[1], 1, 1, true));
				}
				if (array11 != null)
				{
					if (mGraphics.zoomLevel > 1)
					{
						Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + num139, array11);
					}
					BgItemMn.blendcurrBg(num139, image);
				}
				break;
			}
			case 92:
			{
				if (GameCanvas.currentScreen == GameScr.instance)
				{
					GameCanvas.endDlg();
				}
				string text5 = msg.reader().readUTF();
				string str2 = msg.reader().readUTF();
				str2 = Res.changeString(str2);
				string empty = string.Empty;
				Char obj7 = null;
				sbyte b48 = 0;
				if (!text5.Equals(string.Empty))
				{
					obj7 = new Char();
					obj7.charID = msg.reader().readInt();
					obj7.head = msg.reader().readShort();
					obj7.headICON = msg.reader().readShort();
					obj7.body = msg.reader().readShort();
					obj7.bag = msg.reader().readShort();
					obj7.leg = msg.reader().readShort();
					b48 = msg.reader().readByte();
					obj7.cName = text5;
				}
				empty += str2;
				InfoDlg.hide();
				if (text5.Equals(string.Empty))
				{
					GameScr.info1.addInfo(empty, 0);
					break;
				}
				GameScr.info2.addInfoWithChar(empty, obj7, b48 == 0);
				if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
				{
					GameCanvas.panel.initLogMessage();
				}
				break;
			}
			case -26:
				ServerListScreen.testConnect = 2;
				GameCanvas.debug("SA2", 2);
				GameCanvas.startOKDlg(msg.reader().readUTF());
				InfoDlg.hide();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				if (GameCanvas.currentScreen == GameCanvas.loginScr)
				{
					GameCanvas.serverScreen.switchToMe();
				}
				break;
			case -25:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 94:
				GameCanvas.debug("SA3", 2);
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 47:
				GameCanvas.debug("SA4", 2);
				GameScr.gI().resetButton();
				break;
			case 81:
			{
				GameCanvas.debug("SXX4", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isDisable = msg.reader().readBool();
				break;
			}
			case 82:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isDontMove = msg.reader().readBool();
				break;
			}
			case 85:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isFire = msg.reader().readBool();
				break;
			}
			case 86:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isIce = msg.reader().readBool();
				if (!mob8.isIce)
				{
					ServerEffect.addServerEffect(77, mob8.x, mob8.y - 9, 1);
				}
				break;
			}
			case 87:
			{
				GameCanvas.debug("SXX5", 2);
				Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				mob8.isWind = msg.reader().readBool();
				break;
			}
			case 56:
			{
				GameCanvas.debug("SXX6", 2);
				obj = null;
				int num36 = msg.reader().readInt();
				if (num36 == Char.myCharz().charID)
				{
					bool flag3 = false;
					obj = Char.myCharz();
					obj.cHP = msg.reader().readLong();
					long num42 = msg.reader().readLong();
					Res.outz("dame hit = " + num42);
					if (num42 != 0)
					{
						obj.doInjure();
					}
					int num43 = 0;
					try
					{
						flag3 = msg.reader().readBoolean();
						sbyte b22 = msg.reader().readByte();
						if (b22 != -1)
						{
							Res.outz("hit eff= " + b22);
							EffecMn.addEff(new Effect(b22, obj.cx, obj.cy, 3, 1, -1));
						}
					}
					catch (Exception)
					{
					}
					num42 += num43;
					if (Char.myCharz().cTypePk != 4)
					{
						if (num42 == 0)
						{
							GameScr.startFlyText(mResources.miss, obj.cx, obj.cy - obj.ch, 0, -3, mFont.MISS_ME);
						}
						else
						{
							GameScr.startFlyText("-" + num42, obj.cx, obj.cy - obj.ch, 0, -3, flag3 ? mFont.FATAL : mFont.RED);
						}
					}
					break;
				}
				obj = GameScr.findCharInMap(num36);
				if (obj == null)
				{
					return;
				}
				obj.cHP = msg.reader().readLong();
				bool flag4 = false;
				long num44 = msg.reader().readLong();
				if (num44 != 0)
				{
					obj.doInjure();
				}
				int num45 = 0;
				try
				{
					flag4 = msg.reader().readBoolean();
					sbyte b23 = msg.reader().readByte();
					if (b23 != -1)
					{
						Res.outz("hit eff= " + b23);
						EffecMn.addEff(new Effect(b23, obj.cx, obj.cy, 3, 1, -1));
					}
				}
				catch (Exception)
				{
				}
				num44 += num45;
				if (obj.cTypePk != 4)
				{
					if (num44 == 0)
					{
						GameScr.startFlyText(mResources.miss, obj.cx, obj.cy - obj.ch, 0, -3, mFont.MISS);
					}
					else
					{
						GameScr.startFlyText("-" + num44, obj.cx, obj.cy - obj.ch, 0, -3, flag4 ? mFont.FATAL : mFont.ORANGE);
					}
				}
				break;
			}
			case 83:
			{
				GameCanvas.debug("SXX8", 2);
				int num36 = msg.reader().readInt();
				obj = ((num36 != Char.myCharz().charID) ? GameScr.findCharInMap(num36) : Char.myCharz());
				if (obj == null)
				{
					return;
				}
				Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				if (obj.mobMe != null)
				{
					obj.mobMe.attackOtherMob(mobToAttack);
				}
				break;
			}
			case 84:
			{
				int num36 = msg.reader().readInt();
				if (num36 == Char.myCharz().charID)
				{
					obj = Char.myCharz();
				}
				else
				{
					obj = GameScr.findCharInMap(num36);
					if (obj == null)
					{
						return;
					}
				}
				obj.cHP = obj.cHPFull;
				obj.cMP = obj.cMPFull;
				obj.cx = msg.reader().readShort();
				obj.cy = msg.reader().readShort();
				obj.liveFromDead();
				break;
			}
			case 46:
				GameCanvas.debug("SA5", 2);
				Cout.LogWarning("Controler RESET_POINT  " + Char.ischangingMap);
				Char.isLockKey = false;
				Char.myCharz().setResetPoint(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -29:
				messageNotLogin(msg);
				break;
			case -28:
				messageNotMap(msg);
				break;
			case -30:
				messageSubCommand(msg);
				break;
			case 62:
				GameCanvas.debug("SZ3", 2);
				obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.killCharId = Char.myCharz().charID;
					Char.myCharz().npcFocus = null;
					Char.myCharz().mobFocus = null;
					Char.myCharz().itemFocus = null;
					Char.myCharz().charFocus = obj;
					Char.isManualFocus = true;
					GameScr.info1.addInfo(obj.cName + mResources.CUU_SAT, 0);
				}
				break;
			case 63:
				GameCanvas.debug("SZ4", 2);
				Char.myCharz().killCharId = msg.reader().readInt();
				Char.myCharz().npcFocus = null;
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().charFocus = GameScr.findCharInMap(Char.myCharz().killCharId);
				Char.isManualFocus = true;
				break;
			case 64:
				GameCanvas.debug("SZ5", 2);
				obj = Char.myCharz();
				try
				{
					obj = GameScr.findCharInMap(msg.reader().readInt());
				}
				catch (Exception ex3)
				{
					Cout.println("Loi CLEAR_CUU_SAT " + ex3.ToString());
				}
				obj.killCharId = -9999;
				break;
			case 39:
				GameCanvas.debug("SA49", 2);
				GameScr.gI().typeTradeOrder = 2;
				if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
				{
					InfoDlg.showWait();
				}
				break;
			case 57:
			{
				GameCanvas.debug("SZ6", 2);
				MyVector myVector2 = new MyVector();
				myVector2.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88817, null));
				GameCanvas.menu.startAt(myVector2, 3);
				break;
			}
			case 58:
			{
				GameCanvas.debug("SZ7", 2);
				int num36 = msg.reader().readInt();
				Char obj11 = ((num36 != Char.myCharz().charID) ? GameScr.findCharInMap(num36) : Char.myCharz());
				obj11.moveFast = new short[3];
				obj11.moveFast[0] = 0;
				short num168 = msg.reader().readShort();
				short num169 = msg.reader().readShort();
				obj11.moveFast[1] = num168;
				obj11.moveFast[2] = num169;
				try
				{
					num36 = msg.reader().readInt();
					Char obj12 = ((num36 != Char.myCharz().charID) ? GameScr.findCharInMap(num36) : Char.myCharz());
					obj12.cx = num168;
					obj12.cy = num169;
				}
				catch (Exception ex26)
				{
					Cout.println("Loi MOVE_FAST " + ex26.ToString());
				}
				break;
			}
			case 88:
			{
				string info4 = msg.reader().readUTF();
				short num167 = msg.reader().readShort();
				GameCanvas.inputDlg.show(info4, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num167), TField.INPUT_TYPE_ANY);
				break;
			}
			case 27:
			{
				myVector = new MyVector();
				string text8 = msg.reader().readUTF();
				int num158 = msg.reader().readByte();
				for (int num159 = 0; num159 < num158; num159++)
				{
					string caption4 = msg.reader().readUTF();
					short num160 = msg.reader().readShort();
					myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num160));
				}
				GameCanvas.menu.startWithoutCloseButton(myVector, 3);
				break;
			}
			case 33:
			{
				GameCanvas.debug("SA51", 2);
				InfoDlg.hide();
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				myVector = new MyVector();
				try
				{
					while (true)
					{
						string caption3 = msg.reader().readUTF();
						myVector.addElement(new Command(caption3, GameCanvas.instance, 88822, null));
					}
				}
				catch (Exception ex24)
				{
					Cout.println("Loi OPEN_UI_MENU " + ex24.ToString());
				}
				if (Char.myCharz().npcFocus == null)
				{
					return;
				}
				for (int num155 = 0; num155 < Char.myCharz().npcFocus.template.menu.Length; num155++)
				{
					string[] array16 = Char.myCharz().npcFocus.template.menu[num155];
					myVector.addElement(new Command(array16[0], GameCanvas.instance, 88820, array16));
				}
				GameCanvas.menu.startAt(myVector, 3);
				break;
			}
			case 40:
			{
				GameCanvas.debug("SA52", 2);
				GameCanvas.taskTick = 150;
				short taskId = msg.reader().readShort();
				sbyte index3 = msg.reader().readByte();
				string str3 = msg.reader().readUTF();
				str3 = Res.changeString(str3);
				string str4 = msg.reader().readUTF();
				str4 = Res.changeString(str4);
				string[] array12 = new string[msg.reader().readByte()];
				string[] array13 = new string[array12.Length];
				GameScr.tasks = new int[array12.Length];
				GameScr.mapTasks = new int[array12.Length];
				short[] array14 = new short[array12.Length];
				short num145 = -1;
				for (int num146 = 0; num146 < array12.Length; num146++)
				{
					string str5 = msg.reader().readUTF();
					str5 = Res.changeString(str5);
					GameScr.tasks[num146] = msg.reader().readByte();
					GameScr.mapTasks[num146] = msg.reader().readShort();
					string str6 = msg.reader().readUTF();
					str6 = Res.changeString(str6);
					array14[num146] = -1;
					array12[num146] = str5;
					if (!str6.Equals(string.Empty))
					{
						array13[num146] = str6;
					}
				}
				try
				{
					num145 = msg.reader().readShort();
					Cout.println(" TASK_GET count:" + num145);
					for (int num147 = 0; num147 < array12.Length; num147++)
					{
						array14[num147] = msg.reader().readShort();
						Cout.println(num147 + " i TASK_GET   counts[i]:" + array14[num147]);
					}
				}
				catch (Exception ex22)
				{
					Cout.println("Loi TASK_GET " + ex22.ToString());
				}
				Char.myCharz().taskMaint = new Task(taskId, index3, str3, str4, array12, array14, num145, array13);
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
				Char.taskAction(true);
				break;
			}
			case 41:
				GameCanvas.debug("SA53", 2);
				GameCanvas.taskTick = 100;
				Res.outz("TASK NEXT");
				Char.myCharz().taskMaint.index++;
				Char.myCharz().taskMaint.count = 0;
				Npc.clearEffTask();
				Char.taskAction(true);
				break;
                case 50:
                    {
                        sbyte b59 = msg.reader().readByte();
                        Panel.vGameInfo.removeAllElements();
                        GameInfo gameInfoMod = new GameInfo();
                        gameInfoMod.id = 100;
                        gameInfoMod.main = "Lệnh Chat";
                        gameInfoMod.content = "k_x: Đổi khu\r\ncheat_x: Thay đổi tốc độ của game\r\ns_x: Điều chỉnh tốc độ chạy của nhân vật";
                        Panel.vGameInfo.addElement(gameInfoMod);
                        bool checkHasRead = (gameInfoMod.hasRead = Rms.loadRMSInt(gameInfoMod.id + string.Empty) != -1);

                        gameInfoMod = new GameInfo();
                        gameInfoMod.id = 101;
                        gameInfoMod.main = "Tính năng chính";
                        gameInfoMod.content = "Tích hợp sẵn\r\nHiển thị nhân vật trong game\r\nHiển thị thông báo boss\r\nTự cho đậu khi đệ kêu\r\nHiển thị thời gian hồi chiêu\r\nTốc độ game 2.3";
                        Panel.vGameInfo.addElement(gameInfoMod);
                        checkHasRead = (gameInfoMod.hasRead = Rms.loadRMSInt(gameInfoMod.id + string.Empty) != -1);

                        gameInfoMod = new GameInfo();
                        gameInfoMod.id = 102;
                        gameInfoMod.main = "Phím Tắt (bản pc)";
                        gameInfoMod.content =
                        "x: Menu chức năng\r\na: Auto đánh\r\ns: Auto gim boss\r\nd: Đóng băng skill\r\nz: Tự mở/tắt cờ đen\r\nc: Mở Capsunbay\r\nh: Auto phù NRD\r\nj: Load map Trái\r\nk: Load map Giữa\r\nl: Load map Phải\r\nm: Mở tab khu\r\nn: Menu nhặt đồ\r\nb: Danh sách bạn\r\ny: Chat thế giới\r\ne: Auto hồi sinh\r\nt: Bật/tắt TDLT\r\ni: Khóa ID\r\no: Auto NRO đen\r\nf: Bông tai/Hợp thể\r\ng: Gửi giao dịch\r\nv: Chức năng Boss\r\nSHIFT+C: Siêu tối ưu CPU\r\nSHIFT+A: Mặc nhanh set 1\r\nSHIFT+Z: Mặc nhanh set 2\r\nSHIFT+U: Dừng 1 số auto\r\nSHIFT+P: Thoát game\r\np: Kết nối QLTK";
                        Panel.vGameInfo.addElement(gameInfoMod);
                        checkHasRead = (gameInfoMod.hasRead = Rms.loadRMSInt(gameInfoMod.id + string.Empty) != -1);

                        gameInfoMod = new GameInfo();
                        gameInfoMod.id = 103;
                        gameInfoMod.main = "Một số chức năng nổi bật";
                        gameInfoMod.content =
                        "Auto dò boss (tdlt và không tdlt)\r\nHack hồi sinh không cần chọn mục tiêu\r\nAuto cờ đen khi có người\r\nAuto đổi mục tiêu mỗi 5s\r\nSpam trạng thái đệ tử\r\nSpam tách bông tai liên tục\r\nLeo top Whis tự động\r\nTự đấm bản thân làm nhiệm vụ\r\nHiển thị danh sách SKH\r\nShow TNM, SD, HP, KI\r\nAuto bán đồ rác (trừ TL, KH, pha lê)\r\nAuto cất SKH, TL, pha lê\r\nDịch chuyển bám boss\r\nAuto đánh boss\r\nAuto đánh người bật cờ (trừ boss cờ)\r\nAuto up đệ kok\r\nĐệ sủa là auto pem\r\nVứt item theo ID (vd: [451])\r\nThay logo, background cá nhân\r\nHiển thị vàng 1 hit/tổng vàng\r\nTrain đồ khỉ + set khác linh hoạt\r\nXMap toàn bộ\r\nVẽ rương đồ menu hành trang\r\nAuto nhiệm vụ Bò Mộng\r\nPaint rương đồ luôn hiện\r\nHiển thị %HP, %SD trên đồ\r\nProxy HTTP/SOCKS5\r\nTrain vượt địa hình\r\nAuto bỏ chọn mục tiêu dư\r\nAuto farm Boss Nappa\r\nSpam map Boss Trứng Mabu\r\nMua bình hút kill auto\r\nBản 246: Hiển thị crit và giảm sát thương";
                        Panel.vGameInfo.addElement(gameInfoMod);
                        checkHasRead = (gameInfoMod.hasRead = Rms.loadRMSInt(gameInfoMod.id + string.Empty) != -1);

                        gameInfoMod = new GameInfo();
                        gameInfoMod.id = 104;
                        gameInfoMod.main = "Phần mềm QLTK (bản pc)";
                        gameInfoMod.content =
                        "Click 2 lần: Mở game\r\nThay đổi kích thước tự về pixel nếu nhỏ\r\nLoad map nhiều tab\r\nChỉnh sửa nội dung auto chat\r\nSắp xếp tab nhanh\r\nXem chức năng khác trong QLTK\r\nTự thông báo khi có bản mới\r\nChưa có .NET 8 -> chạy Tải framework 8.0.bat";
                        Panel.vGameInfo.addElement(gameInfoMod);
                        checkHasRead = (gameInfoMod.hasRead = Rms.loadRMSInt(gameInfoMod.id + string.Empty) != -1);
                        for (int num132 = 0; num132 < b59; num132++)
                        {
                            GameInfo gameInfo = new GameInfo();
                            gameInfo.id = msg.reader().readShort();
                            gameInfo.main = msg.reader().readUTF();
                            gameInfo.content = msg.reader().readUTF();
                            Panel.vGameInfo.addElement(gameInfo);
                            bool flag10 = (gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1);
                        }
                        break;
                    }
                case 43:
				GameCanvas.taskTick = 50;
				GameCanvas.debug("SA55", 2);
				Char.myCharz().taskMaint.count = msg.reader().readShort();
				if (Char.myCharz().npcFocus != null)
				{
					Npc.clearEffTask();
				}
				try
				{
					short x_hint = msg.reader().readShort();
					short y_hint = msg.reader().readShort();
					Char.myCharz().x_hint = x_hint;
					Char.myCharz().y_hint = y_hint;
				}
				catch (Exception)
				{
				}
				break;
			case 90:
				GameCanvas.debug("SA577", 2);
				requestItemPlayer(msg);
				break;
			case 29:
				GameCanvas.debug("SA58", 2);
				GameScr.gI().openUIZone(msg);
				break;
			case -21:
			{
				GameCanvas.debug("SA60", 2);
				short itemMapID = msg.reader().readShort();
				for (int num136 = 0; num136 < GameScr.vItemMap.size(); num136++)
				{
					if (((ItemMap)GameScr.vItemMap.elementAt(num136)).itemMapID == itemMapID)
					{
						GameScr.vItemMap.removeElementAt(num136);
						break;
					}
				}
				break;
			}
			case -20:
			{
				GameCanvas.debug("SA61", 2);
				Char.myCharz().itemFocus = null;
				short itemMapID = msg.reader().readShort();
				for (int num134 = 0; num134 < GameScr.vItemMap.size(); num134++)
				{
					ItemMap itemMap4 = (ItemMap)GameScr.vItemMap.elementAt(num134);
					if (itemMap4.itemMapID != itemMapID)
					{
						continue;
					}
					itemMap4.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
					string text6 = msg.reader().readUTF();
					num = 0;
					try
					{
						num = msg.reader().readShort();
						if (itemMap4.template.type == 9)
						{
							num = msg.reader().readShort();
							Char.myCharz().xu += num;
							Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
						}
						else if (itemMap4.template.type == 10)
						{
							num = msg.reader().readShort();
							Char.myCharz().luong += num;
							Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
						}
						else if (itemMap4.template.type == 34)
						{
							num = msg.reader().readShort();
							Char.myCharz().luongKhoa += num;
							Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
						}
					}
					catch (Exception)
					{
					}
					if (text6.Equals(string.Empty))
					{
						if (itemMap4.template.type == 9)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.YELLOW);
							SoundMn.gI().getItem();
						}
						else if (itemMap4.template.type == 10)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.GREEN);
							SoundMn.gI().getItem();
						}
						else if (itemMap4.template.type == 34)
						{
							GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.RED);
							SoundMn.gI().getItem();
						}
						else
						{
							GameScr.info1.addInfo(mResources.you_receive + " " + ((num <= 0) ? string.Empty : (num + " ")) + itemMap4.template.name, 0);
							SoundMn.gI().getItem();
						}
						if (num > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 4683)
						{
							ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
							ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
						}
					}
					else if (text6.Length == 1)
					{
						Cout.LogError3("strInf.Length =1:  " + text6);
					}
					else
					{
						GameScr.info1.addInfo(text6, 0);
					}
					break;
				}
				break;
			}
			case -19:
			{
				GameCanvas.debug("SA62", 2);
				short itemMapID = msg.reader().readShort();
				obj = GameScr.findCharInMap(msg.reader().readInt());
				for (int num130 = 0; num130 < GameScr.vItemMap.size(); num130++)
				{
					ItemMap itemMap3 = (ItemMap)GameScr.vItemMap.elementAt(num130);
					if (itemMap3.itemMapID != itemMapID)
					{
						continue;
					}
					if (obj == null)
					{
						return;
					}
					itemMap3.setPoint(obj.cx, obj.cy - 10);
					if (itemMap3.x < obj.cx)
					{
						obj.cdir = -1;
					}
					else if (itemMap3.x > obj.cx)
					{
						obj.cdir = 1;
					}
					break;
				}
				break;
			}
			case -18:
			{
				GameCanvas.debug("SA63", 2);
				int num129 = msg.reader().readByte();
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num129].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
				Char.myCharz().arrItemBag[num129] = null;
				break;
			}
			case 68:
			{
				Res.outz("ADD ITEM TO MAP --------------------------------------");
				GameCanvas.debug("SA6333", 2);
				short itemMapID = msg.reader().readShort();
				short itemTemplateID = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num112 = msg.reader().readInt();
				short r = 0;
				if (num112 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap itemMap = new ItemMap(num112, itemMapID, itemTemplateID, x, y, r);
				bool flag8 = false;
				for (int num113 = 0; num113 < GameScr.vItemMap.size(); num113++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num113);
					if (itemMap2.itemMapID == itemMap.itemMapID)
					{
						flag8 = true;
						break;
					}
				}
				if (!flag8)
				{
					GameScr.vItemMap.addElement(itemMap);
				}
				break;
			}
			case 69:
				SoundMn.IsDelAcc = ((msg.reader().readByte() != 0) ? true : false);
				break;
			case -14:
				GameCanvas.debug("SA64", 2);
				obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					return;
				}
				GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), msg.reader().readShort(), obj.cx, obj.cy, msg.reader().readShort(), msg.reader().readShort()));
				break;
			case -22:
				GameCanvas.debug("SA65", 2);
				Char.isLockKey = true;
				Char.ischangingMap = true;
				GameScr.gI().timeStartMap = 0;
				GameScr.gI().timeLengthMap = 0;
				Char.myCharz().mobFocus = null;
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().focus.removeAllElements();
				Char.myCharz().testCharId = -9999;
				Char.myCharz().killCharId = -9999;
				GameCanvas.resetBg();
				GameScr.gI().resetButton();
				GameScr.gI().center = null;
				if (Effect.vEffData.size() > 15)
				{
					for (int num111 = 0; num111 < 5; num111++)
					{
						Effect.vEffData.removeElementAt(0);
					}
				}
				break;
			case -70:
			{
				Res.outz("BIG MESSAGE .......................................");
				GameCanvas.endDlg();
				int avatar2 = msg.reader().readShort();
				string chat3 = msg.reader().readUTF();
				Npc npc6 = new Npc(-1, 0, 0, 0, 0, 0);
				npc6.avatar = avatar2;
				ChatPopup.addBigMessage(chat3, 100000, npc6);
				sbyte b46 = msg.reader().readByte();
				if (b46 == 0)
				{
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
				}
				if (b46 == 1)
				{
					string p2 = msg.reader().readUTF();
					string caption2 = msg.reader().readUTF();
					ChatPopup.serverChatPopUp.cmdMsg1 = new Command(caption2, ChatPopup.serverChatPopUp, 1000, p2);
					ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 75;
					ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
					ChatPopup.serverChatPopUp.cmdMsg2 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
					ChatPopup.serverChatPopUp.cmdMsg2.x = GameCanvas.w / 2 + 11;
					ChatPopup.serverChatPopUp.cmdMsg2.y = GameCanvas.h - 35;
				}
				break;
			}
			case 38:
			{
				GameCanvas.debug("SA67", 2);
				InfoDlg.hide();
				int num76 = msg.reader().readShort();
				Res.outz("OPEN_UI_SAY ID= " + num76);
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				for (int num105 = 0; num105 < GameScr.vNpc.size(); num105++)
				{
					Npc npc4 = (Npc)GameScr.vNpc.elementAt(num105);
					Res.outz("npc id= " + npc4.template.npcTemplateId);
					if (npc4.template.npcTemplateId == num76)
					{
						ChatPopup.addChatPopupMultiLine(str, 100000, npc4);
						GameCanvas.panel.hideNow();
						return;
					}
				}
				Npc npc5 = new Npc(num76, 0, 0, 0, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				if (npc5.template.npcTemplateId == 5)
				{
					npc5.charID = 5;
				}
				try
				{
					npc5.avatar = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				ChatPopup.addChatPopupMultiLine(str, 100000, npc5);
				GameCanvas.panel.hideNow();
				break;
			}
			case 32:
			{
				GameCanvas.debug("SA68", 2);
				int num76 = msg.reader().readShort();
				for (int num77 = 0; num77 < GameScr.vNpc.size(); num77++)
				{
					Npc npc2 = (Npc)GameScr.vNpc.elementAt(num77);
					if (npc2.template.npcTemplateId == num76 && npc2.Equals(Char.myCharz().npcFocus))
					{
						string chat = msg.reader().readUTF();
						string[] array6 = new string[msg.reader().readByte()];
						for (int num78 = 0; num78 < array6.Length; num78++)
						{
							array6[num78] = msg.reader().readUTF();
						}
						GameScr.gI().createMenu(array6, npc2);
						ChatPopup.addChatPopup(chat, 100000, npc2);
						return;
					}
				}
				Npc npc3 = new Npc(num76, 0, -100, 100, num76, GameScr.info1.charId[Char.myCharz().cgender][2]);
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				string chat2 = msg.reader().readUTF();
				string[] array7 = new string[msg.reader().readByte()];
				for (int num79 = 0; num79 < array7.Length; num79++)
				{
					array7[num79] = msg.reader().readUTF();
				}
				try
				{
					short avatar = msg.reader().readShort();
					npc3.avatar = avatar;
				}
				catch (Exception)
				{
				}
				Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
				GameScr.gI().createMenu(array7, npc3);
				ChatPopup.addChatPopup(chat2, 100000, npc3);
				break;
			}
			case 7:
			{
				sbyte type = msg.reader().readByte();
				short id2 = msg.reader().readShort();
				string info2 = msg.reader().readUTF();
				GameCanvas.panel.saleRequest(type, info2, id2);
				break;
			}
			case 6:
				GameCanvas.debug("SA70", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				GameCanvas.endDlg();
				break;
			case -24:
				Res.outz("***************MAP_INFO**************");
				GameScr.isPickNgocRong = false;
				Char.isLoadingMap = true;
				Cout.println("GET MAP INFO");
				GameScr.gI().magicTree = null;
				GameCanvas.isLoading = true;
				GameCanvas.debug("SA75", 2);
				GameScr.resetAllvector();
				GameCanvas.endDlg();
				TileMap.vGo.removeAllElements();
				PopUp.vPopups.removeAllElements();
				mSystem.gcc();
				TileMap.mapID = msg.reader().readUnsignedByte();
				TileMap.planetID = msg.reader().readByte();
				TileMap.tileID = msg.reader().readByte();
				TileMap.bgID = msg.reader().readByte();
				GameScr.isPaint_CT = TileMap.mapID != 170;
				Cout.println("load planet from server: " + TileMap.planetID + "bgType= " + TileMap.bgType + ".............................");
				TileMap.typeMap = msg.reader().readByte();
				TileMap.mapName = msg.reader().readUTF();
				TileMap.zoneID = msg.reader().readByte();
				GameCanvas.debug("SA75x1", 2);
				try
				{
					TileMap.loadMapFromResource(TileMap.mapID);
				}
				catch (Exception)
				{
					Service.gI().requestMaptemplate(TileMap.mapID);
					messWait = msg;
					break;
				}
				loadInfoMap(msg);
				try
				{
					sbyte b32 = msg.reader().readByte();
					TileMap.isMapDouble = ((b32 != 0) ? true : false);
				}
				catch (Exception)
				{
				}
				GameScr.cmx = GameScr.cmtoX;
				GameScr.cmy = GameScr.cmtoY;
				GameCanvas.isRequestMapID = 2;
				GameCanvas.waitingTimeChangeMap = mSystem.currentTimeMillis() + 1000;
				break;
			case -31:
			{
				TileMap.vItemBg.removeAllElements();
				short num61 = msg.reader().readShort();
				Res.err("[ITEM_BACKGROUND] nItem= " + num61);
				for (int num62 = 0; num62 < num61; num62++)
				{
					BgItem bgItem = new BgItem();
					bgItem.id = num62;
					bgItem.idImage = msg.reader().readShort();
					bgItem.layer = msg.reader().readByte();
					bgItem.dx = msg.reader().readShort();
					bgItem.dy = msg.reader().readShort();
					sbyte b28 = msg.reader().readByte();
					bgItem.tileX = new int[b28];
					bgItem.tileY = new int[b28];
					for (int num63 = 0; num63 < b28; num63++)
					{
						bgItem.tileX[num62] = msg.reader().readByte();
						bgItem.tileY[num62] = msg.reader().readByte();
					}
					TileMap.vItemBg.addElement(bgItem);
				}
				break;
			}
			case -4:
			{
				GameCanvas.debug("SA76", 2);
				obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					return;
				}
				GameCanvas.debug("SA76v1", 2);
				if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
				{
					obj.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
				}
				else
				{
					obj.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
				}
				GameCanvas.debug("SA76v2", 2);
				obj.attMobs = new Mob[msg.reader().readByte()];
				for (int num38 = 0; num38 < obj.attMobs.Length; num38++)
				{
					Mob mob7 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
					obj.attMobs[num38] = mob7;
					if (num38 == 0)
					{
						if (obj.cx <= mob7.x)
						{
							obj.cdir = 1;
						}
						else
						{
							obj.cdir = -1;
						}
					}
				}
				GameCanvas.debug("SA76v3", 2);
				obj.charFocus = null;
				obj.mobFocus = obj.attMobs[0];
				Char[] array = new Char[10];
				num = 0;
				try
				{
					for (num = 0; num < array.Length; num++)
					{
						int num36 = msg.reader().readInt();
						Char obj4 = (array[num] = ((num36 != Char.myCharz().charID) ? GameScr.findCharInMap(num36) : Char.myCharz()));
						if (num == 0)
						{
							if (obj.cx <= obj4.cx)
							{
								obj.cdir = 1;
							}
							else
							{
								obj.cdir = -1;
							}
						}
					}
				}
				catch (Exception ex6)
				{
					Cout.println("Loi PLAYER_ATTACK_N_P " + ex6.ToString());
				}
				GameCanvas.debug("SA76v4", 2);
				if (num > 0)
				{
					obj.attChars = new Char[num];
					for (num = 0; num < obj.attChars.Length; num++)
					{
						obj.attChars[num] = array[num];
					}
					obj.charFocus = obj.attChars[0];
					obj.mobFocus = null;
				}
				GameCanvas.debug("SA76v5", 2);
				break;
			}
			case 54:
			{
				obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					return;
				}
				int num27 = msg.reader().readUnsignedByte();
				if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
				{
					obj.setSkillPaint(GameScr.sks[num27], 0);
				}
				else
				{
					obj.setSkillPaint(GameScr.sks[num27], 1);
				}
				Mob[] array3 = new Mob[10];
				num = 0;
				try
				{
					for (num = 0; num < array3.Length; num++)
					{
						Mob mob6 = (array3[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
						if (num == 0)
						{
							if (obj.cx <= mob6.x)
							{
								obj.cdir = 1;
							}
							else
							{
								obj.cdir = -1;
							}
						}
					}
				}
				catch (Exception)
				{
				}
				if (num > 0)
				{
					obj.attMobs = new Mob[num];
					for (num = 0; num < obj.attMobs.Length; num++)
					{
						obj.attMobs[num] = array3[num];
					}
					obj.charFocus = null;
					obj.mobFocus = obj.attMobs[0];
				}
				break;
			}
			case -60:
			{
				GameCanvas.debug("SA7666", 2);
				int num2 = msg.reader().readInt();
				int num3 = -1;
				if (num2 != Char.myCharz().charID)
				{
					Char obj2 = GameScr.findCharInMap(num2);
					if (obj2 == null)
					{
						return;
					}
					if (obj2.currentMovePoint != null)
					{
						obj2.createShadow(obj2.cx, obj2.cy, 10);
						obj2.cx = obj2.currentMovePoint.xEnd;
						obj2.cy = obj2.currentMovePoint.yEnd;
					}
					int num4 = msg.reader().readUnsignedByte();
					if ((TileMap.tileTypeAtPixel(obj2.cx, obj2.cy) & 2) == 2)
					{
						obj2.setSkillPaint(GameScr.sks[num4], 0);
					}
					else
					{
						obj2.setSkillPaint(GameScr.sks[num4], 1);
					}
					sbyte b = msg.reader().readByte();
					Char[] array = new Char[b];
					for (num = 0; num < array.Length; num++)
					{
						num3 = msg.reader().readInt();
						Char obj3;
						if (num3 == Char.myCharz().charID)
						{
							obj3 = Char.myCharz();
							if (!GameScr.isChangeZone && GameScr.isAutoPlay && GameScr.canAutoPlay)
							{
								Service.gI().requestChangeZone(-1, -1);
								GameScr.isChangeZone = true;
							}
						}
						else
						{
							obj3 = GameScr.findCharInMap(num3);
						}
						array[num] = obj3;
						if (num == 0)
						{
							if (obj2.cx <= obj3.cx)
							{
								obj2.cdir = 1;
							}
							else
							{
								obj2.cdir = -1;
							}
						}
					}
					if (num > 0)
					{
						obj2.attChars = new Char[num];
						for (num = 0; num < obj2.attChars.Length; num++)
						{
							obj2.attChars[num] = array[num];
						}
						obj2.mobFocus = null;
						obj2.charFocus = obj2.attChars[0];
					}
				}
				else
				{
					sbyte b2 = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					num3 = msg.reader().readInt();
				}
				try
				{
					sbyte b4 = msg.reader().readByte();
					Res.outz("isRead continue = " + b4);
					if (b4 != 1)
					{
						break;
					}
					sbyte b5 = msg.reader().readByte();
					Res.outz("type skill = " + b5);
					if (num3 == Char.myCharz().charID)
					{
						bool flag = false;
						obj = Char.myCharz();
						long num5 = msg.reader().readLong();
						Res.outz("dame hit = " + num5);
						obj.isDie = msg.reader().readBoolean();
						if (obj.isDie)
						{
							Char.isLockKey = true;
						}
						Res.outz("isDie=" + obj.isDie + "---------------------------------------");
						int num6 = 0;
						flag = (obj.isCrit = msg.reader().readBoolean());
						obj.isMob = false;
						num5 = (obj.damHP = num5 + num6);
						if (b5 == 0)
						{
							obj.doInjure(num5, 0L, flag, false);
						}
					}
					else
					{
						obj = GameScr.findCharInMap(num3);
						if (obj == null)
						{
							return;
						}
						bool flag2 = false;
						long num7 = msg.reader().readLong();
						Res.outz("dame hit= " + num7);
						obj.isDie = msg.reader().readBoolean();
						Res.outz("isDie=" + obj.isDie + "---------------------------------------");
						int num8 = 0;
						flag2 = (obj.isCrit = msg.reader().readBoolean());
						obj.isMob = false;
						num7 = (obj.damHP = num7 + num8);
						if (b5 == 0)
						{
							obj.doInjure(num7, 0L, flag2, false);
						}
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			}
			switch (msg.command)
			{
			case -2:
			{
				GameCanvas.debug("SA77", 22);
				int num192 = msg.reader().readInt();
				Char.myCharz().yen += num192;
				GameScr.startFlyText((num192 <= 0) ? (string.Empty + num192) : ("+" + num192), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 95:
			{
				GameCanvas.debug("SA77", 22);
				int num179 = msg.reader().readInt();
				Char.myCharz().xu += num179;
				Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
				GameScr.startFlyText((num179 <= 0) ? (string.Empty + num179) : ("+" + num179), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case 96:
				GameCanvas.debug("SA77a", 22);
				Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
				break;
			case 97:
			{
				sbyte b76 = msg.reader().readByte();
				for (int num185 = 0; num185 < Char.myCharz().taskOrders.size(); num185++)
				{
					TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num185);
					if (taskOrder.taskId == b76)
					{
						taskOrder.count = msg.reader().readShort();
						break;
					}
				}
				break;
			}
			case -1:
			{
				GameCanvas.debug("SA77", 222);
				int num191 = msg.reader().readInt();
				Char.myCharz().xu += num191;
				Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
				Char.myCharz().yen -= num191;
				GameScr.startFlyText("+" + num191, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
				break;
			}
			case -3:
			{
				GameCanvas.debug("SA78", 2);
				sbyte b72 = msg.reader().readByte();
				int num176 = msg.reader().readInt();
				if (b72 == 0)
				{
					Char.myCharz().cPower += num176;
				}
				if (b72 == 1)
				{
					Char.myCharz().cTiemNang += num176;
				}
				if (b72 == 2)
				{
					Char.myCharz().cPower += num176;
					Char.myCharz().cTiemNang += num176;
				}
				Char.myCharz().applyCharLevelPercent();
				if (Char.myCharz().cTypePk != 3)
				{
					GameScr.startFlyText(((num176 <= 0) ? string.Empty : "+") + num176, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
					if (num176 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
					{
						ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
						ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
					}
				}
				break;
			}
			case -73:
			{
				sbyte b78 = msg.reader().readByte();
				for (int num190 = 0; num190 < GameScr.vNpc.size(); num190++)
				{
					Npc npc7 = (Npc)GameScr.vNpc.elementAt(num190);
					if (npc7.template.npcTemplateId == b78)
					{
						sbyte b79 = msg.reader().readByte();
						if (b79 == 0)
						{
							npc7.isHide = true;
						}
						else
						{
							npc7.isHide = false;
						}
						break;
					}
				}
				break;
			}
			case -5:
			{
				GameCanvas.debug("SA79", 2);
				int charID = msg.reader().readInt();
				int num181 = msg.reader().readInt();
				Char obj16;
				if (num181 != -100)
				{
					obj16 = new Char();
					obj16.charID = charID;
					obj16.clanID = num181;
				}
				else
				{
					obj16 = new Mabu();
					obj16.charID = charID;
					obj16.clanID = num181;
				}
				if (obj16.clanID == -2)
				{
					obj16.isCopy = true;
				}
				if (readCharInfo(obj16, msg))
				{
					sbyte b74 = msg.reader().readByte();
					if (obj16.cy <= 10 && b74 != 0 && b74 != 2)
					{
						Res.outz("nhân vật bay trên trời xuống x= " + obj16.cx + " y= " + obj16.cy);
						Teleport teleport2 = new Teleport(obj16.cx, obj16.cy, obj16.head, obj16.cdir, 1, false, (b74 != 1) ? b74 : obj16.cgender);
						teleport2.id = obj16.charID;
						obj16.isTeleport = true;
						Teleport.addTeleport(teleport2);
					}
					if (b74 == 2)
					{
						obj16.show();
					}
					for (int num182 = 0; num182 < GameScr.vMob.size(); num182++)
					{
						Mob mob10 = (Mob)GameScr.vMob.elementAt(num182);
						if (mob10 != null && mob10.isMobMe && mob10.mobId == obj16.charID)
						{
							Res.outz("co 1 con quai");
							obj16.mobMe = mob10;
							obj16.mobMe.x = obj16.cx;
							obj16.mobMe.y = obj16.cy - 40;
							break;
						}
					}
					GameScr.RemovefindCharInMap(charID);
                    GameScr.vCharInMap.addElement(obj16);
					obj16.isMonkey = msg.reader().readByte();
					short num183 = msg.reader().readShort();
					Res.outz("mount id= " + num183 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					if (num183 != -1)
					{
						obj16.isHaveMount = true;
						switch (num183)
						{
						case 346:
						case 347:
						case 348:
							obj16.isMountVip = false;
							break;
						case 349:
						case 350:
						case 351:
							obj16.isMountVip = true;
							break;
						case 396:
							obj16.isEventMount = true;
							break;
						case 532:
							obj16.isSpeacialMount = true;
							break;
						default:
							if (num183 >= Char.ID_NEW_MOUNT)
							{
								obj16.idMount = num183;
							}
							break;
						}
					}
					else
					{
						obj16.isHaveMount = false;
					}
				}
				sbyte b75 = msg.reader().readByte();
				Res.outz("addplayer:   " + b75);
				obj16.cFlag = b75;
				obj16.isNhapThe = msg.reader().readByte() == 1;
				try
				{
					obj16.idAuraEff = msg.reader().readShort();
					obj16.idEff_Set_Item = msg.reader().readSByte();
					obj16.idHat = msg.reader().readShort();
					if (obj16.bag >= 201 && obj16.bag < 255)
					{
						Effect effect3 = new Effect(obj16.bag, obj16, 2, -1, 10, 1);
						effect3.typeEff = 5;
						obj16.addEffChar(effect3);
					}
					else
					{
						for (int num184 = 0; num184 < 54; num184++)
						{
							obj16.removeEffChar(0, 201 + num184);
						}
					}
				}
				catch (Exception ex38)
				{
					Res.outz("cmd: -5 err: " + ex38.StackTrace);
				}
				GameScr.gI().getFlagImage(obj16.charID, obj16.cFlag);
				break;
			}
			case -7:
			{
				GameCanvas.debug("SA80", 2);
				int num174 = msg.reader().readInt();
				for (int num177 = 0; num177 < GameScr.vCharInMap.size(); num177++)
				{
					Char obj15 = null;
					try
					{
						obj15 = (Char)GameScr.vCharInMap.elementAt(num177);
					}
					catch (Exception)
					{
						continue;
					}
					if (obj15 == null || obj15.charID != num174)
					{
						continue;
					}
					GameCanvas.debug("SA8x2y" + num177, 2);
					obj15.moveTo(msg.reader().readShort(), msg.reader().readShort(), 0);
					obj15.lastUpdateTime = mSystem.currentTimeMillis();
					break;
				}
				GameCanvas.debug("SA80x3", 2);
				break;
			}
			case -6:
			{
				GameCanvas.debug("SA81", 2);
				int num174 = msg.reader().readInt();
				for (int num175 = 0; num175 < GameScr.vCharInMap.size(); num175++)
				{
					Char obj14 = (Char)GameScr.vCharInMap.elementAt(num175);
					if (obj14 != null && obj14.charID == num174)
					{
						if (!obj14.isInvisiblez && !obj14.isUsePlane)
						{
							ServerEffect.addServerEffect(60, obj14.cx, obj14.cy, 1);
						}
						if (!obj14.isUsePlane)
						{
							GameScr.vCharInMap.removeElementAt(num175);
						}
						return;
					}
				}
				break;
			}
			case -13:
			{
				GameCanvas.debug("SA82", 2);
				int num186 = msg.reader().readUnsignedByte();
				if (num186 > GameScr.vMob.size() - 1 || num186 < 0)
				{
					return;
				}
				Mob mob9 = (Mob)GameScr.vMob.elementAt(num186);
				mob9.sys = msg.reader().readByte();
				mob9.levelBoss = msg.reader().readByte();
				if (mob9.levelBoss != 0)
				{
					mob9.typeSuperEff = Res.random(0, 3);
				}
				mob9.x = mob9.xFirst;
				mob9.y = mob9.yFirst;
				mob9.status = 5;
				mob9.injureThenDie = false;
				mob9.hp = msg.reader().readLong();
				mob9.maxHp = mob9.hp;
				mob9.updateHp_bar();
				ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
				break;
			}
			case -75:
			{
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				if (mob9 != null)
				{
					mob9.levelBoss = msg.reader().readByte();
					if (mob9.levelBoss > 0)
					{
						mob9.typeSuperEff = Res.random(0, 3);
					}
				}
				break;
			}
			case -9:
			{
				GameCanvas.debug("SA83", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA83v1", 2);
				if (mob9 != null)
				{
					mob9.hp = msg.reader().readLong();
					mob9.updateHp_bar();
					long num178 = msg.reader().readLong();
					if (num178 == 1)
					{
						return;
					}
					if (num178 > 1)
					{
						mob9.setInjure();
					}
					bool flag11 = false;
					try
					{
						flag11 = msg.reader().readBoolean();
					}
					catch (Exception)
					{
					}
					sbyte b73 = msg.reader().readByte();
					if (b73 != -1)
					{
						EffecMn.addEff(new Effect(b73, mob9.x, mob9.getY(), 3, 1, -1));
					}
					GameCanvas.debug("SA83v2", 2);
					if (flag11)
					{
						GameScr.startFlyText("-" + num178, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.FATAL);
					}
					else if (num178 == 0)
					{
						mob9.x = mob9.xFirst;
						mob9.y = mob9.yFirst;
						GameScr.startFlyText(mResources.miss, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.MISS);
					}
					else if (num178 > 1)
					{
						GameScr.startFlyText("-" + num178, mob9.x, mob9.getY() - mob9.getH(), 0, -2, mFont.ORANGE);
					}
				}
				GameCanvas.debug("SA83v3", 2);
				break;
			}
			case 45:
			{
				GameCanvas.debug("SA84", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception ex29)
				{
					Cout.println("Loi tai NPC_MISS  " + ex29.ToString());
				}
				if (mob9 != null)
				{
					mob9.hp = msg.reader().readLong();
					mob9.updateHp_bar();
					GameScr.startFlyText(mResources.miss, mob9.x, mob9.y - mob9.h, 0, -2, mFont.MISS);
				}
				break;
			}
			case -12:
			{
				Res.outz("SERVER SEND MOB DIE");
				GameCanvas.debug("SA85", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
					Cout.println("LOi tai NPC_DIE cmd " + msg.command);
				}
				if (mob9 == null || mob9.status == 0 || mob9.status == 0)
				{
					break;
				}
				mob9.startDie();
				try
				{
					long num187 = msg.reader().readLong();
					if (msg.reader().readBool())
					{
						GameScr.startFlyText("-" + num187, mob9.x, mob9.y - mob9.h, 0, -2, mFont.FATAL);
					}
					else
					{
						GameScr.startFlyText("-" + num187, mob9.x, mob9.y - mob9.h, 0, -2, mFont.ORANGE);
					}
					sbyte b77 = msg.reader().readByte();
					for (int num188 = 0; num188 < b77; num188++)
					{
						ItemMap itemMap6 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
						int num189 = (itemMap6.playerId = msg.reader().readInt());
						Res.outz("playerid= " + num189 + " my id= " + Char.myCharz().charID);
						GameScr.vItemMap.addElement(itemMap6);
						if (Res.abs(itemMap6.y - Char.myCharz().cy) < 24 && Res.abs(itemMap6.x - Char.myCharz().cx) < 24)
						{
							Char.myCharz().charFocus = null;
						}
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			case 74:
			{
				GameCanvas.debug("SA85", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
					Cout.println("Loi tai NPC CHANGE " + msg.command);
				}
				if (mob9 != null && mob9.status != 0 && mob9.status != 0)
				{
					mob9.status = 0;
					ServerEffect.addServerEffect(60, mob9.x, mob9.y, 1);
					ItemMap itemMap5 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob9.x, mob9.y, msg.reader().readShort(), msg.reader().readShort());
					GameScr.vItemMap.addElement(itemMap5);
					if (Res.abs(itemMap5.y - Char.myCharz().cy) < 24 && Res.abs(itemMap5.x - Char.myCharz().cx) < 24)
					{
						Char.myCharz().charFocus = null;
					}
				}
				break;
			}
			case -11:
			{
				GameCanvas.debug("SA86", 2);
				Mob mob9 = null;
				try
				{
					int index4 = msg.reader().readUnsignedByte();
					mob9 = (Mob)GameScr.vMob.elementAt(index4);
				}
				catch (Exception ex27)
				{
					Res.outz("Loi tai NPC_ATTACK_ME " + msg.command + " err= " + ex27.StackTrace);
				}
				if (mob9 != null)
				{
					Char.myCharz().isDie = false;
					Char.isLockKey = false;
					long num171 = msg.reader().readLong();
					long num172;
					try
					{
						num172 = msg.reader().readLong();
					}
					catch (Exception)
					{
						num172 = 0L;
					}
					if (mob9.isBusyAttackSomeOne)
					{
						Char.myCharz().doInjure(num171, num172, false, true);
						break;
					}
					mob9.dame = num171;
					mob9.dameMp = num172;
					mob9.setAttack(Char.myCharz());
				}
				break;
			}
			case -10:
			{
				GameCanvas.debug("SA87", 2);
				Mob mob9 = null;
				try
				{
					mob9 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
				}
				catch (Exception)
				{
				}
				GameCanvas.debug("SA87x1", 2);
				if (mob9 != null)
				{
					GameCanvas.debug("SA87x2", 2);
					obj = GameScr.findCharInMap(msg.reader().readInt());
					if (obj == null)
					{
						return;
					}
					GameCanvas.debug("SA87x3", 2);
					long num180 = msg.reader().readLong();
					mob9.dame = obj.cHP - num180;
					obj.cHPNew = num180;
					GameCanvas.debug("SA87x4", 2);
					try
					{
						obj.cMP = msg.reader().readLong();
					}
					catch (Exception)
					{
					}
					GameCanvas.debug("SA87x5", 2);
					if (mob9.isBusyAttackSomeOne)
					{
						obj.doInjure(mob9.dame, 0L, false, true);
					}
					else
					{
						mob9.setAttack(obj);
					}
					GameCanvas.debug("SA87x6", 2);
				}
				break;
			}
			case -17:
				GameCanvas.debug("SA88", 2);
				Char.myCharz().meDead = true;
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().startDie(msg.reader().readShort(), msg.reader().readShort());
				try
				{
					Char.myCharz().cPower = msg.reader().readLong();
					Char.myCharz().applyCharLevelPercent();
				}
				catch (Exception)
				{
					Cout.println("Loi tai ME_DIE " + msg.command);
				}
				Char.myCharz().countKill = 0;
				break;
			case 66:
				Res.outz("ME DIE XP DOWN NOT IMPLEMENT YET!!!!!!!!!!!!!!!!!!!!!!!!!!");
				break;
			case -8:
				GameCanvas.debug("SA89", 2);
				obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					return;
				}
				obj.cPk = msg.reader().readByte();
				obj.waitToDie(msg.reader().readShort(), msg.reader().readShort());
				break;
			case -16:
				GameCanvas.debug("SA90", 2);
				if (Char.myCharz().wdx != 0 || Char.myCharz().wdy != 0)
				{
					Char.myCharz().cx = Char.myCharz().wdx;
					Char.myCharz().cy = Char.myCharz().wdy;
					Char.myCharz().wdx = (Char.myCharz().wdy = 0);
				}
				Char.myCharz().liveFromDead();
				Char.myCharz().isLockMove = false;
				Char.myCharz().meDead = false;
				break;
			case 44:
			{
				GameCanvas.debug("SA91", 2);
				int num173 = msg.reader().readInt();
				string text9 = msg.reader().readUTF();
				Res.outz("user id= " + num173 + " text= " + text9);
				obj = ((Char.myCharz().charID != num173) ? GameScr.findCharInMap(num173) : Char.myCharz());
				if (obj == null)
				{
					return;
				}
				obj.addInfo(text9);
				break;
			}
			case 18:
			{
				sbyte b71 = msg.reader().readByte();
				for (int num170 = 0; num170 < b71; num170++)
				{
					int charId = msg.reader().readInt();
					int cx = msg.reader().readShort();
					int cy = msg.reader().readShort();
					long cHPShow = msg.reader().readLong();
					Char obj13 = GameScr.findCharInMap(charId);
					if (obj13 != null)
					{
						obj13.cx = cx;
						obj13.cy = cy;
						obj13.cHP = (obj13.cHPShow = cHPShow);
						obj13.lastUpdateTime = mSystem.currentTimeMillis();
					}
				}
				break;
			}
			case 19:
				Char.myCharz().countKill = msg.reader().readUnsignedShort();
				Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
				break;
			}
			GameCanvas.debug("SA92", 2);
		}
		catch (Exception ex41)
		{
			Res.err("[Controller] [error] " + ex41.StackTrace + " msg: " + ex41.Message + " cause " + ex41.Data);
		}
		finally
		{
			if (msg != null)
			{
				msg.cleanup();
			}
		}
	}

	private void readLogin(Message msg)
	{
		sbyte b = msg.reader().readByte();
		ChooseCharScr.playerData = new PlayerData[b];
		Res.outz("[LEN] sl nguoi choi " + b);
		for (int i = 0; i < b; i++)
		{
			int playerID = msg.reader().readInt();
			string name = msg.reader().readUTF();
			short head = msg.reader().readShort();
			short body = msg.reader().readShort();
			short leg = msg.reader().readShort();
			long ppoint = msg.reader().readLong();
			ChooseCharScr.playerData[i] = new PlayerData(playerID, name, head, body, leg, ppoint);
		}
		GameCanvas.chooseCharScr.switchToMe();
		GameCanvas.chooseCharScr.updateChooseCharacter((byte)b);
	}

	private void createSkill(myReader d)
	{
		GameScr.vcSkill = d.readByte();
		GameScr.gI().sOptionTemplates = new SkillOptionTemplate[d.readByte()];
		for (int i = 0; i < GameScr.gI().sOptionTemplates.Length; i++)
		{
			GameScr.gI().sOptionTemplates[i] = new SkillOptionTemplate();
			GameScr.gI().sOptionTemplates[i].id = i;
			GameScr.gI().sOptionTemplates[i].name = d.readUTF();
		}
		GameScr.nClasss = new NClass[d.readByte()];
		for (int j = 0; j < GameScr.nClasss.Length; j++)
		{
			GameScr.nClasss[j] = new NClass();
			GameScr.nClasss[j].classId = j;
			GameScr.nClasss[j].name = d.readUTF();
			GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
			for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
			{
				GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
				GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
				GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].manaUseType = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
				GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
				GameScr.nClasss[j].skillTemplates[k].damInfo = d.readUTF();
				int lineWidth = 130;
				if (GameCanvas.w == 128 || GameCanvas.h <= 208)
				{
					lineWidth = 100;
				}
				GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_green2.splitFontArray(d.readUTF(), lineWidth);
				GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
				for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
				{
					GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
					GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
					GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].powRequire = d.readLong();
					GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
					GameScr.nClasss[j].skillTemplates[k].skills[l].damage = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].price = d.readShort();
					GameScr.nClasss[j].skillTemplates[k].skills[l].moreInfo = d.readUTF();
					Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
				}
			}
		}
	}

	private void createMap(myReader d)
	{
		GameScr.vcMap = d.readByte();
		TileMap.mapNames = new string[d.readShort()];
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			TileMap.mapNames[i] = d.readUTF();
		}
		Npc.arrNpcTemplate = new NpcTemplate[d.readByte()];
		for (sbyte b = 0; b < Npc.arrNpcTemplate.Length; b++)
		{
			Npc.arrNpcTemplate[b] = new NpcTemplate();
			Npc.arrNpcTemplate[b].npcTemplateId = b;
			Npc.arrNpcTemplate[b].name = d.readUTF();
			Npc.arrNpcTemplate[b].headId = d.readShort();
			Npc.arrNpcTemplate[b].bodyId = d.readShort();
			Npc.arrNpcTemplate[b].legId = d.readShort();
			Npc.arrNpcTemplate[b].menu = new string[d.readByte()][];
			for (int j = 0; j < Npc.arrNpcTemplate[b].menu.Length; j++)
			{
				Npc.arrNpcTemplate[b].menu[j] = new string[d.readByte()];
				for (int k = 0; k < Npc.arrNpcTemplate[b].menu[j].Length; k++)
				{
					Npc.arrNpcTemplate[b].menu[j][k] = d.readUTF();
				}
			}
		}
		Mob.arrMobTemplate = new MobTemplate[d.readShort()];
		for (int l = 0; l < Mob.arrMobTemplate.Length; l++)
		{
			Mob.arrMobTemplate[l] = new MobTemplate();
			Mob.arrMobTemplate[l].mobTemplateId = l;
			Mob.arrMobTemplate[l].type = d.readByte();
			Mob.arrMobTemplate[l].name = d.readUTF();
			Mob.arrMobTemplate[l].hp = d.readLong();
			Mob.arrMobTemplate[l].rangeMove = d.readByte();
			Mob.arrMobTemplate[l].speed = d.readByte();
			Mob.arrMobTemplate[l].dartType = d.readByte();
		}
	}

	private void createData(myReader d, bool isSaveRMS)
	{
		GameScr.vcData = d.readByte();
		if (isSaveRMS)
		{
			Rms.saveRMS("NR_dart", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_arrow", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_effect", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_image", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_part", NinjaUtil.readByteArray(d));
			Rms.saveRMS("NR_skill", NinjaUtil.readByteArray(d));
			Rms.DeleteStorage("NRdata");
		}
	}

	private Image createImage(sbyte[] arr)
	{
		try
		{
			return Image.createImage(arr, 0, arr.Length);
		}
		catch (Exception)
		{
		}
		return null;
	}

	public int[] arrayByte2Int(sbyte[] b)
	{
		int[] array = new int[b.Length];
		for (int i = 0; i < b.Length; i++)
		{
			int num = b[i];
			if (num < 0)
			{
				num += 256;
			}
			array[i] = num;
		}
		return array;
	}

	public void readClanMsg(Message msg, int index)
	{
		try
		{
			ClanMessage clanMessage = new ClanMessage();
			sbyte b = msg.reader().readByte();
			clanMessage.type = b;
			clanMessage.id = msg.reader().readInt();
			clanMessage.playerId = msg.reader().readInt();
			clanMessage.playerName = msg.reader().readUTF();
			clanMessage.role = msg.reader().readByte();
			clanMessage.time = msg.reader().readInt() + 1000000000;
			bool flag = false;
			GameScr.isNewClanMessage = false;
			if (b == 0)
			{
				string text = msg.reader().readUTF();
				GameScr.isNewClanMessage = true;
				if (mFont.tahoma_7.getWidth(text) > Panel.WIDTH_PANEL - 60)
				{
					clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
				}
				else
				{
					clanMessage.chat = new string[1];
					clanMessage.chat[0] = text;
				}
				clanMessage.color = msg.reader().readByte();
			}
			else if (b == 1)
			{
				clanMessage.recieve = msg.reader().readByte();
				clanMessage.maxCap = msg.reader().readByte();
				flag = msg.reader().readByte() == 1;
				if (flag)
				{
					GameScr.isNewClanMessage = true;
				}
				if (clanMessage.playerId != Char.myCharz().charID)
				{
					if (clanMessage.recieve < clanMessage.maxCap)
					{
						clanMessage.option = new string[1] { mResources.donate };
					}
					else
					{
						clanMessage.option = null;
					}
				}
				if (GameCanvas.panel.cp != null)
				{
					GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
				}
			}
			else if (b == 2 && Char.myCharz().role == 0)
			{
				GameScr.isNewClanMessage = true;
				clanMessage.option = new string[2]
				{
					mResources.CANCEL,
					mResources.receive
				};
			}
			if (GameCanvas.currentScreen != GameScr.instance)
			{
				GameScr.isNewClanMessage = false;
			}
			else if (GameCanvas.panel.isShow && GameCanvas.panel.type == 0 && GameCanvas.panel.currentTabIndex == 3)
			{
				GameScr.isNewClanMessage = false;
			}
			ClanMessage.addMessage(clanMessage, index, flag);
		}
		catch (Exception)
		{
			Cout.println("LOI TAI CMD -= " + msg.command);
		}
	}

	public void loadCurrMap(sbyte teleport3)
	{
		Res.outz("[CONTROLER] start load map " + teleport3);
		GameScr.gI().auto = 0;
		GameScr.isChangeZone = false;
		CreateCharScr.instance = null;
		GameScr.info1.isUpdate = false;
		GameScr.info2.isUpdate = false;
		GameScr.lockTick = 0;
		GameCanvas.panel.isShow = false;
		SoundMn.gI().stopAll();
		if (!GameScr.isLoadAllData && !CreateCharScr.isCreateChar)
		{
			GameScr.gI().initSelectChar();
		}
		GameScr.loadCamera(false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
		TileMap.loadMainTile();
		TileMap.loadMap(TileMap.tileID);
		Res.outz("LOAD GAMESCR 2");
		Char.myCharz().cvx = 0;
		Char.myCharz().statusMe = 4;
		Char.myCharz().currentMovePoint = null;
		Char.myCharz().mobFocus = null;
		Char.myCharz().charFocus = null;
		Char.myCharz().npcFocus = null;
		Char.myCharz().itemFocus = null;
		Char.myCharz().skillPaint = null;
		Char.myCharz().setMabuHold(false);
		Char.myCharz().skillPaintRandomPaint = null;
		GameCanvas.clearAllPointerEvent();
		if (Char.myCharz().cy >= TileMap.pxh - 100)
		{
			Char.myCharz().isFlyUp = true;
			Char.myCharz().cx += Res.abs(Res.random(0, 80));
			Service.gI().charMove();
		}
		GameScr.gI().loadGameScr();
		GameCanvas.loadBG(TileMap.bgID);
		Char.isLockKey = false;
		Res.outz("cy= " + Char.myCharz().cy + "---------------------------------------------");
		for (int i = 0; i < Char.myCharz().vEff.size(); i++)
		{
			EffectChar effectChar = (EffectChar)Char.myCharz().vEff.elementAt(i);
			if (effectChar.template.type == 10)
			{
				Char.isLockKey = true;
				break;
			}
		}
		GameCanvas.clearKeyHold();
		GameCanvas.clearKeyPressed();
		GameScr.gI().dHP = Char.myCharz().cHP;
		GameScr.gI().dMP = Char.myCharz().cMP;
		Char.ischangingMap = false;
		GameScr.gI().switchToMe();
		if (Char.myCharz().cy <= 10 && teleport3 != 0 && teleport3 != 2)
		{
			Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender);
			Teleport.addTeleport(p);
			Char.myCharz().isTeleport = true;
		}
		if (teleport3 == 2)
		{
			Char.myCharz().show();
		}
		if (GameScr.gI().isRongThanXuatHien)
		{
			if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
			{
				GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
			}
			if (mGraphics.zoomLevel > 1)
			{
				GameScr.gI().doiMauTroi();
			}
		}
		InfoDlg.hide();
		InfoDlg.show(TileMap.mapName, mResources.zone + " " + TileMap.zoneID, 30);
		GameCanvas.endDlg();
		GameCanvas.isLoading = false;
		Hint.clickMob();
		Hint.clickNpc();
		GameCanvas.debug("SA75x9", 2);
		GameCanvas.isRequestMapID = 2;
		GameCanvas.waitingTimeChangeMap = mSystem.currentTimeMillis() + 1000;
		Res.outz("[CONTROLLER] loadMap DONE!!!!!!!!!");
	}

	public void loadInfoMap(Message msg)
	{
		try
		{
			if (mGraphics.zoomLevel == 1)
			{
				SmallImage.clearHastable();
			}
			Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
			Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
			Char.myCharz().xSd = Char.myCharz().cx;
			Char.myCharz().ySd = Char.myCharz().cy;
			Res.outz("head= " + Char.myCharz().head + " body= " + Char.myCharz().body + " left= " + Char.myCharz().leg + " x= " + Char.myCharz().cx + " y= " + Char.myCharz().cy + " chung toc= " + Char.myCharz().cgender);
			if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
			{
				Char.myCharz().cdir = 1;
			}
			else if (Char.myCharz().cx >= TileMap.tmw - 100 && Char.myCharz().cx <= TileMap.tmw)
			{
				Char.myCharz().cdir = -1;
			}
			GameCanvas.debug("SA75x4", 2);
			int num = msg.reader().readByte();
			Res.outz("vGo size= " + num);
			if (!GameScr.info1.isDone)
			{
				GameScr.info1.cmx = Char.myCharz().cx - GameScr.cmx;
				GameScr.info1.cmy = Char.myCharz().cy - GameScr.cmy;
			}
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = new Waypoint(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readUTF());
				if ((TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23) || waypoint.minX < 0 || waypoint.minX <= 24)
				{
				}
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			GameCanvas.debug("SA75x5", 2);
			num = msg.reader().readByte();
			Mob.newMob.removeAllElements();
			for (sbyte b = 0; b < num; b++)
			{
				Mob mob = new Mob(b, msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readLong(), msg.reader().readByte(), msg.reader().readLong(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readByte());
				mob.xSd = mob.x;
				mob.ySd = mob.y;
				mob.isBoss = msg.reader().readBoolean();
				if (Mob.arrMobTemplate[mob.templateId].type != 0)
				{
					if (b % 3 == 0)
					{
						mob.dir = -1;
					}
					else
					{
						mob.dir = 1;
					}
					mob.x += 10 - b % 20;
				}
				mob.isMobMe = false;
				BigBoss bigBoss = null;
				BachTuoc bachTuoc = null;
				BigBoss2 bigBoss2 = null;
				NewBoss newBoss = null;
				if (mob.templateId == 70)
				{
					bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 71)
				{
					bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
				}
				if (mob.templateId == 72)
				{
					bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
				}
				if (mob.isBoss)
				{
					newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
				}
				if (newBoss != null)
				{
					GameScr.vMob.addElement(newBoss);
				}
				else if (bigBoss != null)
				{
					GameScr.vMob.addElement(bigBoss);
				}
				else if (bachTuoc != null)
				{
					GameScr.vMob.addElement(bachTuoc);
				}
				else if (bigBoss2 != null)
				{
					GameScr.vMob.addElement(bigBoss2);
				}
				else
				{
					GameScr.vMob.addElement(mob);
				}
			}
			if (Char.myCharz().mobMe != null && GameScr.findMobInMap(Char.myCharz().mobMe.mobId) == null)
			{
				Char.myCharz().mobMe.getData();
				Char.myCharz().mobMe.x = Char.myCharz().cx;
				Char.myCharz().mobMe.y = Char.myCharz().cy - 40;
				GameScr.vMob.addElement(Char.myCharz().mobMe);
			}
			num = msg.reader().readByte();
			for (byte b2 = 0; b2 < num; b2++)
			{
			}
			GameCanvas.debug("SA75x6", 2);
			num = msg.reader().readByte();
			Res.outz("NPC size= " + num);
			for (int j = 0; j < num; j++)
			{
				sbyte b3 = msg.reader().readByte();
				short cx = msg.reader().readShort();
				short num2 = msg.reader().readShort();
				sbyte b4 = msg.reader().readByte();
				short num3 = msg.reader().readShort();
				if (b4 != 6 && ((Char.myCharz().taskMaint.taskId >= 7 && (Char.myCharz().taskMaint.taskId != 7 || Char.myCharz().taskMaint.index > 1)) || (b4 != 7 && b4 != 8 && b4 != 9)) && (Char.myCharz().taskMaint.taskId >= 6 || b4 != 16))
				{
					if (b4 == 4)
					{
						GameScr.gI().magicTree = new MagicTree(j, b3, cx, num2, b4, num3);
						Service.gI().magicTree(2);
						GameScr.vNpc.addElement(GameScr.gI().magicTree);
					}
					else
					{
						Npc o = new Npc(j, b3, cx, num2 + 3, b4, num3);
						GameScr.vNpc.addElement(o);
					}
				}
			}
			GameCanvas.debug("SA75x7", 2);
			num = msg.reader().readByte();
			string empty = string.Empty;
			Res.outz("item size = " + num);
			empty = empty + "item: " + num;
			for (int k = 0; k < num; k++)
			{
				short itemMapID = msg.reader().readShort();
				short num4 = msg.reader().readShort();
				int x = msg.reader().readShort();
				int y = msg.reader().readShort();
				int num5 = msg.reader().readInt();
				short r = 0;
				if (num5 == -2)
				{
					r = msg.reader().readShort();
				}
				ItemMap itemMap = new ItemMap(num5, itemMapID, num4, x, y, r);
				bool flag = false;
				for (int l = 0; l < GameScr.vItemMap.size(); l++)
				{
					ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(l);
					if (itemMap2.itemMapID == itemMap.itemMapID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GameScr.vItemMap.addElement(itemMap);
				}
				empty = empty + num4 + ",";
			}
			Res.err("sl item on map " + empty + "\n");
			TileMap.vCurrItem.removeAllElements();
			if (mGraphics.zoomLevel == 1)
			{
				BgItem.clearHashTable();
			}
			BgItem.vKeysNew.removeAllElements();
			if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && TileMap.isVoDaiMap()) || TileMap.mapID == 45 || TileMap.mapID == 46 || TileMap.mapID == 47 || TileMap.mapID == 48 || TileMap.mapID == 120 || TileMap.mapID == 128 || TileMap.mapID == 170 || TileMap.mapID == 49)
			{
				short num6 = msg.reader().readShort();
				empty = "item high graphic: ";
				for (int m = 0; m < num6; m++)
				{
					short num7 = msg.reader().readShort();
					short num8 = msg.reader().readShort();
					short num9 = msg.reader().readShort();
					if (TileMap.getBIById(num7) != null)
					{
						BgItem bIById = TileMap.getBIById(num7);
						BgItem bgItem = new BgItem();
						bgItem.id = num7;
						bgItem.idImage = bIById.idImage;
						bgItem.dx = bIById.dx;
						bgItem.dy = bIById.dy;
						bgItem.x = num8 * TileMap.size;
						bgItem.y = num9 * TileMap.size;
						bgItem.layer = bIById.layer;
						if (TileMap.isExistMoreOne(bgItem.id))
						{
							bgItem.trans = ((m % 2 != 0) ? 2 : 0);
							if (TileMap.mapID == 45)
							{
								bgItem.trans = 0;
							}
						}
						Image image = null;
						if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
						{
							if (mGraphics.zoomLevel == 1)
							{
								image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image == null)
								{
									image = Image.createRGBImage(new int[1], 1, 1, true);
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
							}
							else
							{
								bool flag2 = false;
								sbyte[] array = Rms.loadRMS(mGraphics.zoomLevel + "bgItem" + bgItem.idImage);
								if (array != null)
								{
									if (BgItem.newSmallVersion != null)
									{
										Res.outz("Small  last= " + array.Length % 127 + "new Version= " + BgItem.newSmallVersion[bgItem.idImage]);
										if (array.Length % 127 != BgItem.newSmallVersion[bgItem.idImage])
										{
											flag2 = true;
										}
									}
									if (!flag2)
									{
										image = Image.createImage(array, 0, array.Length);
										if (image != null)
										{
											BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
										}
										else
										{
											flag2 = true;
										}
									}
								}
								else
								{
									flag2 = true;
								}
								if (flag2)
								{
									image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
									if (image == null)
									{
										image = Image.createRGBImage(new int[1], 1, 1, true);
										Service.gI().getBgTemplate(bgItem.idImage);
									}
									BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
								}
							}
							BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
						}
						if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
						{
							BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
						}
						bgItem.changeColor();
						TileMap.vCurrItem.addElement(bgItem);
					}
					empty = empty + num7 + ",";
				}
				Res.err("item High Graphics: " + empty);
				for (int n = 0; n < BgItem.vKeysLast.size(); n++)
				{
					string text = (string)BgItem.vKeysLast.elementAt(n);
					if (!BgItem.isExistKeyNews(text))
					{
						BgItem.imgNew.remove(text);
						if (BgItem.imgNew.containsKey(text + "blend" + 1))
						{
							BgItem.imgNew.remove(text + "blend" + 1);
						}
						if (BgItem.imgNew.containsKey(text + "blend" + 3))
						{
							BgItem.imgNew.remove(text + "blend" + 3);
						}
						BgItem.vKeysLast.removeElementAt(n);
						n--;
					}
				}
				BackgroudEffect.isFog = false;
				BackgroudEffect.nCloud = 0;
				EffecMn.vEff.removeAllElements();
				BackgroudEffect.vBgEffect.removeAllElements();
				Effect.newEff.removeAllElements();
				short num10 = msg.reader().readShort();
				for (int num11 = 0; num11 < num10; num11++)
				{
					string key = msg.reader().readUTF();
					string value = msg.reader().readUTF();
					keyValueAction(key, value);
				}
			}
			else
			{
				short num12 = msg.reader().readShort();
				for (int num13 = 0; num13 < num12; num13++)
				{
					short num14 = msg.reader().readShort();
					short num15 = msg.reader().readShort();
					short num16 = msg.reader().readShort();
				}
				short num17 = msg.reader().readShort();
				for (int num18 = 0; num18 < num17; num18++)
				{
					string text2 = msg.reader().readUTF();
					string text3 = msg.reader().readUTF();
				}
			}
			TileMap.bgType = msg.reader().readByte();
			sbyte teleport = msg.reader().readByte();
			loadCurrMap(teleport);
			GameCanvas.debug("SA75x8", 2);
		}
		catch (Exception)
		{
			Res.err(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Loadmap khong thanh cong");
			GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
			ServerListScreen.waitToLogin = true;
			GameCanvas.endDlg();
		}
		GameCanvas.isLoading = false;
		Res.err(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Loadmap thanh cong");
	}

	public void keyValueAction(string key, string value)
	{
		if (key.Equals("eff"))
		{
			if (Panel.graphics > 0)
			{
				return;
			}
			string[] array = Res.split(value, ".", 0);
			int id = int.Parse(array[0]);
			int layer = int.Parse(array[1]);
			int x = int.Parse(array[2]);
			int y = int.Parse(array[3]);
			int loop;
			int loopCount;
			if (array.Length <= 4)
			{
				loop = -1;
				loopCount = 1;
			}
			else
			{
				loop = int.Parse(array[4]);
				loopCount = int.Parse(array[5]);
			}
			Effect effect = new Effect(id, x, y, layer, loop, loopCount);
			if (array.Length > 6)
			{
				effect.typeEff = int.Parse(array[6]);
				if (array.Length > 7)
				{
					effect.indexFrom = int.Parse(array[7]);
					effect.indexTo = int.Parse(array[8]);
				}
			}
			EffecMn.addEff(effect);
		}
		else if (key.Equals("beff") && Panel.graphics <= 1)
		{
			BackgroudEffect.addEffect(int.Parse(value));
		}
	}

	public void messageNotMap(Message msg)
	{
		GameCanvas.debug("SA6", 2);
		try
		{
			sbyte b = msg.reader().readByte();
			Res.outz("---messageNotMap : " + b);
			switch (b)
			{
			case 16:
				MoneyCharge.gI().switchToMe();
				break;
			case 17:
				GameCanvas.debug("SYB123", 2);
				Char.myCharz().clearTask();
				break;
			case 18:
			{
				GameCanvas.isLoading = false;
				GameCanvas.endDlg();
				int num2 = msg.reader().readInt();
				GameCanvas.inputDlg.show(mResources.changeNameChar, new Command(mResources.OK, GameCanvas.instance, 88829, num2), TField.INPUT_TYPE_ANY);
				break;
			}
			case 20:
				Char.myCharz().cPk = msg.reader().readByte();
				GameScr.info1.addInfo(mResources.PK_NOW + " " + Char.myCharz().cPk, 0);
				break;
			case 35:
				GameCanvas.endDlg();
				GameScr.gI().resetButton();
				GameScr.info1.addInfo(msg.reader().readUTF(), 0);
				break;
			case 36:
				GameScr.typeActive = msg.reader().readByte();
				Res.outz("load Me Active: " + GameScr.typeActive);
				break;
			case 4:
			{
				GameCanvas.debug("SA8", 2);
				GameCanvas.loginScr.savePass();
				GameScr.isAutoPlay = false;
				GameScr.canAutoPlay = false;
				LoginScr.isUpdateAll = true;
				LoginScr.isUpdateData = true;
				LoginScr.isUpdateMap = true;
				LoginScr.isUpdateSkill = true;
				LoginScr.isUpdateItem = true;
				GameScr.vsData = msg.reader().readByte();
				GameScr.vsMap = msg.reader().readByte();
				GameScr.vsSkill = msg.reader().readByte();
				GameScr.vsItem = msg.reader().readByte();
				sbyte b3 = msg.reader().readByte();
				if (GameCanvas.loginScr.isLogin2)
				{
					Rms.saveRMSString("acc", string.Empty);
					Rms.saveRMSString("pass", string.Empty);
				}
				else
				{
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
				}
				if (GameScr.vsData != GameScr.vcData)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateData();
				}
				else
				{
					try
					{
						LoginScr.isUpdateData = false;
					}
					catch (Exception)
					{
						GameScr.vcData = -1;
						Service.gI().updateData();
					}
				}
				if (GameScr.vsMap != GameScr.vcMap)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateMap();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream = new DataInputStream(Rms.loadRMS("NRmap"));
							createMap(dataInputStream.r);
						}
						LoginScr.isUpdateMap = false;
					}
					catch (Exception)
					{
						GameScr.vcMap = -1;
						Service.gI().updateMap();
					}
				}
				if (GameScr.vsSkill != GameScr.vcSkill)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateSkill();
				}
				else
				{
					try
					{
						if (!GameScr.isLoadAllData)
						{
							DataInputStream dataInputStream2 = new DataInputStream(Rms.loadRMS("NRskill"));
							createSkill(dataInputStream2.r);
						}
						LoginScr.isUpdateSkill = false;
					}
					catch (Exception)
					{
						GameScr.vcSkill = -1;
						Service.gI().updateSkill();
					}
				}
				if (GameScr.vsItem != GameScr.vcItem)
				{
					GameScr.isLoadAllData = false;
					Service.gI().updateItem();
				}
				else
				{
					try
					{
						DataInputStream dataInputStream3 = new DataInputStream(Rms.loadRMS("NRitem0"));
						loadItemNew(dataInputStream3.r, 0, false);
						DataInputStream dataInputStream4 = new DataInputStream(Rms.loadRMS("NRitem1"));
						loadItemNew(dataInputStream4.r, 1, false);
						DataInputStream dataInputStream5 = new DataInputStream(Rms.loadRMS("NRitem100"));
						loadItemNew(dataInputStream5.r, 100, false);
						LoginScr.isUpdateItem = false;
					}
					catch (Exception)
					{
						GameScr.vcItem = -1;
						Service.gI().updateItem();
					}
					try
					{
						DataInputStream dataInputStream6 = new DataInputStream(Rms.loadRMS("NRitem101"));
						loadItemNew(dataInputStream6.r, 101, false);
					}
					catch (Exception)
					{
					}
				}
				if (!GameScr.isLoadAllData)
				{
					GameScr.gI().readOk();
				}
				else
				{
					Service.gI().clientOk();
				}
				sbyte b4 = msg.reader().readByte();
				Res.outz("CAPTION LENT= " + b4);
				GameScr.exps = new long[b4];
				for (int j = 0; j < GameScr.exps.Length; j++)
				{
					GameScr.exps[j] = msg.reader().readLong();
				}
				break;
			}
			case 6:
			{
				Res.outz("GET UPDATE_MAP " + msg.reader().available() + " bytes");
				msg.reader().mark(500000);
				createMap(msg.reader());
				msg.reader().reset();
				sbyte[] data3 = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data3);
				Rms.saveRMS("NRmap", data3);
				sbyte[] data4 = new sbyte[1] { GameScr.vcMap };
				Rms.saveRMS("NRmapVersion", data4);
				LoginScr.isUpdateMap = false;
				GameScr.gI().readOk();
				break;
			}
			case 7:
			{
				Res.outz("GET UPDATE_SKILL " + msg.reader().available() + " bytes");
				msg.reader().mark(500000);
				createSkill(msg.reader());
				msg.reader().reset();
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().readFully(ref data);
				Rms.saveRMS("NRskill", data);
				sbyte[] data2 = new sbyte[1] { GameScr.vcSkill };
				Rms.saveRMS("NRskillVersion", data2);
				LoginScr.isUpdateSkill = false;
				GameScr.gI().readOk();
				break;
			}
			case 8:
				Res.outz("GET UPDATE_ITEM " + msg.reader().available() + " bytes");
				createItemNew(msg.reader());
				break;
			case 10:
				try
				{
					Char.isLoadingMap = true;
					Res.outz("REQUEST MAP TEMPLATE");
					GameCanvas.isLoading = true;
					TileMap.maps = null;
					TileMap.types = null;
					mSystem.gcc();
					GameCanvas.debug("SA99", 2);
					TileMap.tmw = msg.reader().readByte();
					TileMap.tmh = msg.reader().readByte();
					TileMap.maps = new int[TileMap.tmw * TileMap.tmh];
					Res.err("   M apsize= " + TileMap.tmw * TileMap.tmh);
					for (int i = 0; i < TileMap.maps.Length; i++)
					{
						int num = msg.reader().readByte();
						if (num < 0)
						{
							num += 256;
						}
						TileMap.maps[i] = (ushort)num;
					}
					TileMap.types = new int[TileMap.maps.Length];
					msg = messWait;
					loadInfoMap(msg);
					try
					{
						sbyte b2 = msg.reader().readByte();
						TileMap.isMapDouble = ((b2 != 0) ? true : false);
					}
					catch (Exception ex)
					{
						Res.err(" 1 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex.ToString());
					}
				}
				catch (Exception ex2)
				{
					Res.err("2 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex2.ToString());
				}
				msg.cleanup();
				messWait.cleanup();
				msg = (messWait = null);
				GameScr.gI().switchToMe();
				break;
			case 9:
				GameCanvas.debug("SA11", 2);
				break;
			}
		}
		catch (Exception ex8)
		{
			Cout.LogError("LOI TAI messageNotMap=== " + msg.command + "  >>" + ex8.StackTrace);
		}
		finally
		{
			if (msg != null)
			{
				msg.cleanup();
			}
		}
	}

	public void messageNotLogin(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			Res.outz("---messageNotLogin : " + b);
			if (b == 2)
			{
				string linkDefault = msg.reader().readUTF();
				Res.outz(">>Get CLIENT_INFO");
				ServerListScreen.linkDefault = linkDefault;
				mSystem.AddIpTest();
				ServerListScreen.getServerList(ServerListScreen.linkDefault);
				try
				{
					sbyte b2 = msg.reader().readByte();
					Panel.CanNapTien = b2 == 1;
				}
				catch (Exception)
				{
				}
				isGet_CLIENT_INFO = true;
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			if (msg != null)
			{
				msg.cleanup();
			}
		}
	}

	public void messageSubCommand(Message msg)
	{
		try
		{
			GameCanvas.debug("SA12", 2);
			sbyte b = msg.reader().readByte();
			Res.outz("---messageSubCommand : " + b);
			switch (b)
			{
			case 63:
			{
				sbyte b5 = msg.reader().readByte();
				if (b5 > 0)
				{
					GameCanvas.panel.vPlayerMenu_id.removeAllElements();
					InfoDlg.showWait();
					MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
					for (int j = 0; j < b5; j++)
					{
						string caption = msg.reader().readUTF();
						string caption2 = msg.reader().readUTF();
						short num5 = msg.reader().readShort();
						GameCanvas.panel.vPlayerMenu_id.addElement(num5 + string.Empty);
						Char.myCharz().charFocus.menuSelect = num5;
						Command command = new Command(caption, 11115, Char.myCharz().charFocus);
						command.caption2 = caption2;
						vPlayerMenu.addElement(command);
					}
					InfoDlg.hide();
					GameCanvas.panel.setTabPlayerMenu();
				}
				break;
			}
			case 1:
				GameCanvas.debug("SA13", 2);
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().cTiemNang = msg.reader().readLong();
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				Char.myCharz().myskill = null;
				break;
			case 2:
			{
				GameCanvas.debug("SA14", 2);
				if (Char.myCharz().statusMe != 14 && Char.myCharz().statusMe != 5)
				{
					Char.myCharz().cHP = Char.myCharz().cHPFull;
					Char.myCharz().cMP = Char.myCharz().cMPFull;
					Cout.LogError2(" ME_LOAD_SKILL");
				}
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				sbyte b2 = msg.reader().readByte();
				for (sbyte b3 = 0; b3 < b2; b3++)
				{
					short skillId = msg.reader().readShort();
					Skill skill2 = Skills.get(skillId);
					useSkill(skill2);
				}
				GameScr.gI().sortSkill();
				if (GameScr.isPaintInfoMe)
				{
					GameScr.indexRow = -1;
					GameScr.gI().left = (GameScr.gI().center = null);
				}
				break;
			}
			case 19:
				GameCanvas.debug("SA17", 2);
				Char.myCharz().boxSort();
				break;
			case 21:
			{
				GameCanvas.debug("SA19", 2);
				int num3 = msg.reader().readInt();
				Char.myCharz().xuInBox -= num3;
				Char.myCharz().xu += num3;
				Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
				break;
			}
			case 0:
			{
				GameCanvas.debug("SA21", 2);
				RadarScr.list = new MyVector();
				Teleport.vTeleport.removeAllElements();
				GameScr.vCharInMap.removeAllElements();
				GameScr.vItemMap.removeAllElements();
				Char.vItemTime.removeAllElements();
				GameScr.loadImg();
				GameScr.currentCharViewInfo = Char.myCharz();
				Char.myCharz().charID = msg.reader().readInt();
				Char.myCharz().ctaskId = msg.reader().readByte();
				Char.myCharz().cgender = msg.reader().readByte();
				Char.myCharz().head = msg.reader().readShort();
				Char.myCharz().cName = msg.reader().readUTF();
				Char.myCharz().cPk = msg.reader().readByte();
				Char.myCharz().cTypePk = msg.reader().readByte();
				Char.myCharz().cPower = msg.reader().readLong();
				Char.myCharz().applyCharLevelPercent();
				Char.myCharz().eff5BuffHp = msg.reader().readShort();
				Char.myCharz().eff5BuffMp = msg.reader().readShort();
				Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
				Char.myCharz().vSkill.removeAllElements();
				Char.myCharz().vSkillFight.removeAllElements();
				GameScr.gI().dHP = Char.myCharz().cHP;
				GameScr.gI().dMP = Char.myCharz().cMP;
				sbyte b2 = msg.reader().readByte();
				for (sbyte b6 = 0; b6 < b2; b6++)
				{
					Skill skill3 = Skills.get(msg.reader().readShort());
					useSkill(skill3);
				}
				GameScr.gI().sortSkill();
				GameScr.gI().loadSkillShortcut();
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				Char.myCharz().arrItemBody = new Item[msg.reader().readByte()];
				try
				{
					Char.myCharz().setDefaultPart();
					for (int k = 0; k < Char.myCharz().arrItemBody.Length; k++)
					{
						short num6 = msg.reader().readShort();
						if (num6 == -1)
						{
							continue;
						}
						ItemTemplate itemTemplate = ItemTemplates.get(num6);
						int num7 = itemTemplate.type;
						Char.myCharz().arrItemBody[k] = new Item();
						Char.myCharz().arrItemBody[k].template = itemTemplate;
						Char.myCharz().arrItemBody[k].quantity = msg.reader().readInt();
						Char.myCharz().arrItemBody[k].info = msg.reader().readUTF();
						Char.myCharz().arrItemBody[k].content = msg.reader().readUTF();
						int num8 = msg.reader().readUnsignedByte();
						if (num8 != 0)
						{
							Char.myCharz().arrItemBody[k].itemOption = new ItemOption[num8];
							for (int l = 0; l < Char.myCharz().arrItemBody[k].itemOption.Length; l++)
							{
								ItemOption itemOption = readItemOption(msg);
								if (itemOption != null)
								{
									Char.myCharz().arrItemBody[k].itemOption[l] = itemOption;
								}
							}
						}
						switch (num7)
						{
						case 0:
							Res.outz("toi day =======================================" + Char.myCharz().body);
							Char.myCharz().body = Char.myCharz().arrItemBody[k].template.part;
							break;
						case 1:
							Char.myCharz().leg = Char.myCharz().arrItemBody[k].template.part;
							Res.outz("toi day =======================================" + Char.myCharz().leg);
							break;
						}
					}
				}
				catch (Exception)
				{
				}
				Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
				GameScr.hpPotion = 0;
				GameScr.isudungCapsun4 = false;
				GameScr.isudungCapsun3 = false;
				for (int m = 0; m < Char.myCharz().arrItemBag.Length; m++)
				{
					short num9 = msg.reader().readShort();
					if (num9 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBag[m] = new Item();
					Char.myCharz().arrItemBag[m].template = ItemTemplates.get(num9);
					Char.myCharz().arrItemBag[m].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBag[m].info = msg.reader().readUTF();
					Char.myCharz().arrItemBag[m].content = msg.reader().readUTF();
					Char.myCharz().arrItemBag[m].indexUI = m;
					sbyte b7 = msg.reader().readByte();
					if (b7 != 0)
					{
						Char.myCharz().arrItemBag[m].itemOption = new ItemOption[b7];
						for (int n = 0; n < Char.myCharz().arrItemBag[m].itemOption.Length; n++)
						{
							ItemOption itemOption2 = readItemOption(msg);
							if (itemOption2 != null)
							{
								Char.myCharz().arrItemBag[m].itemOption[n] = itemOption2;
								Char.myCharz().arrItemBag[m].getCompare();
							}
						}
					}
					if (Char.myCharz().arrItemBag[m].template.type == 6)
					{
						GameScr.hpPotion += Char.myCharz().arrItemBag[m].quantity;
					}
					switch (num9)
					{
					case 194:
						GameScr.isudungCapsun4 = Char.myCharz().arrItemBag[m].quantity > 0;
						break;
					case 193:
						if (!GameScr.isudungCapsun4)
						{
							GameScr.isudungCapsun3 = Char.myCharz().arrItemBag[m].quantity > 0;
						}
						break;
					}
				}
				Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
				GameCanvas.panel.hasUse = 0;
				for (int num10 = 0; num10 < Char.myCharz().arrItemBox.Length; num10++)
				{
					short num11 = msg.reader().readShort();
					if (num11 == -1)
					{
						continue;
					}
					Char.myCharz().arrItemBox[num10] = new Item();
					Char.myCharz().arrItemBox[num10].template = ItemTemplates.get(num11);
					Char.myCharz().arrItemBox[num10].quantity = msg.reader().readInt();
					Char.myCharz().arrItemBox[num10].info = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num10].content = msg.reader().readUTF();
					Char.myCharz().arrItemBox[num10].itemOption = new ItemOption[msg.reader().readByte()];
					for (int num12 = 0; num12 < Char.myCharz().arrItemBox[num10].itemOption.Length; num12++)
					{
						ItemOption itemOption3 = readItemOption(msg);
						if (itemOption3 != null)
						{
							Char.myCharz().arrItemBox[num10].itemOption[num12] = itemOption3;
							Char.myCharz().arrItemBox[num10].getCompare();
						}
					}
					GameCanvas.panel.hasUse++;
				}
				Char.myCharz().statusMe = 4;
				int num13 = Rms.loadRMSInt(Char.myCharz().cName + "vci");
				if (num13 < 1)
				{
					GameScr.isViewClanInvite = false;
				}
				else
				{
					GameScr.isViewClanInvite = true;
				}
				short num14 = msg.reader().readShort();
				Char.idHead = new short[num14];
				Char.idAvatar = new short[num14];
				for (int num15 = 0; num15 < num14; num15++)
				{
					Char.idHead[num15] = msg.reader().readShort();
					Char.idAvatar[num15] = msg.reader().readShort();
				}
				for (int num16 = 0; num16 < GameScr.info1.charId.Length; num16++)
				{
					GameScr.info1.charId[num16] = new int[3];
				}
				GameScr.info1.charId[Char.myCharz().cgender][0] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][1] = msg.reader().readShort();
				GameScr.info1.charId[Char.myCharz().cgender][2] = msg.reader().readShort();
				Char.myCharz().isNhapThe = msg.reader().readByte() == 1;
				Res.outz("NHAP THE= " + Char.myCharz().isNhapThe);
				GameScr.deltaTime = mSystem.currentTimeMillis() - (long)msg.reader().readInt() * 1000L;
				GameScr.isNewMember = msg.reader().readByte();
				Service.gI().updateCaption((sbyte)Char.myCharz().cgender);
				Service.gI().androidPack();
				try
				{
					Char.myCharz().idAuraEff = msg.reader().readShort();
					Char.myCharz().idEff_Set_Item = msg.reader().readSByte();
					Char.myCharz().idHat = msg.reader().readShort();
					break;
				}
				catch (Exception)
				{
					break;
				}
			}
			case 4:
				GameCanvas.debug("SA23", 2);
				Char.myCharz().xu = msg.reader().readLong();
				Char.myCharz().luong = msg.reader().readInt();
				Char.myCharz().cHP = msg.reader().readLong();
				Char.myCharz().cMP = msg.reader().readLong();
				Char.myCharz().luongKhoa = msg.reader().readInt();
				Char.myCharz().xuStr = Res.formatNumber2(Char.myCharz().xu);
				Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
				Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
				break;
			case 5:
			{
				GameCanvas.debug("SA24", 2);
				long cHP = Char.myCharz().cHP;
				Char.myCharz().cHP = msg.reader().readLong();
				if (Char.myCharz().cHP > cHP && Char.myCharz().cTypePk != 4)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cHP - cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5003)
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, true, -1L, -1L, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cHP < cHP)
				{
					GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
				}
				GameScr.gI().dHP = Char.myCharz().cHP;
				if (GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 6:
			{
				GameCanvas.debug("SA25", 2);
				if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
				{
					break;
				}
				long cMP = Char.myCharz().cMP;
				Char.myCharz().cMP = msg.reader().readLong();
				if (Char.myCharz().cMP > cMP)
				{
					GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
					SoundMn.gI().HP_MPup();
					if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
					{
						MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, true, -1L, -1L, Char.myCharz(), 29);
					}
				}
				if (Char.myCharz().cMP < cMP)
				{
					GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
				}
				Res.outz("curr MP= " + Char.myCharz().cMP);
				GameScr.gI().dMP = Char.myCharz().cMP;
				if (GameScr.isPaintInfoMe)
				{
				}
				break;
			}
			case 7:
			{
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					break;
				}
				obj.clanID = msg.reader().readInt();
				if (obj.clanID == -2)
				{
					obj.isCopy = true;
				}
				readCharInfo(obj, msg);
				try
				{
					obj.idAuraEff = msg.reader().readShort();
					obj.idEff_Set_Item = msg.reader().readSByte();
					obj.idHat = msg.reader().readShort();
					if (obj.bag >= 201)
					{
						Effect effect = new Effect(obj.bag, obj, 2, -1, 10, 1);
						effect.typeEff = 5;
						obj.addEffChar(effect);
					}
					else
					{
						obj.removeEffChar(0, 201);
					}
					break;
				}
				catch (Exception)
				{
					break;
				}
			}
			case 8:
			{
				GameCanvas.debug("SA26", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cspeed = msg.reader().readByte();
				}
				break;
			}
			case 9:
			{
				GameCanvas.debug("SA27", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
				}
				break;
			}
			case 10:
			{
				GameCanvas.debug("SA28", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
					obj.eff5BuffHp = msg.reader().readShort();
					obj.eff5BuffMp = msg.reader().readShort();
					obj.wp = msg.reader().readShort();
					if (obj.wp == -1)
					{
						obj.setDefaultWeapon();
					}
				}
				break;
			}
			case 11:
			{
				GameCanvas.debug("SA29", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
					obj.eff5BuffHp = msg.reader().readShort();
					obj.eff5BuffMp = msg.reader().readShort();
					obj.body = msg.reader().readShort();
					if (obj.body == -1)
					{
						obj.setDefaultBody();
					}
				}
				break;
			}
			case 12:
			{
				GameCanvas.debug("SA30", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
					obj.eff5BuffHp = msg.reader().readShort();
					obj.eff5BuffMp = msg.reader().readShort();
					obj.leg = msg.reader().readShort();
					if (obj.leg == -1)
					{
						obj.setDefaultLeg();
					}
				}
				break;
			}
			case 13:
			{
				GameCanvas.debug("SA31", 2);
				int num2 = msg.reader().readInt();
				Char obj = ((num2 != Char.myCharz().charID) ? GameScr.findCharInMap(num2) : Char.myCharz());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
					obj.eff5BuffHp = msg.reader().readShort();
					obj.eff5BuffMp = msg.reader().readShort();
				}
				break;
			}
			case 14:
			{
				GameCanvas.debug("SA32", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj == null)
				{
					break;
				}
				obj.cHP = msg.reader().readLong();
				sbyte b4 = msg.reader().readByte();
				Res.outz("player load hp type= " + b4);
				if (b4 == 1)
				{
					ServerEffect.addServerEffect(11, obj, 5);
					ServerEffect.addServerEffect(104, obj, 4);
				}
				if (b4 == 2)
				{
					obj.doInjure();
				}
				try
				{
					obj.cHPFull = msg.reader().readLong();
					break;
				}
				catch (Exception)
				{
					break;
				}
			}
			case 15:
			{
				GameCanvas.debug("SA33", 2);
				Char obj = GameScr.findCharInMap(msg.reader().readInt());
				if (obj != null)
				{
					obj.cHP = msg.reader().readLong();
					obj.cHPFull = msg.reader().readLong();
					obj.cx = msg.reader().readShort();
					obj.cy = msg.reader().readShort();
					obj.statusMe = 1;
					obj.cp3 = 3;
					ServerEffect.addServerEffect(109, obj, 2);
				}
				break;
			}
			case 35:
			{
				GameCanvas.debug("SY3", 2);
				int num4 = msg.reader().readInt();
				Res.outz("CID = " + num4);
				if (TileMap.mapID == 130)
				{
					GameScr.gI().starVS();
				}
				if (num4 == Char.myCharz().charID)
				{
					Char.myCharz().cTypePk = msg.reader().readByte();
					if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
					{
						GameScr.gI().starVS();
					}
					Res.outz("type pk= " + Char.myCharz().cTypePk);
					Char.myCharz().npcFocus = null;
					if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
					{
						Char.myCharz().mobFocus = null;
					}
					Char.myCharz().itemFocus = null;
				}
				else
				{
					Char obj = GameScr.findCharInMap(num4);
					if (obj != null)
					{
						Res.outz("type pk= " + obj.cTypePk);
						obj.cTypePk = msg.reader().readByte();
						if (obj.isAttacPlayerStatus())
						{
							Char.myCharz().charFocus = obj;
						}
					}
				}
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char obj2 = GameScr.findCharInMap(i);
					if (obj2 != null && obj2.cTypePk != 0 && obj2.cTypePk == Char.myCharz().cTypePk)
					{
						if (!Char.myCharz().mobFocus.isMobMe)
						{
							Char.myCharz().mobFocus = null;
						}
						Char.myCharz().npcFocus = null;
						Char.myCharz().itemFocus = null;
						break;
					}
				}
				Res.outz("update type pk= ");
				break;
			}
			case 61:
			{
				string text = msg.reader().readUTF();
				sbyte[] data = new sbyte[msg.reader().readInt()];
				msg.reader().read(ref data);
				if (data.Length == 0)
				{
					data = null;
				}
				if (text.Equals("KSkill"))
				{
					GameScr.gI().onKSkill(data);
				}
				else if (text.Equals("OSkill"))
				{
					GameScr.gI().onOSkill(data);
				}
				else if (text.Equals("CSkill"))
				{
					GameScr.gI().onCSkill(data);
				}
				break;
			}
			case 23:
			{
				short num = msg.reader().readShort();
				Skill skill = Skills.get(num);
				useSkill(skill);
				if (num != 0 && num != 14 && num != 28)
				{
					GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill.template.name, 0);
				}
				break;
			}
			case 62:
				Res.outz("ME UPDATE SKILL");
				read_UpdateSkill(msg);
				break;
			}
		}
		catch (Exception ex5)
		{
			Cout.println("Loi tai Sub : " + ex5.ToString());
		}
		finally
		{
			if (msg != null)
			{
				msg.cleanup();
			}
		}
	}

	private void useSkill(Skill skill)
	{
		if (Char.myCharz().myskill == null)
		{
			Char.myCharz().myskill = skill;
		}
		else if (skill.template.Equals(Char.myCharz().myskill.template))
		{
			Char.myCharz().myskill = skill;
		}
		Char.myCharz().vSkill.addElement(skill);
		if ((skill.template.type == 1 || skill.template.type == 4 || skill.template.type == 2 || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
		{
			if (skill.template.id == Char.myCharz().skillTemplateId)
			{
				Service.gI().selectSkill(Char.myCharz().skillTemplateId);
			}
			Char.myCharz().vSkillFight.addElement(skill);
		}
	}

	public bool readCharInfo(Char c, Message msg)
	{
		try
		{
			c.clevel = msg.reader().readByte();
			c.isInvisiblez = msg.reader().readBoolean();
			c.cTypePk = msg.reader().readByte();
			Res.outz("ADD TYPE PK= " + c.cTypePk + " to player " + c.charID + " @@ " + c.cName);
			c.nClass = GameScr.nClasss[msg.reader().readByte()];
			c.cgender = msg.reader().readByte();
			c.head = msg.reader().readShort();
			c.cName = msg.reader().readUTF();
			c.cHP = msg.reader().readLong();
			c.dHP = c.cHP;
			if (c.cHP == 0)
			{
				c.statusMe = 14;
			}
			c.cHPFull = msg.reader().readLong();
			if (c.cy >= TileMap.pxh - 100)
			{
				c.isFlyUp = true;
			}
			c.body = msg.reader().readShort();
			c.leg = msg.reader().readShort();
			c.bag = msg.reader().readShort();
			Res.outz(" body= " + c.body + " leg= " + c.leg + " bag=" + c.bag + "BAG ==" + c.bag + "*********************************");
			c.isShadown = true;
			sbyte b = msg.reader().readByte();
			if (c.wp == -1)
			{
				c.setDefaultWeapon();
			}
			if (c.body == -1)
			{
				c.setDefaultBody();
			}
			if (c.leg == -1)
			{
				c.setDefaultLeg();
			}
			c.cx = msg.reader().readShort();
			c.cy = msg.reader().readShort();
			c.xSd = c.cx;
			c.ySd = c.cy;
			c.eff5BuffHp = msg.reader().readShort();
			c.eff5BuffMp = msg.reader().readShort();
			int num = msg.reader().readByte();
			for (int i = 0; i < num; i++)
			{
				EffectChar effectChar = new EffectChar(msg.reader().readByte(), msg.reader().readInt(), msg.reader().readInt(), msg.reader().readShort());
				c.vEff.addElement(effectChar);
				if (effectChar.template.type == 12 || effectChar.template.type == 11)
				{
					c.isInvisiblez = true;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		return false;
	}

	private void readGetImgByName(Message msg)
	{
		try
		{
			string name = msg.reader().readUTF();
			sbyte nFrame = msg.reader().readByte();
			sbyte[] array = null;
			array = NinjaUtil.readByteArray(msg);
			Image img = createImage(array);
			ImgByName.SetImage(name, img, nFrame);
			if (array == null)
			{
			}
		}
		catch (Exception)
		{
		}
	}

	private void createItemNew(myReader d)
	{
		try
		{
			loadItemNew(d, -1, true);
		}
		catch (Exception)
		{
		}
	}

	private void loadItemNew(myReader d, sbyte type, bool isSave)
	{
		try
		{
			d.mark(1000000);
			GameScr.vcItem = d.readByte();
			type = d.readByte();
			Res.err(GameScr.vcItem + ":<<GameScr.vcItem >>>>>>loadItemNew: " + type + "  isSave:" + isSave);
			if (type == 0)
			{
				GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readShort()];
				for (int i = 0; i < GameScr.gI().iOptionTemplates.Length; i++)
				{
					GameScr.gI().iOptionTemplates[i] = new ItemOptionTemplate();
					GameScr.gI().iOptionTemplates[i].id = i;
					GameScr.gI().iOptionTemplates[i].name = d.readUTF();
					GameScr.gI().iOptionTemplates[i].type = d.readByte();
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data = new sbyte[d.available()];
					d.readFully(ref data);
					Rms.saveRMS("NRitem0", data);
				}
			}
			else if (type == 1)
			{
				ItemTemplates.itemTemplates.clear();
				int num = d.readShort();
				for (int j = 0; j < num; j++)
				{
					ItemTemplate it = new ItemTemplate((short)j, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
					ItemTemplates.add(it);
				}
				if (isSave)
				{
					d.reset();
					sbyte[] data2 = new sbyte[d.available()];
					d.readFully(ref data2);
					Rms.saveRMS("NRitem1", data2);
					sbyte[] data3 = new sbyte[1] { GameScr.vcItem };
					Rms.saveRMS("NRitemVersion", data3);
				}
				LoginScr.isUpdateItem = false;
				GameScr.gI().readOk();
			}
			else
			{
				if (type == 2)
				{
					return;
				}
				if (type == 100)
				{
					Char.Arr_Head_2Fr = readArrHead(d);
					if (isSave)
					{
						d.reset();
						sbyte[] data4 = new sbyte[d.available()];
						d.readFully(ref data4);
						Rms.saveRMS("NRitem100", data4);
					}
				}
				else
				{
					if (type != 101)
					{
						return;
					}
					try
					{
						int num2 = d.readShort();
						Char.Arr_Head_FlyMove = new short[num2];
						for (int k = 0; k < num2; k++)
						{
							short num3 = d.readShort();
							Char.Arr_Head_FlyMove[k] = num3;
						}
						if (isSave)
						{
							d.reset();
							sbyte[] data5 = new sbyte[d.available()];
							d.readFully(ref data5);
							Rms.saveRMS("NRitem101", data5);
						}
						return;
					}
					catch (Exception)
					{
						Char.Arr_Head_FlyMove = new short[0];
						return;
					}
				}
			}
		}
		catch (Exception ex2)
		{
			ex2.ToString();
		}
	}

	private void readFrameBoss(Message msg, int mobTemplateId)
	{
		try
		{
			int num = msg.reader().readByte();
			int[][] array = new int[num][];
			for (int i = 0; i < num; i++)
			{
				int num2 = msg.reader().readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = msg.reader().readByte();
				}
			}
			frameHT_NEWBOSS.put(mobTemplateId + string.Empty, array);
		}
		catch (Exception)
		{
		}
	}

	private int[][] readArrHead(myReader d)
	{
		int[][] array = new int[1][] { new int[2] { 542, 543 } };
		try
		{
			int num = d.readShort();
			array = new int[num][];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = d.readByte();
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = d.readShort();
				}
			}
		}
		catch (Exception)
		{
		}
		return array;
	}

	public void phuban_Info(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				readPhuBan_CHIENTRUONGNAMEK(msg, b);
			}
		}
		catch (Exception)
		{
		}
	}

	private void readPhuBan_CHIENTRUONGNAMEK(Message msg, int type_PB)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				short idmapPaint = msg.reader().readShort();
				string nameTeam = msg.reader().readUTF();
				string nameTeam2 = msg.reader().readUTF();
				int maxPoint = msg.reader().readInt();
				short timeSecond = msg.reader().readShort();
				int maxLife = msg.reader().readByte();
				GameScr.phuban_Info = new InfoPhuBan(type_PB, idmapPaint, nameTeam, nameTeam2, maxPoint, timeSecond);
				GameScr.phuban_Info.maxLife = maxLife;
				GameScr.phuban_Info.updateLife(type_PB, 0, 0);
			}
			else if (b == 1)
			{
				int pointTeam = msg.reader().readInt();
				int pointTeam2 = msg.reader().readInt();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
				}
			}
			else if (b == 2)
			{
				sbyte b2 = msg.reader().readByte();
				short type = 0;
				short num = -1;
				if (b2 == 1)
				{
					type = 1;
					num = 3;
				}
				else if (b2 == 2)
				{
					type = 2;
				}
				num = -1;
				GameScr.phuban_Info = null;
				GameScr.addEffectEnd(type, num, 0, GameCanvas.hw, GameCanvas.hh, 0, 0, -1, null);
			}
			else if (b == 5)
			{
				short timeSecond2 = msg.reader().readShort();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
				}
			}
			else if (b == 4)
			{
				int lifeTeam = msg.reader().readByte();
				int lifeTeam2 = msg.reader().readByte();
				if (GameScr.phuban_Info != null)
				{
					GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void read_cmdExtra(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			mSystem.println(">>---read_cmdExtra-sub:" + b);
			if (b == 0)
			{
				short idHat = msg.reader().readShort();
				Char.myCharz().idHat = idHat;
				SoundMn.gI().getStrOption();
			}
			else if (b == 2)
			{
				int num = msg.reader().readInt();
				sbyte b2 = msg.reader().readByte();
				short num2 = msg.reader().readShort();
				string v = num2 + "," + b2;
				MainImage imagePath = ImgByName.getImagePath("banner_" + num2, ImgByName.hashImagePath);
				GameCanvas.danhHieu.put(num + string.Empty, v);
			}
			else if (b == 3)
			{
				short num3 = msg.reader().readShort();
				SmallImage.createImage(num3);
				BackgroudEffect.id_water1 = num3;
			}
			else if (b == 4)
			{
				string o = msg.reader().readUTF();
				GameCanvas.messageServer.addElement(o);
			}
			else if (b == 5)
			{
				string text = "------------------|ChienTruong|Log: ";
				text = "\n|ChienTruong|Log: ";
				sbyte b3 = msg.reader().readByte();
				if (b3 == 0)
				{
					GameScr.nCT_team = msg.reader().readUTF();
					GameScr.nCT_TeamA = (GameScr.nCT_TeamB = msg.reader().readByte());
					GameScr.nCT_nBoyBaller = GameScr.nCT_TeamA * 2;
					GameScr.isPaint_CT = false;
					string text2 = text;
					text = text2 + "\tsub    0|  nCT_team= " + GameScr.nCT_team + "|nCT_TeamA =" + GameScr.nCT_TeamA + "  isPaint_CT=false \n";
				}
				else if (b3 == 1)
				{
					int num4 = msg.reader().readInt();
					sbyte b4 = (GameScr.nCT_floor = msg.reader().readByte());
					GameScr.nCT_timeBallte = num4 * 1000 + mSystem.currentTimeMillis();
					GameScr.isPaint_CT = true;
					string text2 = text;
					text = text2 + "\tsub    1 floor= " + b4 + "|timeBallte= " + num4 + "isPaint_CT=true \n";
				}
				else if (b3 == 2)
				{
					GameScr.nCT_TeamA = msg.reader().readByte();
					GameScr.nCT_TeamB = msg.reader().readByte();
					GameScr.res_CT.removeAllElements();
					sbyte b5 = msg.reader().readByte();
					for (int i = 0; i < b5; i++)
					{
						string empty = string.Empty;
						empty = empty + msg.reader().readByte() + "|";
						empty = empty + msg.reader().readUTF() + "|";
						empty = empty + msg.reader().readShort() + "|";
						empty += msg.reader().readInt();
						GameScr.res_CT.addElement(empty);
					}
					string text2 = text;
					text = text2 + "\tsub   2|  A= " + GameScr.nCT_TeamA + "|B =" + GameScr.nCT_TeamB + "  isPaint_CT=true \n";
				}
				else if (b3 == 3)
				{
					Service.gI().sendCT_ready(b, b3);
					GameScr.nCT_floor = 0;
					GameScr.nCT_timeBallte = 0L;
					GameScr.isPaint_CT = false;
					text += "\tsub    3|  isPaint_CT=false \n";
				}
				else if (b3 == 4)
				{
					GameScr.nUSER_CT = msg.reader().readByte();
					GameScr.nUSER_MAX_CT = msg.reader().readByte();
				}
				text += "END LOG CT.";
				Res.err(text);
			}
			else
			{
				readExtra(b, msg);
			}
		}
		catch (Exception)
		{
		}
	}

	public void read_UpdateSkill(Message msg)
	{
		try
		{
			short num = msg.reader().readShort();
			sbyte b = -1;
			try
			{
				b = msg.reader().readSByte();
			}
			catch (Exception)
			{
			}
			if (b == 0)
			{
				short curExp = msg.reader().readShort();
				for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
				{
					Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
					if (skill.skillId == num)
					{
						skill.curExp = curExp;
						break;
					}
				}
			}
			else if (b == 1)
			{
				sbyte b2 = msg.reader().readByte();
				for (int j = 0; j < Char.myCharz().vSkill.size(); j++)
				{
					Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(j);
					if (skill2.skillId == num)
					{
						for (int k = 0; k < 20; k++)
						{
							string nameImg = "Skills_" + skill2.template.id + "_" + b2 + "_" + k;
							MainImage imagePath = ImgByName.getImagePath(nameImg, ImgByName.hashImagePath);
						}
						break;
					}
				}
			}
			else
			{
				if (b != -1)
				{
					return;
				}
				Skill skill3 = Skills.get(num);
				for (int l = 0; l < Char.myCharz().vSkill.size(); l++)
				{
					Skill skill4 = (Skill)Char.myCharz().vSkill.elementAt(l);
					if (skill4.template.id == skill3.template.id)
					{
						Char.myCharz().vSkill.setElementAt(skill3, l);
						break;
					}
				}
				for (int m = 0; m < Char.myCharz().vSkillFight.size(); m++)
				{
					Skill skill5 = (Skill)Char.myCharz().vSkillFight.elementAt(m);
					if (skill5.template.id == skill3.template.id)
					{
						Char.myCharz().vSkillFight.setElementAt(skill3, m);
						break;
					}
				}
				for (int n = 0; n < GameScr.onScreenSkill.Length; n++)
				{
					if (GameScr.onScreenSkill[n] != null && GameScr.onScreenSkill[n].template.id == skill3.template.id)
					{
						GameScr.onScreenSkill[n] = skill3;
						break;
					}
				}
				for (int num2 = 0; num2 < GameScr.keySkill.Length; num2++)
				{
					if (GameScr.keySkill[num2] != null && GameScr.keySkill[num2].template.id == skill3.template.id)
					{
						GameScr.keySkill[num2] = skill3;
						break;
					}
				}
				if (Char.myCharz().myskill.template.id == skill3.template.id)
				{
					Char.myCharz().myskill = skill3;
				}
				GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill3.template.name + mResources.hasJustUpgrade2 + skill3.point, 0);
			}
		}
		catch (Exception)
		{
		}
	}

	public void readExtra(sbyte sub, Message msg)
	{
		try
		{
			if (sub != sbyte.MaxValue)
			{
				return;
			}
			GameCanvas.endDlg();
			try
			{
				string text = (ServerListScreen.linkDefault = msg.reader().readUTF());
				mSystem.AddIpTest();
				ServerListScreen.getServerList(ServerListScreen.linkDefault);
				Res.outz(">>>>read.isEXTRA_LINK " + text);
				sbyte b = msg.reader().readByte();
				if (b > 0)
				{
					ServerListScreen.typeClass = new sbyte[b];
					ServerListScreen.listChar = new Char[b];
					for (int i = 0; i < b; i++)
					{
						ServerListScreen.typeClass[i] = msg.reader().readByte();
						Res.outz(ServerListScreen.nameServer[i] + ">>>>read.isEXTRA_LINK  typeClass: " + ServerListScreen.typeClass[i]);
						if (ServerListScreen.typeClass[i] > -1)
						{
							ServerListScreen.isHaveChar = true;
							ServerListScreen.listChar[i] = new Char();
							ServerListScreen.listChar[i].cgender = ServerListScreen.typeClass[i];
							ServerListScreen.listChar[i].head = msg.reader().readShort();
							ServerListScreen.listChar[i].body = msg.reader().readShort();
							ServerListScreen.listChar[i].leg = msg.reader().readShort();
							ServerListScreen.listChar[i].bag = msg.reader().readShort();
							ServerListScreen.listChar[i].cName = msg.reader().readUTF();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			isEXTRA_LINK = true;
			ServerListScreen.saveRMS_ExtraLink();
			ServerListScreen.isWait = false;
			Char.isLoadingMap = false;
			LoginScr.isContinueToLogin = false;
			ServerListScreen.waitToLogin = false;
			bool flag = false;
			bool flag2 = false;
			try
			{
				if (!Rms.loadRMSString("acc").Equals(string.Empty))
				{
					flag = true;
				}
				if (!Rms.loadRMSString("userAo" + ServerListScreen.ipSelect).Equals(string.Empty))
				{
					flag2 = true;
				}
			}
			catch (Exception)
			{
			}
			if (!ServerListScreen.isHaveChar && !flag && !flag2)
			{
				GameCanvas.serverScreen.Login_New();
				return;
			}
			if (Rms.loadRMSInt(ServerListScreen.RMS_svselect) == -1)
			{
				ServerScr.isShowSv_HaveChar = false;
				GameCanvas.serverScr.switchToMe();
				return;
			}
			ServerListScreen.SetIpSelect(Rms.loadRMSInt(ServerListScreen.RMS_svselect), false);
			if (ServerListScreen.listChar != null && ServerListScreen.listChar[ServerListScreen.ipSelect] != null)
			{
				GameCanvas._SelectCharScr.SetInfoChar(ServerListScreen.listChar[ServerListScreen.ipSelect]);
			}
			else
			{
				GameCanvas.serverScreen.Login_New();
			}
		}
		catch (Exception)
		{
			Res.outz(">>>>read.isEXTRA_LINK  errr:");
			GameCanvas.serverScr.switchToMe();
		}
	}

	public ItemOption readItemOption(Message msg)
	{
		ItemOption result = null;
		try
		{
			int num = msg.reader().readShort();
			int param = msg.reader().readInt();
			if (num != -1)
			{
				result = new ItemOption(num, param);
			}
		}
		catch (Exception)
		{
			Res.err(">>>>read.ItemOption  errr:");
		}
		return result;
	}

	public void read_cmdExtraBig(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			mSystem.println(">>---read_cmdExtraBig-sub:" + b);
			if (b == 0)
			{
				loadItemNew(msg.reader(), 1, true);
			}
		}
		catch (Exception)
		{
		}
	}
}
