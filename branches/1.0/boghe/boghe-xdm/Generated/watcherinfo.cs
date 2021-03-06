﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace BogheXdm.Generated.watcherinfo {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:ietf:params:xml:ns:watcherinfo", IsNullable=false)]
    public partial class watcherinfo {
        
        private watcherlist[] watcherlistField;
        
        private System.Xml.XmlElement[] anyField;
        
        private string versionField;
        
        private watcherinfoState stateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("watcher-list", Order=0)]
        public watcherlist[] watcherlist {
            get {
                return this.watcherlistField;
            }
            set {
                this.watcherlistField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order=1)]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="nonNegativeInteger")]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public watcherinfoState state {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    [System.Xml.Serialization.XmlRootAttribute("watcher-list", Namespace="urn:ietf:params:xml:ns:watcherinfo", IsNullable=false)]
    public partial class watcherlist {
        
        private watcher[] watcherField;
        
        private System.Xml.XmlElement[] anyField;
        
        private string resourceField;
        
        private string packageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("watcher", Order=0)]
        public watcher[] watcher {
            get {
                return this.watcherField;
            }
            set {
                this.watcherField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order=1)]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
        public string resource {
            get {
                return this.resourceField;
            }
            set {
                this.resourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string package {
            get {
                return this.packageField;
            }
            set {
                this.packageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:ietf:params:xml:ns:watcherinfo", IsNullable=false)]
    public partial class watcher {
        
        private string displaynameField;
        
        private watcherStatus statusField;
        
        private watcherEvent eventField;
        
        private ulong expirationField;
        
        private bool expirationFieldSpecified;
        
        private string idField;
        
        private ulong durationsubscribedField;
        
        private bool durationsubscribedFieldSpecified;
        
        private string langField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("display-name")]
        public string displayname {
            get {
                return this.displaynameField;
            }
            set {
                this.displaynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public watcherStatus status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public watcherEvent @event {
            get {
                return this.eventField;
            }
            set {
                this.eventField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong expiration {
            get {
                return this.expirationField;
            }
            set {
                this.expirationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationSpecified {
            get {
                return this.expirationFieldSpecified;
            }
            set {
                this.expirationFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("duration-subscribed")]
        public ulong durationsubscribed {
            get {
                return this.durationsubscribedField;
            }
            set {
                this.durationsubscribedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool durationsubscribedSpecified {
            get {
                return this.durationsubscribedFieldSpecified;
            }
            set {
                this.durationsubscribedFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
        public string lang {
            get {
                return this.langField;
            }
            set {
                this.langField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType="anyURI")]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    public enum watcherStatus {
        
        /// <remarks/>
        pending,
        
        /// <remarks/>
        active,
        
        /// <remarks/>
        waiting,
        
        /// <remarks/>
        terminated,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    public enum watcherEvent {
        
        /// <remarks/>
        subscribe,
        
        /// <remarks/>
        approved,
        
        /// <remarks/>
        deactivated,
        
        /// <remarks/>
        probation,
        
        /// <remarks/>
        rejected,
        
        /// <remarks/>
        timeout,
        
        /// <remarks/>
        giveup,
        
        /// <remarks/>
        noresource,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ietf:params:xml:ns:watcherinfo")]
    public enum watcherinfoState {
        
        /// <remarks/>
        full,
        
        /// <remarks/>
        @partial,
    }
}
