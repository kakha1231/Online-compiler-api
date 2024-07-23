﻿namespace informaticsge.models;

public class SubmissionResultDTO
{
    public bool Success { get; set; }
    public string? Input { set; get; }
    public string? ExpectedOutput { set; get; }
    public string? Output { get; set; } 
    public string? Status { get; set; }
    
}