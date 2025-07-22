namespace Auth.Service.DTOs.Homeworks.PointTransactionsDto;

public class PointTransactionForCreationDto
{
    public long UserId { get; set; }
    public int Points { get; set; }
    public string Reason { get; set; }
}
