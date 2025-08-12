﻿using System.ComponentModel.DataAnnotations;

namespace SkinCa.Business.DTOs;

public class ScanResultRequestDto
{
    public bool GotCancer { get; set; }
    public byte[] Data { get; set;} 
    [Range(0,100)]
    public short Confidence { get; set; }
}