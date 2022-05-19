namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class ClaimDto
{
    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public DateTime GrantedTs { get; set; }
    public string GrantedByUsername { get; set; }

    public DateTime? RevokedTs { get; set; }
    public string RevokedByUsername { get; set; }
}