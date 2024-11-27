namespace BCSH2BDAS2.Models;

public interface IUser
{
    public Role? Role { get; set; }

    public bool HasMaintainerRights() => Role?.Prava is 3 or 6;

    public bool HasDispatchRights() => Role?.Prava is 4 or 6;

    public bool HasManagerRights() => Role?.Prava is 5 or 6;

    public bool HasAdminRights() => Role?.Prava == 6;

    public bool HasWorkerRights() => Role?.Prava is 2 or 6;
}