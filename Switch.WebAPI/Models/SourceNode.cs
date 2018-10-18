using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ProtoBuf;
using Switch.WebAPI.Logics;

namespace Switch.WebAPI.Models
{
    [ProtoContract(IgnoreListHandling = true)]
    public class SourceNode 
    {
        [ProtoMember(1)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ProtoMember(2)]
        public string Name { get; set; }

        [Required]
        [ProtoMember(3)]
        public string HostName { get; set; }

        [Required]
        [ProtoMember(4)]
        public string IPAddress { get; set; }

        [Required]
        [ProtoMember(5)]
        public string Port { get; set; }

        [Required]
        [ProtoMember(6)]
        public string Status { get; set; }

        [Required]
        [ProtoMember(7)]
        public int SchemeId { get; set; }
        [ProtoMember(8)]
        public Scheme Scheme { get; set; }
    }
}