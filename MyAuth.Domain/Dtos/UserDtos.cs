using System;
using System.Collections.Generic;
using System.Text;

namespace MyAuth.Domain.Dtos;

// Objeto limpo para devolver ao front-end (sem senha)
public record UserDto(string Username, List<string> Roles, List<string> Permissions);