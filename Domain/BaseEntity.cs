namespace Domain;
public class BaseEntity
{
    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; private set; }

    public BaseEntity() => this.ModifiedDate = DateTime.Now;
}