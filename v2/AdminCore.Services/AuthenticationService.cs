﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="AdminCore">
//   AdminCore
// </copyright>
// <summary>
//   The hello service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminCore.Services
{
  using System;
  using System.IdentityModel.Tokens.Jwt;
  using System.Linq;
  using System.Security.Claims;
  using System.Text;

  using AdminCore.Common.Interfaces;

  using AdminCore.DAL;
  using AdminCore.DTOs;

  using AutoMapper;

  using Microsoft.IdentityModel.Tokens;

  /// <summary>
  /// The hello service.
  /// </summary>
  public class AuthenticationService : IAuthenticationService
  {
    /// <summary>
    /// The _database context.
    /// </summary>
    private readonly IDatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
    /// </summary>
    /// <param name="databaseContext">
    /// The database context.
    /// </param>
    public AuthenticationService(IDatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }

    /// <summary>
    /// The jwt sign in.
    /// </summary>
    /// <param name="email">
    /// The email.
    /// </param>
    /// <param name="password">
    /// The password.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    /// <exception cref="NotImplementedException">
    /// </exception>
    public JwtAuthDto JwtSignIn(string email, string password)
    {
      var employee = _databaseContext.EmployeeRepository.Get(x => x.Email.Equals(email)).FirstOrDefault();

      if (employee == null)
      {
        return null;
      }

 /*     var decodedPassword = DecodePassword(password);
      if (!decodedPassword.Equals(password))
      {
        return null;
      }*/

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("veryVerySecretKey");

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity 
        (
          new[]
          {
                new Claim(
                  ClaimTypes.Name,
                  employee.EmployeeId.ToString())
          }
        ),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials
        (
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature
        )
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var jwtResponseDto = new JwtAuthDto
      {
        AccessToken = tokenHandler.WriteToken(token), 
        TokenType = "Bearer"
      };
      return jwtResponseDto;
    }
  }
}