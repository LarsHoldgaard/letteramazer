﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.6407
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 

namespace LetterAmazer.Business.Services.Model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class s3access
    {

        private string userField;

        private string keyidField;

        private string secretkeyField;

        private string bucketField;

        private string postqueueField;

        /// <remarks/>
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        public string keyid
        {
            get
            {
                return this.keyidField;
            }
            set
            {
                this.keyidField = value;
            }
        }

        /// <remarks/>
        public string secretkey
        {
            get
            {
                return this.secretkeyField;
            }
            set
            {
                this.secretkeyField = value;
            }
        }

        /// <remarks/>
        public string bucket
        {
            get
            {
                return this.bucketField;
            }
            set
            {
                this.bucketField = value;
            }
        }

        /// <remarks/>
        public string postqueue
        {
            get
            {
                return this.postqueueField;
            }
            set
            {
                this.postqueueField = value;
            }
        }
    }
}