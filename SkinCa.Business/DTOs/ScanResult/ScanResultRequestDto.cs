using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SkinCa.Business.DTOs;

public class ScanResultRequestDto
{
    public IFormFile Image { get; set;}
}