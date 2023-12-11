using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.Web.Validation;

namespace FirstResponder.Web.ViewModels;

public class AedFormViewModel
{
    public AedFormDTO AedFormDTO { get; set; } = new AedFormDTO();
    
    [Display(Name = "Banner image")]
    [AllowedMimeTypes("image/jpeg", "image/png")]
    public IFormFile? AedPhotoFormFile { get; set; }
    
    // Edit Aed fields
    public string[]? AedPhotosToDelete { get; set; }
    
}