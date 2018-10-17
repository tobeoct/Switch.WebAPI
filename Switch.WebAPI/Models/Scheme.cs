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
    public class Scheme
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
        public int TransactionTypeId { get; set; }

        [ProtoMember(4)]
        public TransactionType TransactionType { get; set; }

        [Required]
        [ProtoMember(5)]
        public int RouteId { get; set; }

        [ProtoMember(6)]
        public Route Route { get; set; }

        [ProtoMember(7)]
        [Required]
        public int ChannelId { get; set; }
        [ProtoMember(9)]
        public Channel Channel { get; set; }
        [ProtoMember(10)]
        [Required] public int FeeId { get; set; }
        [ProtoMember(11)]
        public Fee Fee { get; set; }
        [ProtoMember(12)]
        [Required] public string Description { get; set; }

    }
}