using System;

using Microsoft.VisualBasic;

using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.TextLogger
{
    #region " Error Logger "
    [CLSCompliant(true)]
    public class ErrorLogger_Cls
    {
        private static System.IO.StreamWriter Lfs_ErrorLog = null;
        private static System.Threading.Mutex Lmutex_LogFileWriter = new System.Threading.Mutex();

        public ErrorLogger_Cls()
        {
            Lfs_ErrorLog = null;
        }

        public static void Write(string str_ErrFunctionName, Exception obj_Exception)
        {
            try
            {
                TextLogger_Cls obj_TextLogger = new TextLogger_Cls
                {
                    Lboo_AddDateBeforeFileName = true,
                    //////////Lstr_LogDir = System.Windows.Forms.Application.ExecutablePath //removed by chang on 20211207: just hard coded to default directory -> "C:\AutoCountPlugin\<PluginName>\
                    //////////Lstr_LogDir = AutoCountPlugin() + @"\" + typeof(PlugIn_Cls).Assembly.ManifestModule.Name //added by chang on 20211214: hard coded to default directory -> "C:\AutoCountPlugin\<DBName>\<PluginName>\ //removed by chang on 20220705
                    Lstr_LogDir = AutoCountPlugin() + @"\" + System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name //added by chang on 20220705: instead of using own assembly here, suppose to use the calling entry assembly to get the name
                };
                obj_TextLogger.Lstr_LogFileName = TextParser.TextParser_Cls.GetToken(ref obj_TextLogger.Lstr_LogDir, @":\", true, false, TextParser.TextParser_Cls.ParserDirection_Enum.RightToLeft) + ".Err";

                obj_TextLogger.AddField("Date/Time", 20);
                obj_TextLogger.AddField("Function Name", 50);
                obj_TextLogger.AddField("Message", 60);
                obj_TextLogger.AddField("Source", 40);
                obj_TextLogger.AddField("Type", 40);
                obj_TextLogger.AddField("Stack Trace", 256);

                Exception obj_Ex = obj_Exception;
                while (obj_Ex != null)
                {
                    obj_TextLogger.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str_ErrFunctionName, obj_Exception.Message, obj_Exception.Source, obj_Exception.GetType().ToString(), obj_Exception.StackTrace);

                    obj_Ex = obj_Ex.InnerException;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    #endregion " Error Logger "

    #region " General Logger " //added by SCChang's Copilot on 20251110
    
    /// <summary>
    /// Log levels for general logging
    /// </summary>
    /// <remarks>
    /// Added by SCChang's Copilot on 20251110: v2.2.1.0
    /// </remarks>
    public enum LogLevel
    {
        INFO,
        SUCCESS,
        WARNING,
        ERROR,
        DEBUG
    }

    /// <summary>
    /// General-purpose logger for plugin debugging and operation tracking
    /// Static class similar to ErrorLogger_Cls for easy access throughout the application
    /// </summary>
    /// <remarks>
    /// Added by SCChang's Copilot on 20251110: v2.2.1.0
    /// - File naming: GeneralLog_yyyyMMdd.log
    /// - Folder: Hard-coded separate location (C:\AutoCountPlugin\{DBName}\Logs\GeneralLogs\)
    /// - Timestamp format: yyyy-MM-dd HH:mm:ss.ffff (includes milliseconds)
    /// - Auto cleanup: Configurable retention period (default 90 days)
    /// - Thread-safe: Uses lock for concurrent access
    /// - Static methods: Can be used anywhere like ErrorLogger_Cls.Write()
    /// </remarks>
    [CLSCompliant(true)]
    public static class GeneralLogger_Cls
    {
        private static DateTime _lastCleanupCheck = DateTime.MinValue;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Log retention period in days (default: 90 days)
        /// User can modify this value to change retention policy
        /// </summary>
        public static int RetentionDays { get; set; } = 90;

        /// <summary>
        /// Enable/disable automatic log cleanup (default: true)
        /// </summary>
        public static bool EnableAutoCleanup { get; set; } = true;

        /// <summary>
        /// Hard-coded log folder location (separate from ErrorLogger)
        /// Returns: C:\AutoCountPlugin\{DBName}\Logs\GeneralLogs\
        /// </summary>
        /// <remarks>
        /// Edited by SCChang's Copilot on 20251110: Use global variable like ErrorLogger
        /// </remarks>
        private static string GetLogFolder()
        {
            try
            {
                // Use AutoCountPlugin() global function like ErrorLogger does
                // This returns C:\AutoCountPlugin\{DBName}\
                string baseFolder = AutoCountPlugin();
                return System.IO.Path.Combine(baseFolder, "Logs", "GeneralLogs");
            }
            catch
            {
                // Fallback to default path if AutoCountPlugin() fails
                return @"C:\AutoCountPlugin\Logs\GeneralLogs";
            }
        }

        /// <summary>
        /// Write log entry with specified message and log level
        /// </summary>
        /// <param name="message">Log message to write</param>
        /// <param name="logLevel">Log level (INFO, SUCCESS, WARNING, ERROR, DEBUG)</param>
        /// <remarks>
        /// Usage example: GeneralLogger_Cls.Write("Operation completed", LogLevel.SUCCESS);
        /// </remarks>
        public static void Write(string message, LogLevel logLevel = LogLevel.INFO)
        {
            try
            {
                // Generate log file name with current date
                string dateStamp = DateTime.Now.ToString("yyyyMMdd");
                string logFileName = $"GeneralLog_{dateStamp}.log";
                string logFolder = GetLogFolder();
                string logFilePath = System.IO.Path.Combine(logFolder, logFileName);

                // Create folder if not exists
                if (!System.IO.Directory.Exists(logFolder))
                {
                    System.IO.Directory.CreateDirectory(logFolder);
                }

                // Format log entry with timestamp (including milliseconds to ffff) and log level
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                string logEntry = $"[{timestamp}] [{logLevel}] {message}";

                // Thread-safe file writing
                lock (_lockObject)
                {
                    System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }

                // Perform automatic cleanup if enabled
                if (EnableAutoCleanup)
                {
                    CleanupOldLogs();
                }
            }
            catch (Exception ex)
            {
                // Fallback to ErrorLogger if general logging fails
                try
                {
                    ErrorLogger_Cls.Write($"{nameof(GeneralLogger_Cls)}.{nameof(Write)}()", ex);
                }
                catch
                {
                    // Silent fail if both loggers are unavailable
                    System.Diagnostics.Debug.WriteLine($"[CRITICAL] Failed to write general log: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Cleanup old log files based on retention policy
        /// Only checks once per day to avoid overhead
        /// </summary>
        private static void CleanupOldLogs()
        {
            try
            {
                // Only check once per day to avoid overhead
                if ((DateTime.Now - _lastCleanupCheck).TotalHours < 24)
                {
                    return;
                }

                _lastCleanupCheck = DateTime.Now;

                string logFolder = GetLogFolder();
                if (!System.IO.Directory.Exists(logFolder))
                {
                    return;
                }

                // Calculate cutoff date
                DateTime cutoffDate = DateTime.Now.AddDays(-RetentionDays);

                // Get all log files matching pattern GeneralLog_*.log
                string[] logFiles = System.IO.Directory.GetFiles(logFolder, "GeneralLog_*.log");

                foreach (string logFile in logFiles)
                {
                    try
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFile);
                        
                        // Delete if older than retention period
                        if (fileInfo.LastWriteTime < cutoffDate)
                        {
                            fileInfo.Delete();
                            Write($"Deleted old log file: {fileInfo.Name}", LogLevel.INFO);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log cleanup failure but continue with other files
                        Write($"Failed to delete old log file {System.IO.Path.GetFileName(logFile)}: {ex.Message}", LogLevel.WARNING);
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail for cleanup errors
                System.Diagnostics.Debug.WriteLine($"[WARNING] Log cleanup failed: {ex.Message}");
            }
        }
    }
    #endregion " General Logger " //added by SCChang's Copilot on 20251110

    #region " General Text Logger "
    public class TextLogger_Cls
    {
        private static System.IO.StreamWriter Lfs_LogFile;
        private static System.Threading.Mutex Lmutex_LogFileWriter = new System.Threading.Mutex();

        private int Lint_FieldCount = 0;
        private int[] Lint_FieldLen;
        private string[] Lstr_FieldName;
        private string Lstr_Text = System.Windows.Forms.Application.ExecutablePath; //added by chang on 20200603: c# can't accepts ref param with readonly attribute

        public bool Lboo_AddDateBeforeFileName = true;
        public string Lstr_LogFileName;
        public string Lstr_LogDir;

        public TextLogger_Cls()
        {
            Lfs_LogFile = null;
            Lstr_LogDir = @".\";
            Lstr_LogFileName = TextParser.TextParser_Cls.GetToken(ref Lstr_Text, str_Separators: @":\", enm_Direction: TextParser.TextParser_Cls.ParserDirection_Enum.RightToLeft) + ".TxtLog";
        }
        public bool AddField(string str_Name, int int_Len)
        {
            if (int_Len > 0)
            {
                var oldLstr_FieldName = Lstr_FieldName;
                Lstr_FieldName = new string[Lint_FieldCount + 1];
                if (oldLstr_FieldName != null)
                    Array.Copy(oldLstr_FieldName, Lstr_FieldName, Math.Min(Lint_FieldCount + 1, oldLstr_FieldName.Length));
                var oldLint_FieldLen = Lint_FieldLen;
                Lint_FieldLen = new int[Lint_FieldCount + 1];
                if (oldLint_FieldLen != null)
                    Array.Copy(oldLint_FieldLen, Lint_FieldLen, Math.Min(Lint_FieldCount + 1, oldLint_FieldLen.Length));

                Lstr_FieldName[Lint_FieldCount] = str_Name;
                Lint_FieldLen[Lint_FieldCount] = int_Len;

                Lint_FieldCount += 1;

                return true;
            }

            return false;
        }
        public void Write(params object[] str_Fields)
        {
            int int_Index;
            string str_Text;
            try
            {
                // Block other from writting simultaneously
                Lmutex_LogFileWriter.WaitOne();
            }
            catch (Exception e)
            {
            }

            try
            {
                if (Lstr_LogFileName == "")
                    return;

                // Log into local error text file
                if (Lfs_LogFile == null)
                {
                    if (Strings.Right(Lstr_LogDir, 1) != @"\") Lstr_LogDir += @"\";
                    if (!System.IO.Directory.Exists(Lstr_LogDir)) System.IO.Directory.CreateDirectory(Lstr_LogDir);

                    string str_FileName = Lstr_LogFileName;
                    if (Lboo_AddDateBeforeFileName) str_FileName = "(" + DateTime.Now.ToString("yyyy-MM-dd") + ") " + str_FileName;
                    str_FileName = Lstr_LogDir + str_FileName;
                    bool boo_WriteHeader = !System.IO.File.Exists(str_FileName);

                    Lfs_LogFile = System.IO.File.AppendText(str_FileName);
                    if (boo_WriteHeader)
                    {
                        // Write file header
                        string str_HeaderText = "";
                        string str_HeaderBottomLine = "";

                        for (int_Index = 0; int_Index <= Lint_FieldCount - 1; int_Index++)
                        {
                            str_Text = Lstr_FieldName[int_Index];
                            str_HeaderText += StringToFixLengthField(ref str_Text, Lint_FieldLen[int_Index]) + " ";
                            str_Text = Strings.Replace(Strings.Space(Lint_FieldLen[int_Index]), " ", "=");
                            str_HeaderBottomLine += StringToFixLengthField(ref str_Text, Lint_FieldLen[int_Index]) + " ";
                        }
                        WriteTextFile(ref Lfs_LogFile, str_HeaderText);
                        WriteTextFile(ref Lfs_LogFile, str_HeaderBottomLine);
                    }
                }
                if (Lfs_LogFile != null)
                {
                    // Write into a reader friendly format of text log file
                    string str_Temp;
                    string str_Temp1; //added by chang on 20200719
                    bool boo_Completed = false;

                    while (!boo_Completed)
                    {
                        // Format the log strings
                        str_Text = "";
                        boo_Completed = true;
                        for (int_Index = 0; int_Index <= Lint_FieldCount - 1; int_Index++)
                        {
                            str_Temp = "";
                            str_Temp1 = ""; //added by chang on 20200719
                            if (int_Index < str_Fields.Length)
                            {
                                //////////if (str_Fields[int_Index].ToString() != "") //removed by chang on 20211207: need to check the str_Fields[] value of null
                                if (str_Fields[int_Index] != null && str_Fields[int_Index].ToString() != "") //added by chang on 20211207: need to check the str_Fields[] value of null
                                {
                                    boo_Completed = false;
                                    str_Temp1 = str_Fields[int_Index].ToString(); //added by chang on 20200603: to fit with c# pattern
                                                                                  //////////str_Temp = StringToFixLengthField(ref str_Fields[int_Index], Lint_FieldLen[int_Index]);
                                }

                                str_Temp = StringToFixLengthField(ref str_Temp1, Lint_FieldLen[int_Index]);
                                str_Fields[int_Index] = str_Temp1; //added by chang on 20200719: need to assign back to this array element else cannot out of loop
                            }
                            str_Text += str_Temp + " ";
                        }

                        // Write the formated string
                        if (Strings.Trim(str_Text) != "") WriteTextFile(ref Lfs_LogFile, str_Text);
                    }

                    // Make a diveder
                    str_Text = "";
                    for (int_Index = 0; int_Index <= Lint_FieldCount - 1; int_Index++)
                    {
                        //////////str_Text += StringToFixLengthField(ref Strings.Replace(Strings.Space(Lint_FieldLen[int_Index]), " ", "-"), Lint_FieldLen[int_Index]) + " "; //removed by chang on 20200603
                        str_Temp1 = Strings.Replace(Strings.Space(Lint_FieldLen[int_Index]), " ", "-"); //added by chang on 20200603: to fit with c# pattern
                        str_Text += StringToFixLengthField(ref str_Temp1, Lint_FieldLen[int_Index]) + " "; //added by chang on 20200603: to fit with c# pattern
                    }
                    WriteTextFile(ref Lfs_LogFile, str_Text);

                    // Make a blank line
                    WriteTextFile(ref Lfs_LogFile, "");
                    Lfs_LogFile.Flush();
                }
            }
            catch (Exception e)
            {
                try
                {
                    // Close File
                    //////////Lfs_LogFile.Close(); //removed by chang on 20211207: need to check null value
                    if (Lfs_LogFile != null) Lfs_LogFile.Close(); //added by chang on 20211207: need to check null value
                }
                catch (Exception e2)
                {
                }
                Lfs_LogFile = null;
            }
            finally
            {
                try
                {
                    // Close File
                    //////////Lfs_LogFile.Close(); //removed by chang on 20211207: need to check null value
                    if (Lfs_LogFile != null) Lfs_LogFile.Close(); //added by chang on 20211207: need to check null value
                }
                catch (Exception e2)
                {
                }
                Lfs_LogFile = null;
                try
                {
                    // Release the block
                    Lmutex_LogFileWriter.ReleaseMutex();
                }
                catch (Exception e)
                {
                }
            }
        }
        private void WriteTextFile(ref System.IO.StreamWriter fs_FileStream, string str_Text)
        {
            try
            {
                if (str_Text.Length == 0)
                    return;
                if (fs_FileStream == null)
                    return;

                fs_FileStream.WriteLine(str_Text);
            }
            catch (Exception ex)
            {
            }
        }
        private string StringToFixLengthField(ref string str_Input, int int_Len)
        {
            string str_Return = "";
            string str_Partial = "";
            int int_Space = -1;

            if (int_Len < 0)
            {
                str_Input = "";
                return "";
            }
            else if (int_Len == 0)
            {
                str_Return = str_Input;
                str_Input = "";

                return str_Return;
            }

            // Remove CR/LF
            str_Input = str_Input.Replace(Strings.Chr(13).ToString(), Strings.Space(int_Len));
            str_Input = str_Input.Replace(Strings.Chr(10).ToString(), Strings.Space(int_Len));

            while (str_Input != "" & str_Return.Length < int_Len)
            {
                // Remove left space
                str_Input = str_Input.TrimStart(char.Parse(" "));
                // Get 1st occurance of space
                int_Space = str_Input.IndexOf(" ");
                if (int_Space > -1)
                    str_Partial = Strings.Left(str_Input, int_Space + 1);
                else
                    str_Partial = str_Input;

                if (str_Return.Length + str_Partial.Length > int_Len)
                    break;
                if (int_Space > -1)
                    str_Input = Strings.Mid(str_Input, int_Space + 1);
                else
                    str_Input = "";
                str_Return += str_Partial;
            }

            if (str_Return == "")
            {
                str_Return = Strings.Left(str_Input, int_Len);
                str_Input = Strings.Mid(str_Input, int_Len + 1);
            }

            if (str_Return.Length < int_Len)
                str_Return += Strings.Space(int_Len - str_Return.Length);

            return str_Return;
        }
    }
    #endregion " General Text Logger "
}

namespace VTACPluginBase.Classes.TextParser
{
    #region " Text Parser "
    public class TextParser_Cls
    {
        public enum ParserDirection_Enum
        {
            LeftToRight = 0,
            RightToLeft = 1
        }

        public static string GetToken(ref string str_Text, string str_Separators = @" ,!@#$%^&*()-+=\/{}[]|:;""'<>?", bool boo_RemoveFromText = true, bool boo_ReturnSeparator = false, ParserDirection_Enum enm_Direction = ParserDirection_Enum.LeftToRight)
        {
            string str_Token = "";
            string str_Char;
            int int_Position = default(int);
            bool boo_Exit = false;

            switch (enm_Direction)
            {
                case ParserDirection_Enum.LeftToRight:
                    {
                        int_Position = 1;
                        break;
                    }

                case ParserDirection_Enum.RightToLeft:
                    {
                        int_Position = str_Text.Length;
                        break;
                    }
            }

            while (str_Text != "" & (int_Position >= 1 & int_Position <= str_Text.Length))
            {
                str_Char = Strings.Mid(str_Text, int_Position, 1);
                if (boo_RemoveFromText)
                {
                    switch (enm_Direction)
                    {
                        case ParserDirection_Enum.LeftToRight:
                            {
                                str_Text = Strings.Mid(str_Text, 2);
                                break;
                            }

                        case ParserDirection_Enum.RightToLeft:
                            {
                                if (str_Text.Length > 0)
                                    str_Text = Strings.Mid(str_Text, 1, str_Text.Length - 1);
                                int_Position = str_Text.Length;
                                break;
                            }
                    }
                }
                else
                    switch (enm_Direction)
                    {
                        case ParserDirection_Enum.LeftToRight:
                            {
                                int_Position += 1;
                                break;
                            }

                        case ParserDirection_Enum.RightToLeft:
                            {
                                int_Position -= 1;
                                break;
                            }
                    }

                if (Strings.InStr(str_Separators, str_Char, CompareMethod.Text) > 0)
                {
                    int_Position = -1;

                    if (!boo_ReturnSeparator)
                        str_Char = "";

                    boo_Exit = true;
                }

                switch (enm_Direction)
                {
                    case ParserDirection_Enum.LeftToRight:
                        {
                            str_Token += str_Char;
                            break;
                        }

                    case ParserDirection_Enum.RightToLeft:
                        {
                            str_Token = str_Char + str_Token;
                            break;
                        }
                }

                if (boo_Exit)
                    break;
            }

            return str_Token;
        }
        public static string RemoveDoubleSpaces(string str_Origin)
        {
            try
            {
                while (Strings.InStr(str_Origin, "  ") > 0)
                    str_Origin = Strings.Replace(str_Origin, "  ", " ");

                return str_Origin;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
    #endregion " Text Parser "
}
