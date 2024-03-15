using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Common.Enums;

public enum RegionOfState
{
    [Display(Name="Bratislavský kraj")]
    Bratislavsky,
    
    [Display(Name="Trnavský kraj")]
    Trnavsky,

    [Display(Name="Trenčiansky kraj")]
    Trenciansky,
    
    [Display(Name="Nitriansky kraj")]
    Nitriansky,
    
    [Display(Name="Žilinský kraj")]
    Zilinsky,
    
    [Display(Name="Banskobystrický kraj")]
    Banskobystricky,
    
    [Display(Name="Prešovský kraj")]
    Presovsky,
    
    [Display(Name="Košický kraj")]
    Kosicky
}