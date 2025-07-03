using System;
using System.Threading;
using UnityEngine;

public class SMS
{
	private const int INTERVAL = 5;

	private const int MAXTIME = 500;

	private static int status;

	private static int _result;

	private static string _to;

	private static string _content;

	private static bool f;

	private static int time;

	public static bool sendEnable;

	private static int time0;

	public static int send(string content, string to)
	{
		if (Thread.CurrentThread.Name == Main.mainThreadName)
		{
			return __send(content, to);
		}
		return _send(content, to);
	}

	private static int _send(string content, string to)
	{
		if (status != 0)
		{
			for (int i = 0; i < 500; i++)
			{
				Thread.Sleep(5);
				if (status == 0)
				{
					break;
				}
			}
			if (status != 0)
			{
				Cout.LogError("CANNOT SEND SMS " + content + " WHEN SENDING " + _content);
				return -1;
			}
		}
		_content = content;
		_to = to;
		_result = -1;
		status = 2;
		int j;
		for (j = 0; j < 500; j++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (j == 500)
		{
			Debug.LogError("TOO LONG FOR SEND SMS " + content);
			status = 0;
		}
		else
		{
			Debug.Log("Send SMS " + content + " done in " + j * 5 + "ms");
		}
		return _result;
	}

	private static int __send(string content, string to)
	{
		return 0;
	}

	public static void update()
	{
		float num = Time.time;
		if (num - (float)time > 1f)
		{
			time++;
		}
		if (f)
		{
			OnSMS();
		}
		if (status == 2)
		{
			status = 1;
			try
			{
				_result = __send(_content, _to);
			}
			catch (Exception)
			{
				Debug.Log("CANNOT SEND SMS");
			}
			status = 0;
		}
	}

	private static void OnSMS()
	{
		
	}
}
