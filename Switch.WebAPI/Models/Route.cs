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

    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ProtoMember(1)]
        public int Id { get; set; }

        [Required]
        [ProtoMember(2)]
        public string Name { get; set; }
        [Required]
        [ProtoMember(3)]
        public int SinkNodeId { get; set; }

        [ProtoMember(4)]
        public SinkNode SinkNode { get; set; }

        [Required]
        [ProtoMember(5)]
        public long CardPan { get; set; }

        [Required]
        [ProtoMember(6)]
        public string Description { get; set; }
    }
}