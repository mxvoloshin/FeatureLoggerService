using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FeatureLoggerService.Entities
{
    [DataContract]
    public class SemanticsModificationInfo
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        [Required]
        public ModificationInfo Info { get; set; }
        [DataMember]
        [Required]
        public string Attribute { get; set; }
        [DataMember]
        [Required]
        public string Value { get; set; }
        
    }
}
