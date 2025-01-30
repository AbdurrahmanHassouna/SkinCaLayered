﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SkinCa.Common;
using SkinCa.Common.CostumValidationAttributes;

namespace SkinCa.Business.DTOs.User;

public class ProfileRequestDto
{
    [StringLength(50,MinimumLength = 5)]
    public string FirstName { get; set; }
    [StringLength(100,MinimumLength = 5)]
    public string LastName { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    [Birthdate]
    public DateTime BirthDate { get; set; }
    public Governorate Governorate { get; set; }
    [RegularExpression(@"^-(?:\d{1,2}|1[0-7]\d|180)(\.\d{1,10})?|^\d{1,2}(\.\d{1,10})?$", ErrorMessage = "Invalid Latitude.")]
    public double? Latitude { get; set; }
    [RegularExpression(@"^-(?:\d{1,2}|1[0-7]\d|180)(\.\d{1,10})?|^\d{1,2}(\.\d{1,10})?$", ErrorMessage = "Invalid Longitude.")]
    public double? Longitude { get; set; }
    [StringLength(200)]
    public string? Address { get; set; }
    [EmailAddress, StringLength(50)]
    public string Email { get; set; }
    [Image(["JPEG","PNG","GIF"])]
    public IFormFile? ProfilePicture { get; set; }
}