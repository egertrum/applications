using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using BLL.App.DTO.Identity;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Report: DomainEntityId
    {
        public Guid? AppUserId { get; set; }
        
        [Display(Name = nameof(Submitter), ResourceType = typeof(Base.Resources.DTO.v1.Report))] 
        public string? Submitter { get; set; }

        [Display(Name = nameof(Date), ResourceType = typeof(Base.Resources.DTO.v1.Report))] 
        public DateTime Date { get; set; } = DateTime.Now;
        
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Display(Name = nameof(Title), ResourceType = typeof(Base.Resources.DTO.v1.Report))] 
        public string Title { get; set; } = default!;

        [MaxLength(Int32.MaxValue, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Display(Name = nameof(Comment), ResourceType = typeof(Base.Resources.DTO.v1.Report))] 
        public string Comment { get; set; } = default!;
    }
}