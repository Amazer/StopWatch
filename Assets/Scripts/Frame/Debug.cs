/*
********************************************** 
 *Copyright(C) 2018 by 延澈左 
 *
 *模块名:           Debug.cs 
 *创建者:           延澈左 
 *创建日期:         2019-07-22 
 *修改者列表:
 *描述:    
**********************************************
*/
public static class Debug
{
    public static void Log(object message, params object[] argv)
    {
        UnityEngine.Debug.Log(LogStringFormat(message, argv));
    }
    public static void Error(object message, params object[] argv)
    {
        UnityEngine.Debug.LogError(LogStringFormat(message, argv));

    }
    public static void Warning(object message, params object[] argv)
    {
        UnityEngine.Debug.LogWarning(LogStringFormat(message, argv));
    }

    private static string LogStringFormat(object message, params object[] argv)
    {
        string logMsg;
        if (argv == null || argv.Length == 0)
        {
            logMsg = message.ToString();
        }
        else
        {
            logMsg = string.Format(message.ToString(), argv);
        }
        return logMsg;
    }
}
