﻿namespace SkinCa.Business.DTOs;

public class DiseaseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string UserId { get; set; }
    public byte[] Image { get; set; }
    public string Specialty { get; set; }
    public string Symptoms { get; set; }
    public string? Types { get; set; }
    public string? Causes { get; set; }
    public string? DiagnosticMethods { get; set; }
    public string? Prevention { get; set; }
}
