using System;
using System.IO;

namespace Unmoddable;

// TODO: optimize logging on all stages with StringBuilders.
// TODO: Expose low-level non-extension methods for external things to provide StringBuilder as well.
//  In short - reduce reallocations on all levels.
public static class LoggingHelpers
{
    const string LogFilePath = "unmoddable.log";
    public static void Clear()
    {
        try { File.Delete(LogFilePath); }
        catch { }
    }

    public static string ToDisplay(this Exception? ex)
    {
        if (ex is null) return "null";
        else if (ex.InnerException is null)
            return $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
        else
            return $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n{ex.InnerException.ToDisplay()}";
    }

    public static void Out(this object? obj, object? prefix = null)
    {
        string content;
        if (prefix is not null)
        {
            string p = prefix.ToString();
            if (p.EndsWith(']'))
                content = $"{prefix} {obj}\r\n";
            else
                content = $"{prefix}\r\n {obj}\r\n";
        }
        else content = $"{obj}\r\n";
        File.AppendAllText(LogFilePath, content);
    }

    public static void Out(this Exception? ex, object? prefix = null)
    {
        string content;
        if (prefix is not null)
        {
            string p = prefix.ToString();
            if (p.EndsWith(']'))
                content = $"{prefix} {ex.ToDisplay()}\r\n";
            else
                content = $"{prefix}\r\n {ex.ToDisplay()}\r\n";
        }
        else content = $"{ex.ToDisplay()}\r\n";
        File.AppendAllText(LogFilePath, content);
    }
}
