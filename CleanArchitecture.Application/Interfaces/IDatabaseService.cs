﻿using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IDatabaseService
    {
        int CreateUser(User user);
    }
}
