namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class ClaimDto
{
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;

    public DateTime GrantedTs { get; set; }
    public string GrantedByUsername { get; set; } = string.Empty;

    public DateTime? RevokedTs { get; set; }
    public string RevokedByUsername { get; set; } = string.Empty;
}