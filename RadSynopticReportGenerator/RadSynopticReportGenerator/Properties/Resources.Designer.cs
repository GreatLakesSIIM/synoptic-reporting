﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RadSynopticReportGenerator.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RadSynopticReportGenerator.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RPID,LETTER_CODE,SHORT_NAME,LONG_NAME,AUTOMATED_SHORT_NAME,AUTOMATED_LONG_NAME,AUTOMATED_LONG_DESCRIPTION,STATUS,STATUS_REASON,STATUS_TEXT,CHANGE_TYPE,CHANGE_REASON_PUBLIC,EXPORTED_TO_LOINC,MODALITY,PLAYBOOK_TYPE,POPULATION,BODY_REGION,BODY_REGION_2,BODY_REGION_3,BODY_REGION_4,BODY_REGION_5,MODALITY_MODIFIER,MODALITY_MODIFIER_2,MODALITY_MODIFIER_3,PROCEDURE_MODIFIER,PROCEDURE_MODIFIER_2,ANATOMIC_FOCUS,ANATOMIC_FOCUS_2,LATERALITY,REASON_FOR_EXAM,REASON_FOR_EXAM_2,REASON_FOR_EXAM_3,TECHNIQUE,PHARMACEUTICAL,PH [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string complete_playbook_2_5 {
            get {
                return ResourceManager.GetString("complete_playbook_2_5", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name or Synonym,RID,Parent RID
        ///Radlex ontology entity,RID0,:THING
        ///RadLex entity,RID1,RID0
        ///image quality,RID10,RID35977
        ///imaging modality,RID10311,RID1
        ///magnetic resonance imaging,RID10312,RID10311
        ///magnetic resonance spectroscopy,RID10315,RID10311
        ///functional magnetic resonance imaging,RID10317,RID10312
        ///magnetic resonance angiography,RID10319,RID10312
        ///computed tomography,RID10321,RID28840
        ///quantitative computed tomography,RID10323,RID10321
        ///ultrasound,RID10326,RID10311
        ///nuclear medicine imaging,RID1033 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Radlex {
            get {
                return ResourceManager.GetString("Radlex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LoincNumber,LongCommonName,PartNumber,PartTypeName,PartName,PartSequenceOrder,RID,PreferredName,RPID,LongName
        ///17787-3,NM Thyroid gland Study report,LP199995-4,Rad.Anatomic Location.Region Imaged,Neck,A,RID7488,neck,,
        ///17787-3,NM Thyroid gland Study report,LP206648-0,Rad.Anatomic Location.Imaging Focus,Thyroid gland,A,RID7578,thyroid gland,,
        ///17787-3,NM Thyroid gland Study report,LP208891-4,Rad.Modality.Modality type,NM,A,RID10330,nuclear medicine imaging,,
        ///24531-6,US Retroperitoneum,LP207608-3,Rad.Modalit [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RADLEX_to_LOINC {
            get {
                return ResourceManager.GetString("RADLEX_to_LOINC", resourceCulture);
            }
        }
    }
}