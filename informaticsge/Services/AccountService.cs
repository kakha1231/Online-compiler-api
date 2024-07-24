﻿using informaticsge.Dto;
using informaticsge.Dto.Request;
using informaticsge.Entity;
using informaticsge.JWT;
using informaticsge.Models;
using Microsoft.AspNetCore.Identity;


namespace informaticsge.Services;

public class AccountService
{
    private AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JWTService _jwtService;

    public AccountService(AppDbContext appDbContext, UserManager<User> userManager, SignInManager<User> signInManager, JWTService jwtService)
    {
        _appDbContext = appDbContext;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _userManager = userManager;
    }
    
    public async Task<string> Register(RegistrationDto newuser)
    {
        
       if (await CheckEmailExists(newuser.Email))
       {
           return "email already here";
       }
     
       //createasync method does it for u but i added it for clarity
       if (await CheckUsernameExists(newuser.UserName))  
       {
           return "username already here";
       }
       
       var userToAdd = new User()
       {
           Email = newuser.Email,
           UserName = newuser.UserName,
       };
       
       //errors are quite tricky hard to debug in case of errors use debugger 
       var createprocess = await _userManager.CreateAsync(userToAdd, newuser.Password);
        
       if (!createprocess.Succeeded)
       {
           //made for logging createasync errors not best option but does its job well
           var errors = string.Join("\n ", createprocess.Errors.Select(e=>e.Description));
           return errors;
       }
       return "user added successfully";
    }

    
    public async Task<string> Login(UserLoginDto userLogin)
    {
        var user = await _userManager.FindByEmailAsync(userLogin.Email);
        
        if (user == null)
        {
            return "Invalid Email or Password";
        }

        var checkpass = await _userManager.CheckPasswordAsync(user, userLogin.Password);

        if (checkpass == false)
        {
            return "Invalid Email or Password";
        }
        
        var jwt = _jwtService.CreateJwt(user);

        return jwt;
    }

    //santas little helpers
    private async Task<bool> CheckEmailExists(string email)
    {
      var result =  await _userManager.FindByEmailAsync(email);
     
      if (result == null)
      {
          return false;
      }
      return true;
    }
    
//i could do it more fancy way but it works sooo... 
    private async Task<bool> CheckUsernameExists(string username)
    {
        var result = await _userManager.FindByNameAsync(username);
        if (result == null)
        {
            return false;
        }
        return true;
    }
    
}