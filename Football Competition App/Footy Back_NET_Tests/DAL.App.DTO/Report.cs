using System;
using System.ComponentModel.DataAnnotations;
using Domain.App.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Report: DomainEntityId
    {
        public Guid? AppUserId { get; set; }
        public string Submitter { get; set; } = default!;
        
        public DateTime Date { get; set; } = DateTime.Now;

        [MaxLength(Int32.MaxValue)] public string Title { get; set; } = default!;

        [MaxLength(Int32.MaxValue)] public string Comment { get; set; } = default!;
    }
}