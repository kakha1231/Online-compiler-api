﻿using informaticsge.Dto.Request;
using informaticsge.Services;
using Microsoft.AspNetCore.Mvc;

namespace informaticsge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProblemsController : ControllerBase
{
    private readonly ProblemsService _problemsService;

    public ProblemsController(ProblemsService problemsService)
    {
        _problemsService = problemsService;
    }
    
    [HttpGet("/problems")]
    public async Task<IActionResult> GetProblems(int pagenumber)
    {
        var problems =  await _problemsService.GetAllProblems(pagenumber);

        return Ok(problems);
    }

    [HttpGet("/problems/{id}")]
    public async Task<IActionResult> GetProblem(int id)
    {
        var problem = await _problemsService.GetProblem(id);
        
        return Ok(problem);
    }
    
    [HttpGet("/problems/{id}/submissions")]
    public async Task<IActionResult> GetSubmissions(int id)
    {
        var submissions = await _problemsService.GetSubmissions(id);

        return Ok(submissions);
    }

    [HttpPost("/add-problem")]
    public async Task<IActionResult> AddProblem(AddProblemDto problem)
    {
        var addProblem=  await _problemsService.AddProblem(problem);

        return Ok(addProblem);
    }


    [HttpPut("/edit-problem")]
    public async Task<IActionResult> EditProblem(int id, AddProblemDto editProblem)
    {
        var edit = await _problemsService.EditProblem(id, editProblem);
        
        return Ok(edit);
    }
    
    

    
    
    
    
}