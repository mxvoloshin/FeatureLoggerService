using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FeatureLoggerService.Entities
{
    [DataContract]
    public enum ModifyState
    {
        [EnumMember]
        None = -1,
        [EnumMember]
        Inserted = 0,
        [EnumMember]
        Modified,
        [EnumMember]
        Deleted
    }

    [DataContract]
    public class ModificationInfo
    {
        [DataMember]
        public long ID { get; set; }
        [Required]
        [DataMember]
        public string UserName { get; set; }
        [Required]
        [DataMember]
        public DateTime ModifyTime { get; set; }
        [Required]
        [DataMember]
        public long FID { get; set; }
        [Required]
        [DataMember]
        public string FeatureClass { get; set; }
        [Required]
        [DataMember]
        public ModifyState State { get; set; }
        [DataMember]
        public virtual List<SemanticsModificationInfo> SemanticsInfo { get; set; }
        [DataMember]
        public virtual GeometryModificationInfo GeometryInfo { get; set; }
    }
    
    [DataContract]
    public class ModificationInfoDTO
    {
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<ModificationInfo> Infos { get; set; }
    }
}
