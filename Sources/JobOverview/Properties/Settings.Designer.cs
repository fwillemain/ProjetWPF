﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobOverview.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=FWILLEMAIN17-DE\\IP08R2;Initial Catalog=JobOverview;Integrated Securit" +
            "y=True")]
        public string JobOverviewConnectionStringDefault {
            get {
                return ((string)(this["JobOverviewConnectionStringDefault"]));
            }
            set {
                this["JobOverviewConnectionStringDefault"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=FWILLEMAIN17-DE\\IP08R2;Initial Catalog=JobOverview;Integrated Securit" +
            "y=True")]
        public string JobOverviewConnectionStringFlo {
            get {
                return ((string)(this["JobOverviewConnectionStringFlo"]));
            }
            set {
                this["JobOverviewConnectionStringFlo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=GLAVENDER17-DE\\IP08R2;Initial Catalog=\"job overview\";Integrated Secur" +
            "ity=True")]
        public string JobOverviewConnectionStringGaetan {
            get {
                return ((string)(this["JobOverviewConnectionStringGaetan"]));
            }
            set {
                this["JobOverviewConnectionStringGaetan"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=LMILBOR17-DE\\IP08R2;Initial Catalog=JobOverview;Integrated Security=T" +
            "rue")]
        public string JobOverviewConnectionStringLeo {
            get {
                return ((string)(this["JobOverviewConnectionStringLeo"]));
            }
            set {
                this["JobOverviewConnectionStringLeo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("JROUSSET")]
        public string EmployeeId {
            get {
                return ((string)(this["EmployeeId"]));
            }
            set {
                this["EmployeeId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=FLORIAN-PC\\SQLEXPRESS;Initial Catalog=JobOverview;Integrated Security" +
            "=True")]
        public string JobOverviewConnectionStringFloMaison {
            get {
                return ((string)(this["JobOverviewConnectionStringFloMaison"]));
            }
            set {
                this["JobOverviewConnectionStringFloMaison"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public string EmployeeId1 {
            get {
                return ((string)(this["EmployeeId1"]));
            }
            set {
                this["EmployeeId1"] = value;
            }
        }
    }
}
