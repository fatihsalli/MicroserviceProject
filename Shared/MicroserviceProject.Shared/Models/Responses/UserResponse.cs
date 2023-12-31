﻿namespace MicroserviceProject.Shared.Models.Responses;

public class UserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<AddressResponse> Addresses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
}