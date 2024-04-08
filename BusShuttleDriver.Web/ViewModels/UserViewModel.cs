public class UserViewModel
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public bool IsActive { get; set; }
    public string? Role { get; set; } // Assuming a user has only one role for simplicity
}
