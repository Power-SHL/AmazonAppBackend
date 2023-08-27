﻿using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services;
public interface IProfileService
{
    Task<Profile> GetProfile(string username);
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task DeleteProfile(string username);
}