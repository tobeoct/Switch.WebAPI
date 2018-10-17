using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ProtoBuf;

namespace Switch.WebAPI.Models
{
    [ProtoContract]
    public class Fee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ProtoMember(1)]
        public int Id { get; set; }

        [Required]
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public double? FlatAmount { get; set; }

        [ProtoMember(4)]
        public double? PercentOfTrx { get; set; }
        [ProtoMember(5)]
        public double? Minimum { get; set; }
        [ProtoMember(6)]
        public double? Maximum { get; set; }
    }
}