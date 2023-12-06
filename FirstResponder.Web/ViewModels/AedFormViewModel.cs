using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.Web.Validation;

namespace FirstResponder.Web.ViewModels;

public class AedFormViewModel : AedFormDTO
{
    [Display(Name = "Banner image")]
    [AllowedMimeTypes("image/jpeg", "image/png")]
    public IFormFile? AedPhotoFormFile { get; set; }
}