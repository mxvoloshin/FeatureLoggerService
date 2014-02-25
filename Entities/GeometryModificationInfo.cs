using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FeatureLoggerService.Entities
{
    [DataContract]
    public class GeometryModificationInfo
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        [Required]
        public ModificationInfo Info { get; set; }
        [DataMember]
        [Required]
        public String WKTGeometry { get; set; }
    }
}
