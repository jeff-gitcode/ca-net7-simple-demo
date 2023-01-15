using Domain;

using Infrastructure.DB;

public class UserSpecification : Specification<UserDTO>
{
    public UserSpecification(string email)
    {
        SetFilterCondition(x => x.Email == email);
    }
}