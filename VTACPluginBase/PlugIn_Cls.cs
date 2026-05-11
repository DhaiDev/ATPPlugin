using AutoCount.Data;

namespace VTACPluginBase
{
    public static class PlugIn_Cls
    {
        // edited by SCChang's Copilot on 20251126: changed from field to property to support Design-Time
        // Original code (caused TypeInitializationException in Visual Studio Designer):
        // public static DBSetting myDBSetting = AutoCount.Authentication.UserSession.CurrentUserSession.DBSetting;
        // 
        // Problem: Static field initializer executes when class is loaded, but CurrentUserSession is null in Designer.
        // Solution: Changed to property with Design-Time check and null-safe operator.
        public static DBSetting myDBSetting 
        {
            get 
            {
                // If in Design Time (e.g., Visual Studio Designer), return null to avoid exception
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    return null;
                    
                // Runtime: get actual DBSetting (with null-safe operator to prevent NullReferenceException)
                return AutoCount.Authentication.UserSession.CurrentUserSession?.DBSetting;
            }
        }

        public static string AutoCountPlugin()
        {
            string str_rtn = @"C:\AutoCountPlugin";
            //string test = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            //////////if (myDBSetting != null) str_rtn += @"\" + myDBSetting.DBName + @"\" + typeof(PlugIn_Cls).Assembly.GetName().Name; //removed by chang on 20220705
            if (myDBSetting != null) str_rtn += @"\" + myDBSetting.DBName + @"\" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name; //added by chang on 20220705: instead of using own assembly here, suppose to use the calling entry assembly to get the name

            return str_rtn;
        }

        public static string AutoCountProfile()
        {
            // edited by SCChang's Copilot on 20251126: added Design-Time check and null-safe operator
            // Original code (caused exception in Visual Studio Designer):
            // return AutoCountPlugin() + $@"\{AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID}" + @"\Profile";
            
            // If in Design Time or CurrentUserSession is null, return default path
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime ||
                AutoCount.Authentication.UserSession.CurrentUserSession == null)
            {
                return AutoCountPlugin() + @"\DefaultUser\Profile";
            }
            
            return AutoCountPlugin() + $@"\{AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID}" + @"\Profile";
        }
    }
}
