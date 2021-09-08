using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace PublicApi.DTO.v1
{
    public class GameLength
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(HalfLength), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public int HalfLength { get; set; }
        
        [Display(Name = nameof(ExtraTimeHalfLength), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public int? ExtraTimeHalfLength { get; set; }

        /*
        public int? FirstHalfAdditionalTime { get; set; }
        
        public int? SecondHalfAdditionalTime { get; set; }
        
        public int? ExtraTimeFirstHalfAdditionalTime { get; set; }
        
        public int? ExtraTimeSecondHalfAdditionalTime { get; set; }
        */
    }
}