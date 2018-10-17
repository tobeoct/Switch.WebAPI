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
    public class Channel
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
        public string Code { get; set; }

        [Required]
        [ProtoMember(4)]
        public string Description { get; set; }
    }
}